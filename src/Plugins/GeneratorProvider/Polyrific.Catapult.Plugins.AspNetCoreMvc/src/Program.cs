// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Plugins.Core;

namespace Polyrific.Catapult.Plugins.AspNetCoreMvc
{
    internal class Program : GenerateTaskProvider
    {
        public override string Name => "Polyrific.Catapult.Plugins.AspNetCoreMvc";

        public Program(string[] args) : base(args)
        {
        }
        
        public override Task<(string outputLocation, Dictionary<string, string> outputValues, string errorMessage)> Generate()
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
