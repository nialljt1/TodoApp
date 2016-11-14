﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QuickstartIdentityServer.Data.Context;
using System.Linq;
using System.Reflection;

namespace QuickstartIdentityServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var connectionString = @"server=DESKTOP-82VF481\SQLEXPRESS;database=IdentityServer4;trusted_connection=yes";
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            // configure identity server with in-memory users, but EF stores for clients and scopes
            services.AddIdentityServer()
                .AddTemporarySigningCredential()
                .AddAspNetIdentity<IdentityUser>()
                .AddConfigurationStore(builder =>
                    builder.UseSqlServer(connectionString, options =>
                        options.MigrationsAssembly(migrationsAssembly)))
                .AddOperationalStore(builder =>
                    builder.UseSqlServer(connectionString, options =>
                        options.MigrationsAssembly(migrationsAssembly)));


        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // this will do the initial DB population
            InitializeDatabase(app);
            loggerFactory.AddConsole(LogLevel.Debug);
            app.UseDeveloperExceptionPage();
            app.UseIdentity();
            app.UseIdentityServer();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme,

                AutomaticAuthenticate = false,
                AutomaticChallenge = false
            });

            app.UseGoogleAuthentication(new GoogleOptions
            {
                AuthenticationScheme = "Google",
                DisplayName = "Google",
                SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme,

                ClientId = "434483408261-55tc8n0cs4ff1fe21ea8df2o443v2iuc.apps.googleusercontent.com",
                ClientSecret = "3gcoTrEDPPJ0ukn_aYYT6PWo"
            });

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.Scopes.Any())
                {
                    foreach (var scope in Config.GetScopes())
                    {
                        context.Scopes.Add(scope.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}