using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using Newtonsoft.Json;
using Polyrific.Catapult.TaskProviders.Core.Configs;
using Xunit;

namespace Polyrific.Catapult.Plugins.EntityFrameworkCore.UnitTests
{
    public class DatabaseProviderTests
    {
        private readonly Mock<IDatabaseCommand> _databaseCommand;

        public DatabaseProviderTests()
        {
            _databaseCommand = new Mock<IDatabaseCommand>();
        }

        [Fact]
        public async void DeployDatabase_Success()
        {
            var workingLocation = Path.Combine(AppContext.BaseDirectory, "working", "20180817.1");

            var taskConfig = new DeployDbTaskConfig
            {
                MigrationLocation = "src",
                WorkingLocation = workingLocation
            };

            var additionalConfig = new Dictionary<string, string>
            {
                { "ConnectionString", "Server=localhost;Database=generated.db;User ID=sa;Password=samprod;" }
            };

            var provider = new Program(new string[] { GetArgString("main", "TestProject", taskConfig, additionalConfig) }, _databaseCommand.Object);

            var result = await provider.DeployDatabase();

            Assert.Equal("", result.errorMessage);
            Assert.Equal("localhost", result.databaseLocation);
        }

        private string GetArgString(string process, string projectName, DeployDbTaskConfig taskConfig, Dictionary<string, string> additionalConfigs)
        {
            var dict = new Dictionary<string, object>
            {
                {"process", process},
                {"project", projectName},
                {"config", taskConfig},
                {"additional", additionalConfigs}
            };

            return JsonConvert.SerializeObject(dict);
        }
    }
}
