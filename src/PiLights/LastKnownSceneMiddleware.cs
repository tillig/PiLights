using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PiLights
{
    public class LastKnownSceneMiddleware
    {
        private readonly RequestDelegate next;

        public LastKnownSceneMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            Console.WriteLine("LastKnownSceneMiddlware.Invoke");

            var lastKnownScene = ConfigurationManager.GetLastKnownScene();
            if (lastKnownScene != null)
            {
                ConfigurationManager.SendDataToSocket(lastKnownScene);
            }

            await this.next(httpContext).ConfigureAwait(false);
        }
    }
}
