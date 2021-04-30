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
using System.IO;
using GoodNewsAggregator.DAL.Core;
using Microsoft.AspNetCore.Mvc.Routing;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.Services.Implementation;
using GoodNewsAggregator.DAL.Repositories.Interfaces;
using GoodNewsAggregator.DAL.Repositories.Implementation.Repositories;
using GoodNewsAggregator.DAL.Repositories.Implementation;
using GoodNewsAggregator.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace GoodNewsAggregator
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<GoodNewsAggregatorContext>(options
                => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<IRSSRepository, RSSRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<IRSSService, RSSService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IWebPageParser, OnlinerParser>();
            services.AddScoped<IWebPageParser, TutByParser>();
            services.AddScoped<IWebPageParser, S13Parser>();

            services.AddTransient<OnlinerParser>();
            services.AddTransient<TutByParser>();
            services.AddTransient<S13Parser>();

            services.AddScoped<ChromeFilterAttribute>();
            services.AddScoped<CustomExceptionFilterAttribute>();

            //services.AddAutoMapper(typeof(Startup));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(opt =>
               {
                   opt.LoginPath = new PathString("/Account/Login");
                   opt.AccessDeniedPath = new PathString("/Account/Login");
               });

            services.AddSession(options =>
            {
                //options.Cookie.IsEssential = true;
                //options.IdleTimeout = TimeSpan.FromSeconds(3600);
            });

            services.AddControllersWithViews().AddMvcOptions(opt =>
            {
                opt.Filters.Add(new ChromeFilterAttribute());
                opt.Filters.Add(typeof(CustomExceptionFilterAttribute));
                //opt.Filters.Add(new CustomExceptionFilterAttribute());
            });            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseAuthentication();
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
