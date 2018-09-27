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
            if (additionalConfigs != null && additionalConfigs.ContainsKey("StartupProjectName"))
                startupProjectName = additionalConfigs["StartupProjectName"];

            var startupProjectPath = Path.Combine(config.MigrationLocation, startupProjectName);
            if (!Path.IsPathRooted(startupProjectPath))
                startupProjectPath = Path.Combine(config.WorkingLocation, startupProjectPath);

            var dataProjectName = $"{projectName}.Data";
            if (additionalConfigs != null && additionalConfigs.ContainsKey("DataProjectName"))
                dataProjectName = additionalConfigs["DataProjectName"];

            var dataProjectPath = Path.Combine(config.MigrationLocation, dataProjectName);
            if (!Path.IsPathRooted(dataProjectPath))
                dataProjectPath = Path.Combine(config.WorkingLocation, dataProjectPath);

            var buildConfiguration = "Release";
            if (additionalConfigs != null && additionalConfigs.ContainsKey("Configuration"))
                buildConfiguration = additionalConfigs["Configuration"];

            if (_databaseCommand == null)
                _databaseCommand = new DatabaseCommand(logger);

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
