using System.ServiceProcess;
using Microsoft.AspNetCore.Hosting;

namespace Polyrific.Catapult.Api
{
    public static class WebHostExtensions
    {
        public static void RunAsCustomService(this IWebHost host)
        {
            var webHostService = new CustomWebHostService(host);
            ServiceBase.Run(webHostService);
        }
    }
}
