// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Plugins.Abstraction;
using Polyrific.Catapult.Plugins.Abstraction.Configs;

namespace EntityFrameworkCore
{
    [Export(typeof(IDatabaseProvider))]
    public class DatabaseProvider : IDatabaseProvider
    {
        private IDatabaseCommand _databaseCommand;

        public DatabaseProvider()
        {

        }

        public DatabaseProvider(IDatabaseCommand database)
        {
            _databaseCommand = database;
        }

        public string Name => "EntityFrameworkCore";

        public string[] RequiredServices => new string[0];

        public Task<string> BeforeDeployDatabase(string projectName, DeployDbTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult("");
        }

        public async Task<(string databaseLocation, Dictionary<string, string> outputValues, string errorMessage)> DeployDatabase(string projectName, DeployDbTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            var startupProjectName = projectName;
            if (additionalConfigs != null && additionalConfigs.ContainsKey("StartupProjectName") && !string.IsNullOrEmpty(additionalConfigs["StartupProjectName"]))
                startupProjectName = additionalConfigs["StartupProjectName"];
            startupProjectName = $"{startupProjectName}.dll";

            var migrationLocation = config.MigrationLocation ?? Path.Combine(config.WorkingLocation, "publish");

            var startupProjectPath = Path.Combine(migrationLocation, startupProjectName);
            if (!Path.IsPathRooted(startupProjectPath))
                startupProjectPath = Path.Combine(config.WorkingLocation, startupProjectPath);

            var dataProjectName = $"{projectName}.Data";
            if (additionalConfigs != null && additionalConfigs.ContainsKey("DatabaseProjectName") && !string.IsNullOrEmpty(additionalConfigs["DatabaseProjectName"]))
                dataProjectName = additionalConfigs["DatabaseProjectName"];
            dataProjectName = $"{dataProjectName}.dll";

            var dataProjectPath = Path.Combine(migrationLocation, dataProjectName);
            if (!Path.IsPathRooted(dataProjectPath))
                dataProjectPath = Path.Combine(config.WorkingLocation, dataProjectPath);

            var buildConfiguration = "Release";
            if (additionalConfigs != null && additionalConfigs.ContainsKey("Configuration") && !string.IsNullOrEmpty(additionalConfigs["Configuration"]))
                buildConfiguration = additionalConfigs["Configuration"];

            string connectionString = "";
            if (additionalConfigs != null && additionalConfigs.ContainsKey("ConnectionString") && !string.IsNullOrEmpty(additionalConfigs["ConnectionString"]))
                connectionString = additionalConfigs["ConnectionString"];

            if (_databaseCommand == null)
                _databaseCommand = new DatabaseCommand(logger, connectionString);

            var error = await _databaseCommand.Update(dataProjectPath, startupProjectPath, buildConfiguration);
            if (!string.IsNullOrEmpty(error))
                return ("", null, error);
            
            return (dataProjectName, null, "");
        }

        public Task<string> AfterDeployDatabase(string projectName, DeployDbTaskConfig config, Dictionary<string, string> additionalConfigs, ILogger logger)
        {
            return Task.FromResult("");
        }
    }
}
