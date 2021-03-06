﻿using System;
using System.IO.Compression;
using CoreCrm.Core;
using CoreCrm.Core.Email;
using CoreCrm.Models.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreCrm
{
   public class Startup
   {
      private readonly IConfiguration _configuration;

      public Startup(IConfiguration configuration)
      {
         _configuration = configuration;
      }

      public void ConfigureServices(
         IServiceCollection services)
      {
         services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(_configuration["DB_CONN"]));

         services.AddIdentity<AppIdentityUser, AppIdentityRole>()
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();

         services.Configure<IdentityOptions>(options =>
         {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;

            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;

            options.User.RequireUniqueEmail = true;

            options.SignIn.RequireConfirmedEmail = true;
            options.SignIn.RequireConfirmedPhoneNumber = false;
         });

         services.ConfigureApplicationCookie(options =>
         {
            options.LoginPath = "/Security/Login";
            options.LogoutPath = "/Security/Logout";
            options.AccessDeniedPath = "/Security/AccessDenied";
            options.SlidingExpiration = true;
            options.Cookie = new CookieBuilder
            {
               //Domain = "",
               HttpOnly = true,
               Name = ".Security.Cookie",
               Path = "/",
               SameSite = SameSiteMode.Lax,
               SecurePolicy = CookieSecurePolicy.SameAsRequest
            };
         });

         services.AddTransient<IEmailSender, EmailSender>();

         services.AddMvc();

         services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
         services.AddResponseCompression(options => options.EnableForHttps = true);
      }

      public void Configure(
         IApplicationBuilder app,
         IHostingEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseBrowserLink();
            app.UseDeveloperExceptionPage();
         }
         else
         {
            app.UseExceptionHandler("/Home/Error");
            app.UseStatusCodePagesWithRedirects("/Home/Error/{0}");
            app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");
         }

         //Registered before static files to always set header
         app.UseXContentTypeOptions();
         app.UseReferrerPolicy(opts => opts.NoReferrer());
         app.UseCsp(opt => opt
            .DefaultSources(s => s.Self())
            .FontSources(s => s.Self().CustomSources("fonts.gstatic.com"))
            .StyleSources(s =>
               s.Self().CustomSources("fonts.googleapis.com")
                  .UnsafeInline()) //TODO: Remove UnsafeInline when Nwebsec Nonce fixed.
            .ScriptSources(s => s.Self().CustomSources("cdnjs.cloudflare.com", "code.jquery.com"))
            .ImageSources(s => s.Self().CustomSources("data:"))
         );

         app.UseResponseCompression();
         app.UseAuthentication();
         app.UseStaticFiles();

         //Registered after static files, to set headers for dynamic content.
         app.UseXfo(xfo => xfo.Deny());
         app.UseRedirectValidation(); //Register this earlier if there's middleware that might redirect.
         app.UseXDownloadOptions();
         app.UseXRobotsTag(options => options.NoIndex().NoFollow());
         app.UseXXssProtection(options => options.EnabledWithBlockMode());

         app.UseMvcWithDefaultRoute();
      }
   }
}