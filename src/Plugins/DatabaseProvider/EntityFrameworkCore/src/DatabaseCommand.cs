// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;
using EntityFrameworkCore.Helpers;
using Microsoft.Extensions.Logging;

namespace EntityFrameworkCore
{
    public class DatabaseCommand : IDatabaseCommand
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;

        public DatabaseCommand(ILogger logger, string connectionString)
        {
            _logger = logger;
            _connectionString = connectionString;
        }

        public async Task<string> Update(string dataProject, string startupProject, string configuration = "Debug")
        {
            var args = $"ef database update --project \"{dataProject}\" --startup-project \"{startupProject}\" --configuration {configuration}";
            var result = await RunDotnet(args);

            if (!string.IsNullOrEmpty(result.error))
                return result.error;
            else if (!result.output.EndsWith("Done.\r\n"))
                return "Failed updating database";
            else
                return "";
        }

        private async Task<(string output, string error)> RunDotnet(string args)
        {
            var result = await CommandHelper.Execute("dotnet", args, new System.Collections.Generic.Dictionary<string, string>
            {
                { "ConnectionStrings__DefaultConnection", _connectionString }
            }, _logger);

            return result;
        }
    }
}
