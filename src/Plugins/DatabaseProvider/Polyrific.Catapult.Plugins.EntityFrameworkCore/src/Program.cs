// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Polyrific.Catapult.Plugins.Core;

namespace Polyrific.Catapult.Plugins.EntityFrameworkCore
{
    public class Program : DatabaseProvider
    {
        private const string TaskProviderName = "Polyrific.Catapult.Plugins.EntityFrameworkCore";

        private IDatabaseCommand _databaseCommand;

        public Program(string[] args) : base(args)
        {
        }

        public Program(string[] args, IDatabaseCommand databaseCommand) : this(args)
        {
            _databaseCommand = databaseCommand;
        }

        public override string Name => TaskProviderName;
        
        public override async Task<(string databaseLocation, Dictionary<string, string> outputValues, string errorMessage)> DeployDatabase()
        {
            var startupProjectName = ProjectName;
            if (AdditionalConfigs != null && AdditionalConfigs.ContainsKey("StartupProjectName") && !string.IsNullOrEmpty(AdditionalConfigs["StartupProjectName"]))
                startupProjectName = AdditionalConfigs["StartupProjectName"];
            startupProjectName = $"{startupProjectName}.dll";

            var migrationLocation = Config.MigrationLocation ?? Path.Combine(Config.WorkingLocation, "publish");

            var startupProjectPath = Path.Combine(migrationLocation, startupProjectName);
            if (!Path.IsPathRooted(startupProjectPath))
                startupProjectPath = Path.Combine(Config.WorkingLocation, startupProjectPath);

            var dataProjectName = $"{ProjectName}.Data";
            if (AdditionalConfigs != null && AdditionalConfigs.ContainsKey("DatabaseProjectName") && !string.IsNullOrEmpty(AdditionalConfigs["DatabaseProjectName"]))
                dataProjectName = AdditionalConfigs["DatabaseProjectName"];
            dataProjectName = $"{dataProjectName}.dll";

            var dataProjectPath = Path.Combine(migrationLocation, dataProjectName);
            if (!Path.IsPathRooted(dataProjectPath))
                dataProjectPath = Path.Combine(Config.WorkingLocation, dataProjectPath);

            string connectionString = "";
            if (AdditionalConfigs != null && AdditionalConfigs.ContainsKey("ConnectionString") && !string.IsNullOrEmpty(AdditionalConfigs["ConnectionString"]))
                connectionString = AdditionalConfigs["ConnectionString"];

            if (_databaseCommand == null)
                _databaseCommand = new DatabaseCommand(Logger, connectionString);

            var error = await _databaseCommand.Update(dataProjectPath, startupProjectPath);
            if (!string.IsNullOrEmpty(error))
                return ("", null, error);
            
            return (GetDatabaseLocation(connectionString), null, "");
        }

        private static async Task Main(string[] args)
        {
            var app = new Program(args);
            
            var result = await app.Execute();
            app.ReturnOutput(result);
        }

        private string GetDatabaseLocation(string connectionString)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                var builder = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
                return builder.DataSource;
            }

            return "";
        }
    }
}
