using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using GoodNewsAggregator.MyLogger;
using System.IO;
using GoodNewsAggregator.DAL.Core;
using Microsoft.AspNetCore.Mvc.Routing;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.Services.Implementation;
using GoodNewsAggregator.DAL.Repositories;
using GoodNewsAggregator.DAL.Repositories.Interfaces;

namespace GoodNewsAggregator
{
    public class Startup
    {
        private readonly INewsService _newsService;
        private readonly INewsRepository _newsRepository;
        private readonly IRSSRepository _rSSRepository;

        public Startup(IConfiguration configuration, INewsService newsService, INewsRepository newsRepository, IRSSRepository rSSRepository)
        {
            _newsService = newsService;
            _newsRepository = newsRepository;
            _rSSRepository = rSSRepository;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {            
            services.AddDbContext<GoodNewsAggregatorContext>(options 
                => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<INewsService, NewsService>();
            services.AddTransient<INewsRepository, NewsRepository>();
            services.AddTransient<IRSSRepository, RSSRepository>();

            services.AddControllersWithViews();
            services.AddSession(options => {
                //options.Cookie.IsEssential = true;
                //options.IdleTimeout = TimeSpan.FromSeconds(3600);
            });            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory factory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //Error Loggs:

            //var loggerFactory = LoggerFactory.Create(builder =>
            //{
            //    builder.AddDebug();
            //    builder.AddFilter("Default", LogLevel.Error).SetMinimumLevel(LogLevel.Error);
            //});
            //ILogger logger = loggerFactory.CreateLogger<Startup>();

            //app.Use(async (context, next) =>
            //{
            //    logger.LogError($"Error path: {context.Request.Path}");
            //    logger.LogCritical($"Critical Error path: {context.Request.Path}");
            //    await next.Invoke();
            //});

            factory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
            var logger = factory.CreateLogger("FileLogger");

            app.Use(async (context, next) =>
            {
                logger.LogInformation($"Info Processing request: {context.Request.Path}");
                logger.LogError($"Eror Processing request: {context.Request.Path}");
                logger.LogTrace($"Trace Processing request: {context.Request.Path}");

                await next.Invoke();
            });

            //Session & Cookies:
            app.UseSession();
            app.Use(async (context, next) =>
            {
                if (context.Session.Keys.Contains("sessionName")) { }
                else
                {
                    context.Session.SetString("sessionName", "GoodNewsAggregatorSessionName");
                }
                await next.Invoke();
            });

            app.Use(async (context, next) =>
            {
                if (!(context.Request.Cookies.ContainsKey("cookiesName")))
                {                
                    context.Response.Cookies.Append("cookiesName", "GoodNewsAggregatorCookiesName");
                }
                await next.Invoke();
            });           

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=News}/{action=AllNews}/{id?}");
            });

            //Дефолтная страница, если endpoint указан не верно:
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapFallbackToController("AllNews", "News");
            });
        }
    }
}
