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
                // TODO: Running this causes the app to hang on the socket.Disconnect(true); part after sending the previous scene.
                // builder.UseMiddleware<LastKnownSceneMiddleware>();
                next(builder);
            };
        }
    }
}
