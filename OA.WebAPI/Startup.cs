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
        /// {ApiName} 定义成全局变量，方便修改
        /// </summary>
        public string ApiName { get; set; } = "Blog.Core";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(options => {
                    //修改属性名称的序列化方式，首字母小写
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                    //修改时间的序列化方式
                    options.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy/MM/dd HH:mm:ss" });
                });

            //添加数据库上下文
            services.AddDbContext<OADbContext>(option => {
                option.UseSqlServer(Configuration.GetConnectionString("Default"));
            });

            //添加JSON序列化编码，防止中文被编码为Unicode字符。
            //services.AddControllers().AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            //});

            //设置默认的Identity密码强度
            services.Configure<IdentityOptions>(options => {
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            });


            //添加Identity服务
            services.AddIdentity<OmsUser, OmsRoles>()
                .AddEntityFrameworkStores<OADbContext>();

            var Issurer = "JWTBearer.Auth";  //发行人
            var Audience = "api.auth";       //受众人
            var secretCredentials = "q2xiARx$4x3TKqBJ";   //密钥

            //配置认证服务
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o => {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                        //是否验证发行人
                        ValidateIssuer = true,
                    ValidIssuer = Issurer,//发行人

                        //是否验证受众人
                        ValidateAudience = true,
                    ValidAudience = Audience,//受众人

                        //是否验证密钥
                        ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretCredentials)),

                    ValidateLifetime = true, //验证生命周期
                        RequireExpirationTime = true //过期时间
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
                    // {ApiName} 定义成全局变量，方便修改
                    Version = "V1",
                    Title = $"{ApiName} 接口文档——Netcore 3.0",
                    Description = $"{ApiName} HTTP API V1",
                    Contact = new OpenApiContact { Name = ApiName, Email = "Blog.Core@xxx.com", Url = new Uri("https://www.jianshu.com/u/94102b59cc2a") },
                    License = new OpenApiLicense { Name = ApiName, Url = new Uri("https://www.jianshu.com/u/94102b59cc2a") }
                });
                c.OrderActionsBy(o => o.RelativePath);

                //就是这里！！！！！！！！！
                var xmlPath = Path.Combine(basePath, "OA.WebAPI.xml");//这个就是刚刚配置的xml文件名
                c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改//就是这里！！！！！！！！！
                
                var xmlPath_Model = Path.Combine(basePath, "OA.Model.xml");//这个就是刚刚配置的xml文件名
                c.IncludeXmlComments(xmlPath_Model, true);//默认的第二个参数是false，这个是controller的注释，记得修改


                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                #region Token绑定到ConfigureServices
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
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

                //路径配置，设置为空，表示直接在根域名（localhost:8001）访问该文件,
                //注意localhost:8001/swagger是访问不到的，去launchSettings.json把launchUrl去掉，
                //如果你想换一个路径，直接写名字即可，比如直接写c.RoutePrefix = "doc";
                c.DocumentTitle = "SparkTodo API";
                c.RoutePrefix = "";
            });

            app.UseRouting();

            app.UseAuthentication();    //添加身份认证中间件

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
