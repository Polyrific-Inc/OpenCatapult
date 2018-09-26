// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.IO;
using System.IO.Compression;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DotNetCore.Helpers;

namespace DotNetCore
{
    public class Builder : IBuilder
    {
        public async Task<string> Build(string csprojLocation, string buildOutputLocation, string configuration = "Debug")
        {
            var args = $"publish {csprojLocation} --output {buildOutputLocation} --configuration {configuration}";
            return await RunDotnet(args);
        }

        public Task<string> CreateArtifact(string buildOutputLocation, string destinationArtifact)
        {
            var errorMessage = "";

            var extension = Path.GetExtension(destinationArtifact);
            if (string.IsNullOrEmpty(extension) || !extension.Equals(".zip", StringComparison.InvariantCultureIgnoreCase))
                destinationArtifact = Path.ChangeExtension(destinationArtifact, "zip");

            var dir = Path.GetDirectoryName(destinationArtifact);
            if (dir != null && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            try
            {
                ZipFile.CreateFromDirectory(buildOutputLocation, destinationArtifact, CompressionLevel.Fastest, false);
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }
            
            return Task.FromResult(errorMessage);
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
            var result = await CommandHelper.Execute("dotnet", args);

            return result.error;
        }
    }
}
