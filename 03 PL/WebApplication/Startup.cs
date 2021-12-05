using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories;
using Belcukerkka.Services;
using Belcukerkka.Services.Operations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Infrastructure;

namespace WebApplication
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
            services.AddDbContextPool<CandyShopDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("CandyShopDbLaptop")));

            services.RegisterCultureInfo();

            services.AddSession();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Admin/Login";
                    options.AccessDeniedPath = "/Admin/AccessDenied";
                    options.Cookie.Name = "CandyShopCookie";
                    options.ExpireTimeSpan = new TimeSpan(2, 0, 0);
                });

            services.AddRazorPages(
                options =>
                {
                    options.Conventions.AuthorizeFolder("/Admin");
                    options.Conventions.AllowAnonymousToPage("/Admin/Login");
                })
                .AddMvcOptions(options =>
                {
                    options.ModelBinderProviders.Insert(0, new DoubleModelBinderProvider());
                });

            services.AddHttpContextAccessor();

            services.AddAntiforgery(options => options.HeaderName = "XSRF-TOKEN");

            services.RegisterRepositoryService();

            services.AddTransient<IManyToManyService<CandyInComposition>, CandyInCompositionService>();
            services.AddTransient<IPartialToStringRenderer, PartialToStringRenderer>();

            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseSession();

            app.UseStaticFiles();

            app.UseRequestLocalization();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
