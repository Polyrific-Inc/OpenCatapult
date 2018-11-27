using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Plugins.Core;

namespace Polyrific.Catapult.Plugins.AzureAppService
{
    internal class Program : HostingProvider
    {
        public Program(string[] args) : base(args)
        {
        }

        public override string Name => "Polyrific.Catapult.Plugins.AzureAppService";
        
        public override Task<(string hostLocation, Dictionary<string, string> outputValues, string errorMessage)> Deploy()
        {
            throw new NotImplementedException();
        }

        private static async Task Main(string[] args)
        {
            var app = new Program(args);

            var result = await app.Execute();
            app.ReturnOutput(result);
        }
    }
}
