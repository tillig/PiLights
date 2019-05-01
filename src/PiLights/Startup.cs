using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PiLights.Configuration;
using PiLights.Scenes;
using PiLights.Validation;

namespace PiLights
{
    public class Startup
    {
        public Startup(IHostingEnvironment environment, IConfiguration configuration)
        {
            this.HostingEnvironment = environment;
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment HostingEnvironment { get; }

        [SuppressMessage("CA1822", "CA1822", Justification = "Startup methods need to be all instance or all static.")]
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
            builder
                .RegisterAssemblyTypes(typeof(Startup).Assembly)
                .Where(t => typeof(Scene).IsAssignableFrom(t))
                .As<Scene>();
            builder.RegisterInstance(this.HostingEnvironment);
            builder.RegisterInstance(this.Configuration);
            builder
                .Register(ctx => ctx.Resolve<IConfiguration>().GetSection("ws281x").Get<LedSettings>())
                .As<LedSettings>()
                .SingleInstance();
            builder.RegisterType<BootstrapValidationAttributeAdapterProvider>().As<IValidationAttributeAdapterProvider>();
        }

        [SuppressMessage("CA1822", "CA1822", Justification = "Startup methods need to be all instance or all static.")]
        [SuppressMessage("CA1812", "CA1812", Justification = "Startup requires instance methods.")]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDistributedMemoryCache();
            services.AddSession();
        }
    }
}
