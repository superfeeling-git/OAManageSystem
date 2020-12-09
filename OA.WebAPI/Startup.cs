using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using OA.Model;
using Microsoft.EntityFrameworkCore;
using OA.Model.Entity;
using Autofac;
using Microsoft.OpenApi.Models;
using Microsoft.DotNet.PlatformAbstractions;
using System.IO;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace OA.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// {ApiName} �����ȫ�ֱ����������޸�
        /// </summary>
        public string ApiName { get; set; } = "Blog.Core";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(options => {
                    //�޸��������Ƶ����л���ʽ������ĸСд
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                    //�޸�ʱ������л���ʽ
                    options.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy/MM/dd HH:mm:ss" });
                });

            //������ݿ�������
            services.AddDbContext<OADbContext>(option => {
                option.UseSqlServer(Configuration.GetConnectionString("Default"));
            });

            //���JSON���л����룬��ֹ���ı�����ΪUnicode�ַ���
            //services.AddControllers().AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            //});

            //����Ĭ�ϵ�Identity����ǿ��
            services.Configure<IdentityOptions>(options => {
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            });


            //���Identity����
            services.AddIdentity<OmsUser, OmsRoles>()
                .AddEntityFrameworkStores<OADbContext>();

            var Issurer = "JWTBearer.Auth";  //������
            var Audience = "api.auth";       //������
            var secretCredentials = "q2xiARx$4x3TKqBJ";   //��Կ

            //������֤����
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o => {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                        //�Ƿ���֤������
                        ValidateIssuer = true,
                    ValidIssuer = Issurer,//������

                        //�Ƿ���֤������
                        ValidateAudience = true,
                    ValidAudience = Audience,//������

                        //�Ƿ���֤��Կ
                        ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretCredentials)),

                    ValidateLifetime = true, //��֤��������
                        RequireExpirationTime = true //����ʱ��
                    };
            });

            services.AddAuthorization(options => {
                options.AddPolicy("BlogController", builder => {
                    builder.Requirements.Add(new PermissionRequirement());
                });
            });

            //services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();

            var basePath = ApplicationEnvironment.ApplicationBasePath;
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("V1", new OpenApiInfo
                {
                    // {ApiName} �����ȫ�ֱ����������޸�
                    Version = "V1",
                    Title = $"{ApiName} �ӿ��ĵ�����Netcore 3.0",
                    Description = $"{ApiName} HTTP API V1",
                    Contact = new OpenApiContact { Name = ApiName, Email = "Blog.Core@xxx.com", Url = new Uri("https://www.jianshu.com/u/94102b59cc2a") },
                    License = new OpenApiLicense { Name = ApiName, Url = new Uri("https://www.jianshu.com/u/94102b59cc2a") }
                });
                c.OrderActionsBy(o => o.RelativePath);

                //�����������������������
                var xmlPath = Path.Combine(basePath, "OA.WebAPI.xml");//������Ǹո����õ�xml�ļ���
                c.IncludeXmlComments(xmlPath, true);//Ĭ�ϵĵڶ���������false�������controller��ע�ͣ��ǵ��޸�//�����������������������
                
                var xmlPath_Model = Path.Combine(basePath, "OA.Model.xml");//������Ǹո����õ�xml�ļ���
                c.IncludeXmlComments(xmlPath_Model, true);//Ĭ�ϵĵڶ���������false�������controller��ע�ͣ��ǵ��޸�


                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                #region Token�󶨵�ConfigureServices
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) ֱ�����¿�������Bearer {token}��ע������֮����һ���ո�\"",
                    Name = "Authorization",//jwtĬ�ϵĲ�������
                    In = ParameterLocation.Header,//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
                    Type = SecuritySchemeType.ApiKey
                });
                #endregion
            });
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<ConfigureAutofac>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/V1/swagger.json", $"{ApiName} V1");

                //·�����ã�����Ϊ�գ���ʾֱ���ڸ�������localhost:8001�����ʸ��ļ�,
                //ע��localhost:8001/swagger�Ƿ��ʲ����ģ�ȥlaunchSettings.json��launchUrlȥ����
                //������뻻һ��·����ֱ��д���ּ��ɣ�����ֱ��дc.RoutePrefix = "doc";
                c.DocumentTitle = "SparkTodo API";
                c.RoutePrefix = "";
            });

            app.UseRouting();

            app.UseAuthentication();    //��������֤�м��

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }


    public class PermissionRequirement : IAuthorizationRequirement
    {
    }

    public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>, IAuthorizationHandler
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var endpoint = context.Resource as RouteEndpoint;

            var descriptor = endpoint?.Metadata?
                .SingleOrDefault(md => md is ControllerActionDescriptor) as ControllerActionDescriptor;

            if (descriptor == null)
                throw new InvalidOperationException("Unable to retrieve current action descriptor.");

            var _controllerName = descriptor.ControllerName;
            var _actionName = descriptor.ActionName;

            string name = context.User.Identity.Name;

            var role = context.User.FindFirst(c => c.Type == ClaimTypes.Role);
            if (role != null)
            {
                var roleValue = role.Value;
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
