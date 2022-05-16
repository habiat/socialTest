using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Social.Data;
using Social.Service.Authentication;
using Social.Service.Security;
using Social.Service.User;
using Social.Web.Models;

namespace Social.Web
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
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
            services.AddHttpContextAccessor();

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            services.AddDbContext<SocialDbContext>(c =>
                c.UseSqlServer(Configuration.GetConnectionString("sqlConnection")));


           
            services.AddScoped<IAuthenticationService, CookieAuthenticationService>();
          
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRegistrationService, UserRegistrationService>();
            services.AddScoped<IEncryptionService, EncryptionService>();
         
            services.AddScoped<CustomCookieAuthenticationEvents>();
         


            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.Cookie.Name = "MySessionCookiesssd";
                    options.LoginPath = "/login";
                    options.LogoutPath = "/logout";
                    options.SlidingExpiration = true;
                    options.AccessDeniedPath = "/Forbidden";
                    options.ExpireTimeSpan = TimeSpan.FromHours(10);
                    options.EventsType = typeof(CustomCookieAuthenticationEvents);
                });
           
            services.AddRazorPages();
            services.AddResponseCompression();
            services.Configure<GzipCompressionProviderOptions>
            (opt =>
            {
                opt.Level = CompressionLevel.Fastest;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var cookiePolicyOptions = new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,//important for cookie anf multi-language--Unspecified or 

                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.None,
            };
            app.UseCookiePolicy(cookiePolicyOptions);
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
