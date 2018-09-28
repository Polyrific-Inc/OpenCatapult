// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;
using EntityFrameworkCore.Helpers;
using Microsoft.Extensions.Logging;

namespace EntityFrameworkCore
{
    public class DatabaseCommand : IDatabaseCommand
    {
        private readonly ILogger _logger;

        public DatabaseCommand(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<string> Update(string dataProject, string startupProject, string configuration = "Debug")
        {
            var args = $"ef database update --project \"{dataProject}\" --startup-project \"{startupProject}\" --configuration {configuration}";
            return await RunDotnet(args);
        }

        private async Task<string> RunDotnet(string args)
        {
            var result = await CommandHelper.Execute("dotnet", args, _logger);

            return result.error;
        }
    }
}
