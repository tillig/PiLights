﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PiLights.Scenes;
using PiLights.Services;

namespace PiLights
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        [SuppressMessage("CA1812", "CA1812", Justification = "Startup requires instance methods.")]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        [SuppressMessage("CA1812", "CA1812", Justification = "Startup requires instance methods.")]
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<SceneManager>();
            builder
                .RegisterAssemblyTypes(typeof(Startup).Assembly)
                .Where(t => typeof(Scene).IsAssignableFrom(t));
        }

        [SuppressMessage("CA1812", "CA1812", Justification = "Startup requires instance methods.")]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddTransient<IStartupFilter, StartupFilter>();
            services.AddDistributedMemoryCache();
            services.AddSession();
        }
    }
}
