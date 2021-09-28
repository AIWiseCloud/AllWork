using AllWork.Nlog.Log;
using AllWork.Web.Auth;
using AllWork.Web.Filter;
using AllWork.Web.Helper;
using AllWork.Web.Helper.Redis;
using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UEditorNetCore;

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

            //配置允许跨域
            services.AddCors(options =>
            {
                options.AddPolicy("any",
                    builder => builder.AllowAnyOrigin()
                    .WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS")
                    );
            });


            //百度编辑器
            services.AddUEditorService();

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
                //以下为遇到401Error: Unauthorized时的处理，让客户端能获取到需要的信息（若不处理，则客户端不好捕获)
                x.Events = new JwtBearerEvents
                {
                    //此处为权限验证失败后触发的事件
                    OnChallenge = context =>
                    {
                        //此处代码为终止.Net Core默认的返回类型和数据结果，这个很重要哦，必须
                        context.HandleResponse();
                        var res = new { code = 401, msg = "401Error: Unauthorized 错误，未经授权", result = false, returnStatus = 0 };//与WebApiResultMiddleware中的返回格式保持一致
                        context.Response.ContentType = "application/json";
                        //自定义返回状态码，默认为401 这里改成 200
                        context.Response.StatusCode = StatusCodes.Status200OK; //StatusCodes.Status401Unauthorized;
                        context.Response.WriteAsync(JsonConvert.SerializeObject(res));
                        return Task.FromResult(0);
                    }
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

            services.Configure<FormOptions>(x =>
            {
                //最大200M  (上传app包文件出现Request body too large而设） 可以直接在接口那里取消限制[DisableRequestSizeLimit]
                x.MultipartBodyLengthLimit = 209_715_200;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseHttpsRedirection(); //这句加上会出现http自动切换为https的情况
            //以上不注销无问题，注销了用了阿里云http, 然后取消注销导致vue项目net::ERR_CERT_COMMON_NAME_INVALID错误，应是vue在此要转为https

            app.UseAuthentication();//增加认证(jwt)
            app.UseRouting();

            app.UseAuthorization();//授权
            app.UseCors("any"); // 跨域请求（放在app.UseAuthorization()后, 且在UseRouting后，UseEndpoints前)

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AllWork API V1");
                c.RoutePrefix = "api";// 如果设为空，访问路径就是根域名/index.html，设置为空，表示直接在根域名访问；想换一个路径，直接写名字即可，比如直接写c.RoutePrefix = "swagger"; 则访问路径为 根域名/swagger/index.html
            });



            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = new FileExtensionContentTypeProvider(new Dictionary<string, string>
{
{ ".apk","application/vnd.android.package-archive"},
{ ".nupkg","application/zip"}
})
            });
            app.UseStaticFiles();//访问wwwroot下的静态资源


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
