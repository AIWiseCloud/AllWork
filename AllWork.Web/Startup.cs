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

        //ΪAutofac�����ӵ�ConfigureContainer������û�б����ã����Ѿ����Խ�����������ﲢʵ�ֵķ����ע�ᣬ
        //�������Ϊ������Program��ָ����UseServiceProviderFactory
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            //����һ��ÿ��һ���񼰽ӿ�ʵ�ֶ�Ҫ�ֶ�ע��һ�£������ã�
            //containerBuilder.RegisterType<UserServices>().As<IUserServices>().InstancePerLifetimeScope().AsImplementedInterfaces();
            //containerBuilder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope().AsImplementedInterfaces();
            //��������ʹ�÷���ķ�ʽ��ʵ�ַ����ע��
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
            //���������ٷ��Ƽ��ķ�ʽ�� �����ļ���ע�᷽ʽ�������ԣ�
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //������
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(WebApiResultMiddleware)); //ͨ���Զ��������ͳһ�������ݵĸ�ʽ
                options.Filters.Add(typeof(CustomerGlobalExceptionFilterAsync)); //����쳣���������(Nlog��־��
                options.Filters.Add(typeof(ModelValidateActionFilterAttribute));//ģ����֤
            });

            //��IConfiguration�е���չ�������������ݿ����ӣ���ǰֻ������mysql��, roy 2021.08.17)
            Configuration.LoadMySqlConnection();

            services.Configure<TokenManagement>(Configuration.GetSection("tokenManagement"));//��ȡtoken������Ϣ
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

           

            //��װMicrosoft.AspNetCore.Mvc.NewtonsoftJson����ӣ����ǰ��ʵ���������ַ�������ת��Ϊint32֮��Ĵ���
            services.AddControllers().AddNewtonsoftJson();

            services.AddSingleton<INLogHelper, NLogHelper>();//��־
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
                    Description = "AllWorkƽ̨ API�����ĵ�",
                });
                //����XML�ĵ������·��
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//��ȡӦ�ó�������Ŀ¼
                c.IncludeXmlComments(Path.Combine(basePath, "AllWork.Web.xml"), true);
                c.IncludeXmlComments(Path.Combine(basePath, "AllWork.Model.xml"), true); //ʵ��ע���ļ�Ҳ��������

                

                #region JWT��֤Swagger��Ȩ
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT��Ȩ(���ݽ�������ͷheader�н��д���) ֱ�����¿�������Bearer {token}���м��ǿո�",
                    Name = "Authorization",    // jwtĬ�ϵĲ������� ������Ҳ����ΪX-token) �ͻ�������token����������ƣ��� obj.header["X-Token"] = `Bearer ${token ? token : ''}`;
                    In = ParameterLocation.Header,// jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
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

            app.UseAuthentication();//������֤(jwt)
            app.UseRouting();

            app.UseAuthorization();//��Ȩ

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AllWork API V1");
                c.RoutePrefix = "api";// �����Ϊ�գ�����·�����Ǹ�����/index.html������Ϊ�գ���ʾֱ���ڸ��������ʣ��뻻һ��·����ֱ��д���ּ��ɣ�����ֱ��дc.RoutePrefix = "swagger"; �����·��Ϊ ������/swagger/index.html
            });
            app.UseStaticFiles();//����wwwroot�µľ�̬��Դ
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
