using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace PiLights
{
    public class StartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                // TODO: This dumb crap runs on every request
                // builder.UseMiddleware<LastKnownSceneMiddleware>();
                next(builder);
            };
        }
    }
}
