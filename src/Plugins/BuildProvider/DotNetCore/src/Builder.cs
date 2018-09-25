// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.IO.Compression;
using System.Threading.Tasks;
using DotNetCore.Helpers;

namespace DotNetCore
{
    public class Builder : IBuilder
    {
        public async Task<string> Build(string csprojLocation, string buildOutputLocation)
        {
            var args = $"publish {csprojLocation} --output {buildOutputLocation}";
            return await RunDotnet(args);
        }

        public async Task<string> CreateArtifact(string buildOutputLocation, string artifactLocation)
        {
            await Task.Run(() => ZipFile.CreateFromDirectory(buildOutputLocation, artifactLocation));

            return "";
        }

        public async Task<string> Restore(string csprojLocation)
        {
            var args = $"restore {csprojLocation}";
            return await RunDotnet(args);
        }

        public async Task<string> Test(string csprojLocation)
        {
            var args = $"test {csprojLocation}";
            return await RunDotnet(args);
        }

        private async Task<string> RunDotnet(string args)
        {
            return await CommandHelper.Execute("dotnet", args);
        }
    }
}
