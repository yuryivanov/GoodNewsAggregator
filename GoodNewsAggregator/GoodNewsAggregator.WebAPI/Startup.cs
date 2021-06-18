using AutoMapper;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.CQRS.CommandHandlers;
using GoodNewsAggregator.DAL.CQRS.Queries;
using GoodNewsAggregator.DAL.CQRS.QueryHandlers;
using GoodNewsAggregator.DAL.Repositories.Implementation;
using GoodNewsAggregator.DAL.Repositories.Implementation.Repositories;
using GoodNewsAggregator.DAL.Repositories.Interfaces;
using GoodNewsAggregator.Services.Implementation;
using Hangfire;
using Hangfire.SqlServer;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NewsAggregators.Services.Implementation.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;


namespace GoodNewsAggregator.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<GoodNewsAggregatorContext>(options
                => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<IRSSRepository, RSSRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<INewsService, NewsCqsService>();
            services.AddScoped<IRSSService, RssCqsService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ICommentService, CommentService>();

            services.AddScoped<IWebPageParser, OnlinerParser>();
            services.AddScoped<IWebPageParser, TutByParser>();
            services.AddScoped<IWebPageParser, S13Parser>();
            services.AddScoped<IWebPageParser, FourPdaParser>();

            services.AddTransient<OnlinerParser>();
            services.AddTransient<TutByParser>();
            services.AddTransient<S13Parser>();
            services.AddTransient<FourPdaParser>();

            services.AddHangfire(conf => conf
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(30),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(30),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

            services.AddHangfireServer();

            services.AddAutoMapper(typeof(Startup));

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapping());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddMediatR(typeof(GetRssByIdQueryHandler).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(GetAllRssesQueryHandler).GetTypeInfo().Assembly);            
            services.AddMediatR(typeof(GetNewsByIdWithRssAddressQueryHandler).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(GetAllExistingNewsUrlsQueryHandler).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(GetAllNewsQueryHandler).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(GetNewsCommentsByNewsIdQueryHandler).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(AddCommentByNewsIdCommandHandler).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(EditNewsCommandHandler).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(AddRangeNewsCommandHandler).GetTypeInfo().Assembly);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GoodNewsAggregator.WebAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GoodNewsAggregator.WebAPI v1"));

                app.UseHangfireDashboard();

                var newsCqsService = serviceProvider.GetService(typeof(INewsService)) as INewsService;
                RecurringJob.AddOrUpdate(() => newsCqsService.RateNews(), "0,15,30,45 * * * *");
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
        }
    }
}
