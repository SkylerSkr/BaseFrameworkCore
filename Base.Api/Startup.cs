using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Base.Api.AuthHelper.OverWrite;
using Base.Api.Error;
using Base.Api.Filters;
using Base.Api.Interceptor;
using Base.Api.Log;
using Base.Common.Redis;
using Base.SDK.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog.Extensions.Logging;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Base.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc(config =>
            {
                config.Filters.Add(new ApiActionFilterAttribute());
                //config.Filters.Add(new ApiErrorFilterAttribute());

            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddControllersAsServices();

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v0.1.0",
                    Title = "Base.Core API",
                    Description = "框架说明文档",
                    TermsOfService = "None",
                    //Contact = new Swashbuckle.AspNetCore.Swagger.Contact { Name = "Base.Core", Email = "Base.Core@xxx.com", Url = "https://www.jianshu.com/u/94102b59cc2a" }
                });

                //swagger注释
                var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "Base.Api.xml");//这个就是刚刚配置的xml文件名
                c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改

                #region Token绑定到ConfigureServices
                //添加header验证信息
                //c.OperationFilter<SwaggerHeader>();
                var security = new Dictionary<string, IEnumerable<string>> { { "Base.Core", new string[] { } }, };
                c.AddSecurityRequirement(security);
                //方案名称“Blog.Core”可自定义，上下一致即可
                c.AddSecurityDefinition("Base.Core", new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });
                #endregion
            });

            #endregion

            #region MyRegion
            services.AddAuthorization(options =>
            {
                options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));
            });
            #endregion



            return RegisterAutofac(services);//注册Autofac
        }

        private IServiceProvider RegisterAutofac(IServiceCollection services)
        {
            //实例化Autofac容器
            var builder = new ContainerBuilder();
            //将Services中的服务填充到Autofac中
            builder.Populate(services);

            //redis注入
            //services.AddSingleton<IRedisCacheManager, RedisCacheManager>();
            builder.RegisterType<RedisCacheManager>().As<IRedisCacheManager>();

            #region 事务拦截器注入
            builder.RegisterType<RedisCacheAOPInterceptor>();
            builder.RegisterType<TransactionInterceptor>();//可以直接替换其他拦截器！一定要把拦截器进行注册

            var assemblysServices = Assembly.Load("Base.BusinessService");

            //builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces();//指定已扫描程序集中的类型注册为提供所有其实现的接口。
            builder.RegisterAssemblyTypes(assemblysServices)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                //引用Autofac.Extras.DynamicProxy;
                .EnableInterfaceInterceptors()
               //可以直接替换拦截器 使用redis全局缓存
               //.InterceptedBy(typeof(RedisCacheAOPInterceptor), typeof(TransactionInterceptor));
               .InterceptedBy(typeof(TransactionInterceptor));//不使用redis全局缓存
            #endregion


            //新模块组件注册    
            builder.RegisterModule<DefaultModule>();
            //创建容器
            var Container = builder.Build();
            //第三方IOC接管 core内置DI容器 
            return new AutofacServiceProvider(Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            app.UseErrorHandling();

            //app.UseJwtTokenAuth();
            app.UseMiddleware<JwtTokenAuth>();

            //使用NLog作为日志记录工具
            loggerFactory.AddNLog();
            //引入Nlog配置文件
            env.ConfigureNLog("nlog.config");

            //异常处理
            app.UseExceptionHandler(builder =>
            {

                builder.Run(async context =>
                {


                    context.Response.StatusCode = StatusCodes.Status200OK;
                    context.Response.ContentType = "application/json;charset=utf-8";
                    var ex = context.Features.Get<IExceptionHandlerFeature>();
                    var result = new ExceptionResponse
                    {
                        ErrCode = 500,
                        ErrMsg = ex?.Error?.Message,
                        BizErrorMsg = ex?.Error?.Message
                    };

                    using (var requestSm = context.Request.Body)
                    {
                        var requestMethod = context.Request.Method;
                        var requestHost = context.Request.Host.ToString();
                        var requestPath = context.Request.Path;
                        var exception = ex?.Error?.Message;
                        LogHelper.Error($"\r\nMethod:{requestMethod} \r\nHost:{requestHost} \r\nPath:{requestPath} \r\nException:{exception} \r\n");
                    }


                    await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
                });


            });

            ////跨域
            app.UseCors("LimitRequests");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                c.RoutePrefix = "";//路径配置，设置为空，表示直接访问该文件，
                //路径配置，设置为空，表示直接在根域名（localhost:8001）访问该文件,注意localhost:8001/swagger是访问不到的，
                //这个时候去launchSettings.json中把"launchUrl": "swagger/index.html"去掉， 然后直接访问localhost:8001/index.html即可
            });
            #endregion

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
