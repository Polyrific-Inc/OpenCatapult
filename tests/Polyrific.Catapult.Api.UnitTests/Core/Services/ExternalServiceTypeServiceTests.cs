// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Moq;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.Core.Specifications;
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Core.Services
{
    public class ExternalServiceTypeServiceTests
    {
        private readonly List<ExternalServiceType> _data;
        private readonly Mock<IExternalServiceTypeRepository> _externalServiceTypeRepository;

        public ExternalServiceTypeServiceTests()
        {
            _data = new List<ExternalServiceType>
            {
                new ExternalServiceType
                {
                    Id = 1,
                    Name = "GitHub"
                }
            };

            _externalServiceTypeRepository = new Mock<IExternalServiceTypeRepository>();
            _externalServiceTypeRepository.Setup(r =>
                    r.GetBySpec(It.IsAny<ExternalServiceTypeFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ExternalServiceTypeFilterSpecification spec, CancellationToken cancellationToken) =>
                    _data.Where(spec.Criteria.Compile()));
            _externalServiceTypeRepository.Setup(r =>
                    r.GetSingleBySpec(It.IsAny<ExternalServiceTypeFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ExternalServiceTypeFilterSpecification spec, CancellationToken cancellationToken) =>
                    _data.FirstOrDefault(spec.Criteria.Compile()));
        }

        [Fact]
        public async void GetExternalServiceTypes_ReturnItems()
        {
            var externalServiceTypeService = new ExternalServiceTypeService(_externalServiceTypeRepository.Object);
            var externalServiceTypes = await externalServiceTypeService.GetExternalServiceTypes();

            Assert.NotEmpty(externalServiceTypes);
        }

        [Fact]
        public async void GetExternalServiceType_ReturnItem()
        {
            var externalServiceTypeService = new ExternalServiceTypeService(_externalServiceTypeRepository.Object);
            var entity = await externalServiceTypeService.GetExternalServiceType(1);

            Assert.NotNull(entity);
            Assert.Equal(1, entity.Id);
        }

        [Fact]
        public async void GetExternalServiceType_ReturnNull()
        {
            var externalServiceTypeService = new ExternalServiceTypeService(_externalServiceTypeRepository.Object);
            var externalServiceType = await externalServiceTypeService.GetExternalServiceType(2);

            Assert.Null(externalServiceType);
        }

        [Fact]
        public async void GetExternalServiceTypeByName_ReturnItem()
        {
            var externalServiceTypeService = new ExternalServiceTypeService(_externalServiceTypeRepository.Object);
            var entity = await externalServiceTypeService.GetExternalServiceTypeByName("GitHub");

            Assert.NotNull(entity);
            Assert.Equal(1, entity.Id);
        }

        [Fact]
        public async void GetExternalServiceTypeByName_ReturnNull()
        {
            var externalServiceTypeService = new ExternalServiceTypeService(_externalServiceTypeRepository.Object);
            var externalServiceType = await externalServiceTypeService.GetExternalServiceTypeByName("VSTS");

            Assert.Null(externalServiceType);
        }
    }
}
