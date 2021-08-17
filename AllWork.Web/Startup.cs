using AllWork.Nlog.Log;
using AllWork.Web.Auth;
using AllWork.Web.Filter;
using AllWork.Web.Helper;
using AllWork.Web.Helper.Redis;
using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Reflection;
using System.Text;

namespace AllWork.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //为Autofac自增加的ConfigureContainer方法并没有被引用，但已经可以进到这个方法里并实现的服务的注册，
        //这就是因为我们在Program中指定了UseServiceProviderFactory
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            //方法一：每增一服务及接口实现都要手动注册一下（不常用）
            //containerBuilder.RegisterType<UserServices>().As<IUserServices>().InstancePerLifetimeScope().AsImplementedInterfaces();
            //containerBuilder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope().AsImplementedInterfaces();
            //方法二：使用反射的方式来实现服务的注册
            Assembly service = Assembly.Load("AllWork.Services");
            Assembly iservice = Assembly.Load("AllWork.IServices");
            Assembly repository = Assembly.Load("AllWork.Repository");
            Assembly irepository = Assembly.Load("AllWork.IRepository");
            containerBuilder.RegisterAssemblyTypes(service, iservice)
                .Where(t => t.FullName.EndsWith("Services") && !t.IsAbstract)
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces()
                .PropertiesAutowired();
            containerBuilder.RegisterAssemblyTypes(repository, irepository)
              .Where(t => t.FullName.EndsWith("Repository") && !t.IsAbstract)
              .InstancePerLifetimeScope()
              .AsImplementedInterfaces()
              .PropertiesAutowired();
            //方法三：官方推荐的方式， 配置文件的注册方式，更灵活（略）
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //过滤器
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(WebApiResultMiddleware)); //通过自定义过滤器统一返回数据的格式
                options.Filters.Add(typeof(CustomerGlobalExceptionFilterAsync)); //添加异常处理过滤器(Nlog日志）
                options.Filters.Add(typeof(ModelValidateActionFilterAttribute));//模型验证
            });

            //将IConfiguration中的扩展方法，加载数据库连接（当前只加载了mysql的, roy 2021.08.17)
            Configuration.LoadMySqlConnection();

            services.Configure<TokenManagement>(Configuration.GetSection("tokenManagement"));//读取token配置信息
            var token = Configuration.GetSection("tokenManagement").Get<TokenManagement>();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token.Secret)),
                    ValidIssuer = token.Issuer,
                    ValidAudience = token.Audience,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

           

            //安装Microsoft.AspNetCore.Mvc.NewtonsoftJson后添加（解决前端实体中数字字符串不能转换为int32之类的错误
            services.AddControllers().AddNewtonsoftJson();

            services.AddSingleton<INLogHelper, NLogHelper>();//日志
            services.AddScoped<IAuthenticateService, AuthenticationService>();

            services.AddHttpClient(); //HttpClient
            RedisClient.redisClient.InitConnect(Configuration); //Redis
            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "AllWork API",
                    Version = "V1",
                    Description = "AllWork平台 API描述文档",
                });
                //配置XML文档的输出路径
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录
                c.IncludeXmlComments(Path.Combine(basePath, "AllWork.Web.xml"), true);
                c.IncludeXmlComments(Path.Combine(basePath, "AllWork.Model.xml"), true); //实体注释文件也包括进来

                

                #region JWT认证Swagger授权
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头header中进行传输) 直接在下框中输入Bearer {token}（中间是空格）",
                    Name = "Authorization",    // jwt默认的参数名称 （有人也定义为X-token) 客户端设置token用这里的名称，如 obj.header["X-Token"] = `Bearer ${token ? token : ''}`;
                    In = ParameterLocation.Header,// jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
                #endregion
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();

            app.UseAuthentication();//增加认证(jwt)
            app.UseRouting();

            app.UseAuthorization();//授权

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AllWork API V1");
                c.RoutePrefix = "api";// 如果设为空，访问路径就是根域名/index.html，设置为空，表示直接在根域名访问；想换一个路径，直接写名字即可，比如直接写c.RoutePrefix = "swagger"; 则访问路径为 根域名/swagger/index.html
            });
            app.UseStaticFiles();//访问wwwroot下的静态资源
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
