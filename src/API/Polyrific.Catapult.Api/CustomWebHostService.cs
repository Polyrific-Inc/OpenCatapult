// Copyright (c) Polyrific, Inc 2019. All rights reserved.

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Polyrific.Catapult.Api
{
    public class CustomWebHostService : WebHostService
    {
        private ILogger _logger;

        public CustomWebHostService(IWebHost host) : base(host)
        {
            _logger = host.Services.GetRequiredService<ILogger<CustomWebHostService>>();
        }

        protected override void OnStarting(string[] args)
        {
            _logger.LogInformation("Starting OpenCatapult API service.");

            base.OnStarting(args);
        }

        protected override void OnStarted()
        {
            _logger.LogInformation("OpenCatapult API service has started.");

            base.OnStarted();
        }

        protected override void OnStopping()
        {
            _logger.LogInformation("Stopping OpenCatapult API service.");

            base.OnStopping();
        }

        protected override void OnStopped()
        {
            _logger.LogInformation("OpenCatapult API service has stopped.");

            base.OnStopping();
        }
    }
}
