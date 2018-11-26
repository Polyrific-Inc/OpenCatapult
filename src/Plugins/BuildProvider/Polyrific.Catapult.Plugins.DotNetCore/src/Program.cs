// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Plugins.Core;

namespace Polyrific.Catapult.Plugins.DotNetCore
{
    internal class Program : BuildTaskProvider
    {
        public override string Name => "Polyrific.Catapult.Plugins.DotNetCore";

        public Program(string[] args) : base(args)
        {
        }
        
        public override Task<(string outputArtifact, Dictionary<string, string> outputValues, string errorMessage)> Build()
        {
            throw new NotImplementedException();
        }

        private static async Task Main(string[] args)
        {
            await new Program(args).Execute();
        }
        
        
    }
}
