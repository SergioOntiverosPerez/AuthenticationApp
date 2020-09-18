using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthenticationApp.AuthorizationRequirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuthenticationApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", config =>
            {
                config.Cookie.Name = "IdentityCookie";
                config.LoginPath = "/Home/Authenticate";
            });

            services.AddAuthorization(config =>
           {
               //var defaultAuthBuilder = new AuthorizationPolicyBuilder();
               //var defaultAuthPolicy = defaultAuthBuilder
               // .RequireAuthenticatedUser().RequireClaim(ClaimTypes.DateOfBirth)
               // .Build();

               //config.DefaultPolicy = defaultAuthPolicy;


               //config.AddPolicy("Claim.DoB", policyBuilder =>
               //{
               //    policyBuilder.RequireClaim(ClaimTypes.DateOfBirth);
               //});

               //config.AddPolicy("Claim.DoB", policyBuilder =>
               //{
               //    policyBuilder.AddRequirements(new CustomRequireClaim(ClaimTypes.DateOfBirth));
               //});

               config.AddPolicy("Claim.Dob", policyBuilder =>
               {
                   policyBuilder.RequireCustomClaim(ClaimTypes.DateOfBirth);
               });

               config.AddPolicy("Admin", policyBuilder => policyBuilder.RequireCustomClaim(ClaimTypes.Role));

               config.AddPolicy("Professor", policyBuilder => policyBuilder.RequireCustomClaim(ClaimTypes.Role));

               config.AddPolicy("Student", policyBuilder => policyBuilder.RequireCustomClaim(ClaimTypes.Role));

               
           });


            services.AddScoped<IAuthorizationHandler, CustomRequireClaimHandler>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // Who are you?
            app.UseAuthentication();


            // Are you allowed?
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
