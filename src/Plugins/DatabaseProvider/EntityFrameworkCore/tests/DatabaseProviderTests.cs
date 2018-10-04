// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Polyrific.Catapult.Plugins.Abstraction.Configs;
using Xunit;

namespace DotNetCore.Tests
{
    public class DatabaseProviderTests
    {
        private readonly Mock<ILogger> _logger;
        private readonly Mock<IDatabaseCommand> _databaseCommand;

        public DatabaseProviderTests()
        {
            _logger = new Mock<ILogger>();
            _databaseCommand = new Mock<IDatabaseCommand>();
        }

        [Fact]
        public async void DeployDatabase_Success()
        {
            var workingLocation = Path.Combine(AppContext.BaseDirectory, "working", "20180817.1");
            
            var provider = new DatabaseProvider(_databaseCommand.Object);

            var taskConfig = new DeployDbTaskConfig
            {
                MigrationLocation = "src",
                WorkingLocation = workingLocation
            };
            
            var additionalConfig = new Dictionary<string, string>
            {
                { "ConnectionString", "Server=localhost;Database=generated.db;User ID=sa;Password=samprod;" }
            };

            var result = await provider.DeployDatabase("MyProject", taskConfig, additionalConfig, _logger.Object);

            Assert.Equal("", result.errorMessage);
            Assert.Equal("MyProject.Data", result.databaseLocation);
        }
    }
}
