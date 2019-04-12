// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Moq;
using Newtonsoft.Json;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Security;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.Core.Specifications;
using Polyrific.Catapult.Shared.Dto.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Core.Services
{
    public class JobDefinitionServiceTests
    {
        private readonly List<JobDefinition> _data;
        private readonly List<JobTaskDefinition> _dataTask;
        private readonly Mock<IJobDefinitionRepository> _jobDefinitionRepository;
        private readonly Mock<IJobTaskDefinitionRepository> _jobTaskDefinitionRepository;
        private readonly Mock<IProjectRepository> _projectRepository;
        private readonly Mock<ITaskProviderRepository> _providerRepository;
        private readonly Mock<IExternalServiceRepository> _externalServiceRepository;
        private readonly Mock<ITaskProviderAdditionalConfigRepository> _providerAdditionalConfigRepository;
        private readonly Mock<ISecretVault> _secretVault;

        public JobDefinitionServiceTests()
        {
            _data = new List<JobDefinition>
            {
                new JobDefinition
                {
                    Id = 1,
                    ProjectId = 1,
                    Name = "Default"
                }
            };

            _dataTask = new List<JobTaskDefinition>
            {
                new JobTaskDefinition
                {
                    Id = 1,
                    JobDefinitionId = 1,
                    Name = "Generate",
                    Type = JobTaskDefinitionType.Generate,
                    Provider = "AspNetCoreMvc",
                    ConfigString = "test"
                }
            };
            
            _jobDefinitionRepository = new Mock<IJobDefinitionRepository>();
            _jobDefinitionRepository.Setup(r =>
                    r.GetBySpec(It.IsAny<JobDefinitionFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobDefinitionFilterSpecification spec, CancellationToken cancellationToken) =>
                    _data.Where(spec.Criteria.Compile()));
            _jobDefinitionRepository.Setup(r =>
                    r.GetSingleBySpec(It.IsAny<JobDefinitionFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobDefinitionFilterSpecification spec, CancellationToken cancellationToken) =>
                    _data.FirstOrDefault(spec.Criteria.Compile()));
            _jobDefinitionRepository.Setup(r =>
                    r.CountBySpec(It.IsAny<JobDefinitionFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobDefinitionFilterSpecification spec, CancellationToken cancellationToken) =>
                    _data.Count(spec.Criteria.Compile()));
            _jobDefinitionRepository.Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask).Callback((int id, CancellationToken cancellationToken) =>
                {
                    var entity = _data.FirstOrDefault(d => d.Id == id);
                    if (entity != null)
                        _data.Remove(entity);
                });
            _jobDefinitionRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => _data.FirstOrDefault(d => d.Id == id));
            _jobDefinitionRepository.Setup(r => r.Create(It.IsAny<JobDefinition>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(2).Callback((JobDefinition entity, CancellationToken cancellationToken) =>
                {
                    entity.Id = 2;
                    _data.Add(entity);
                });
            _jobDefinitionRepository.Setup(r => r.Update(It.IsAny<JobDefinition>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask).Callback((JobDefinition entity, CancellationToken cancellationToken) =>
                {
                    var oldEntity = _data.FirstOrDefault(d => d.Id == entity.Id);
                    if (oldEntity != null)
                    {
                        _data.Remove(oldEntity);
                        _data.Add(entity);
                    }
                });

            _jobTaskDefinitionRepository = new Mock<IJobTaskDefinitionRepository>();
            _jobTaskDefinitionRepository.Setup(r =>
                    r.GetBySpec(It.IsAny<JobTaskDefinitionFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobTaskDefinitionFilterSpecification spec, CancellationToken cancellationToken) =>
                    _dataTask.Where(spec.Criteria.Compile()));
            _jobTaskDefinitionRepository.Setup(r =>
                    r.CountBySpec(It.IsAny<JobTaskDefinitionFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobTaskDefinitionFilterSpecification spec, CancellationToken cancellationToken) =>
                    _dataTask.Count(spec.Criteria.Compile()));
            _jobTaskDefinitionRepository.Setup(r =>
                    r.GetSingleBySpec(It.IsAny<JobTaskDefinitionFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((JobTaskDefinitionFilterSpecification spec, CancellationToken cancellationToken) =>
                    _dataTask.FirstOrDefault(spec.Criteria.Compile()));
            _jobTaskDefinitionRepository.Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask).Callback((int id, CancellationToken cancellationToken) =>
                {
                    var entity = _dataTask.FirstOrDefault(d => d.Id == id);
                    if (entity != null)
                        _dataTask.Remove(entity);
                });
            _jobTaskDefinitionRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => _dataTask.FirstOrDefault(d => d.Id == id));
            _jobTaskDefinitionRepository
                .Setup(r => r.Create(It.IsAny<JobTaskDefinition>(), It.IsAny<CancellationToken>())).ReturnsAsync(2)
                .Callback((JobTaskDefinition entity, CancellationToken cancellationToken) =>
                {
                    entity.Id = 2;
                    _dataTask.Add(entity);
                });

            _jobTaskDefinitionRepository
                .Setup(r => r.CreateRange(It.IsAny<List<JobTaskDefinition>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<int> {2, 3})
                .Callback((List<JobTaskDefinition> entities, CancellationToken cancellationToken) =>
                {
                    _dataTask.AddRange(entities);
                });
            _jobTaskDefinitionRepository
                .Setup(r => r.Update(It.IsAny<JobTaskDefinition>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask).Callback((JobTaskDefinition entity, CancellationToken cancellationToken) =>
                {
                    var oldEntity = _dataTask.FirstOrDefault(d => d.Id == entity.Id);
                    if (oldEntity != null)
                    {
                        _dataTask.Remove(oldEntity);
                        _dataTask.Add(entity);
                    }
                });

            _projectRepository = new Mock<IProjectRepository>();
            _projectRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => id == 1 ? new Project() { Id = id } : null);

            _providerRepository = new Mock<ITaskProviderRepository>();

            _externalServiceRepository = new Mock<IExternalServiceRepository>();

            _providerAdditionalConfigRepository = new Mock<ITaskProviderAdditionalConfigRepository>();

            _secretVault = new Mock<ISecretVault>();
        }

        [Fact]
        public async void AddJobDefinition_ValidItem()
        {
            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            int newId = await projectJobDefinitionService.AddJobDefinition(1, "Complete CI/CD", false);

            Assert.True(newId > 1);
            Assert.True(_data.Count > 1);
        }

        [Fact]
        public void AddJobDefinition_DuplicateItem()
        {
            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var exception = Record.ExceptionAsync(() => projectJobDefinitionService.AddJobDefinition(1, "Default", false));

            Assert.IsType<DuplicateJobDefinitionException>(exception?.Result);
        }

        [Fact]
        public void AddJobDefinition_InvalidProject()
        {
            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var exception = Record.ExceptionAsync(() => projectJobDefinitionService.AddJobDefinition(2, "Category", false));

            Assert.IsType<ProjectNotFoundException>(exception?.Result);
        }

        [Fact]
        public void AddJobDefinition_MultipleDeletionJob()
        {
            _data[0].IsDeletion = true;
            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var exception = Record.ExceptionAsync(() => projectJobDefinitionService.AddJobDefinition(1, "Category", true));

            Assert.IsType<MultipleDeletionJobException>(exception?.Result);
        }

        [Fact]
        public async void GetJobDefinitions_ReturnItems()
        {
            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var dataModels = await projectJobDefinitionService.GetJobDefinitions(1);

            Assert.NotEmpty(dataModels);
        }

        [Fact]
        public async void GetJobDefinitions_ReturnEmpty()
        {
            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var dataModels = await projectJobDefinitionService.GetJobDefinitions(2);

            Assert.Empty(dataModels);
        }

        [Fact]
        public async void GetJobDefinitionById_ReturnItem()
        {
            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var projectJobDefinition = await projectJobDefinitionService.GetJobDefinitionById(1);

            Assert.NotNull(projectJobDefinition);
            Assert.Equal(1, projectJobDefinition.Id);
        }

        [Fact]
        public async void GetJobDefinitionById_ReturnNull()
        {
            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var jobDefinition = await projectJobDefinitionService.GetJobDefinitionById(2);

            Assert.Null(jobDefinition);
        }

        [Fact]
        public async void GetJobDefinitionByName_ReturnItem()
        {
            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var projectJobDefinition = await projectJobDefinitionService.GetJobDefinitionByName(1, "Default");

            Assert.NotNull(projectJobDefinition);
            Assert.Equal(1, projectJobDefinition.Id);
        }

        [Fact]
        public async void GetJobDefinitionByName_ReturnNull()
        {
            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var jobDefinition = await projectJobDefinitionService.GetJobDefinitionByName(1, "Default2");

            Assert.Null(jobDefinition);
        }

        [Fact]
        public async void DeleteJobDefinition_ValidItem()
        {
            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            await projectJobDefinitionService.DeleteJobDefinition(1);

            Assert.Empty(_data);
            Assert.Empty(_dataTask);
        }

        [Fact]
        public async void DeleteJobDefinitions_ValidItem()
        {
            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            await projectJobDefinitionService.DeleteJobDefinitions(1, new int[] { 1 });

            Assert.Empty(_data);
            Assert.Empty(_dataTask);
        }

        [Fact]
        public async void RenameJobDefinition_ValidItem()
        {
            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            await projectJobDefinitionService.RenameJobDefinition(1, "newName");

            var dataModel = _data.First(d => d.Id == 1);

            Assert.Equal("newName", dataModel.Name);
        }

        [Fact]
        public void RenameJobDefinition_DuplicateItem()
        {
            _data.Add(new JobDefinition
            {
                Id = 2,
                ProjectId = 1,
                Name = "newName"
            });

            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var exception = Record.ExceptionAsync(() => projectJobDefinitionService.RenameJobDefinition(1, "newName"));

            Assert.IsType<DuplicateJobDefinitionException>(exception?.Result);
        }

        [Fact]
        public async void AddJobTaskDefinition_ValidItem()
        {
            _providerRepository.Setup(r => r.GetSingleBySpec(It.IsAny<ISpecification<TaskProvider>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TaskProvider { Id = 3, Name = "GitHubProvider", Type = TaskProviderType.RepositoryProvider, RequiredServicesString = "GitHub" });

            _externalServiceRepository.Setup(r => r.GetSingleBySpec(It.IsAny<ISpecification<ExternalService>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                new ExternalService
                {
                    Id = 3,
                    Name = "github-default",
                    ExternalServiceTypeId = 1,
                    ExternalServiceType = new ExternalServiceType
                    {
                        Id = 1,
                        Name = "GitHub"
                    }
                });

            _providerAdditionalConfigRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<TaskProviderAdditionalConfig>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TaskProviderAdditionalConfig>
                {
                    new TaskProviderAdditionalConfig
                    {
                        Id = 1,
                        Name = "testconfig",
                        IsRequired = true
                    }
                });

            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            int newId = await projectJobDefinitionService.AddJobTaskDefinition(new JobTaskDefinition
            {
                JobDefinitionId = 1,
                Type = JobTaskDefinitionType.Push,
                ConfigString = @"{""GitHubExternalService"":""github-default""}",
                AdditionalConfigString = @"{""testconfig"":""testvalue""}",
                Provider = "GitHubProvider"
            });

            Assert.True(newId > 1);
            Assert.True(_dataTask.Count > 1);
            Assert.Equal(1, _dataTask[1].Sequence);
        }

        [Fact]
        public void AddJobTaskDefinition_ConfigNull_ExternalServiceRequiredException()
        {
            _providerRepository.Setup(r => r.GetSingleBySpec(It.IsAny<ISpecification<TaskProvider>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TaskProvider { Id = 3, Name = "GitHubProvider", Type = TaskProviderType.RepositoryProvider, RequiredServicesString = "GitHub" });

            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var exception = Record.ExceptionAsync(() => projectJobDefinitionService.AddJobTaskDefinition(new JobTaskDefinition
            {
                JobDefinitionId = 1,
                Type = JobTaskDefinitionType.Push,
                ConfigString = null,
                Provider = "GitHubProvider"
            }));

            Assert.IsType<ExternalServiceRequiredException>(exception?.Result);
        }

        [Fact]
        public void AddJobTaskDefinition_ExternalServiceRequiredException()
        {
            _providerRepository.Setup(r => r.GetSingleBySpec(It.IsAny<ISpecification<TaskProvider>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TaskProvider { Id = 3, Name = "GitHubProvider", Type = TaskProviderType.RepositoryProvider, RequiredServicesString = "GitHub" });

            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var exception = Record.ExceptionAsync(() => projectJobDefinitionService.AddJobTaskDefinition(new JobTaskDefinition
            {
                JobDefinitionId = 1,
                Type = JobTaskDefinitionType.Push,
                ConfigString = "{}",
                Provider = "GitHubProvider"
            }));

            Assert.IsType<ExternalServiceRequiredException>(exception?.Result);
        }

        [Fact]
        public void AddJobTaskDefinition_ExternalServiceNotFoundException()
        {
            _providerRepository.Setup(r => r.GetSingleBySpec(It.IsAny<ISpecification<TaskProvider>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TaskProvider { Id = 3, Name = "GitHubProvider", Type = TaskProviderType.RepositoryProvider, RequiredServicesString = "GitHub" });

            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var exception = Record.ExceptionAsync(() => projectJobDefinitionService.AddJobTaskDefinition(new JobTaskDefinition
            {
                JobDefinitionId = 1,
                Type = JobTaskDefinitionType.Push,
                ConfigString = @"{""GitHubExternalService"":""github-default""}",
                Provider = "GitHubProvider"
            }));

            Assert.IsType<ExternalServiceNotFoundException>(exception?.Result);
        }

        [Fact]
        public void AddJobTaskDefinition_IncorrectExternalServiceTypeException()
        {
            _providerRepository.Setup(r => r.GetSingleBySpec(It.IsAny<ISpecification<TaskProvider>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TaskProvider { Id = 3, Name = "GitHubProvider", Type = TaskProviderType.RepositoryProvider, RequiredServicesString = "GitHub" });

            _externalServiceRepository.Setup(r => r.GetSingleBySpec(It.IsAny<ISpecification<ExternalService>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                new ExternalService { Id = 3, Name = "azure-default", ExternalServiceTypeId = 1,
                    ExternalServiceType = new ExternalServiceType
                    {
                        Id = 1,
                        Name = "Azure"
                    }
                });

            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var exception = Record.ExceptionAsync(() => projectJobDefinitionService.AddJobTaskDefinition(new JobTaskDefinition
            {
                JobDefinitionId = 1,
                Type = JobTaskDefinitionType.Push,
                ConfigString = @"{""GitHubExternalService"":""azure-default""}",
                Provider = "GitHubProvider"
            }));

            Assert.IsType<IncorrectExternalServiceTypeException>(exception?.Result);
        }

        [Fact]
        public async void AddJobTaskDefinition_GenericService_ValidItem()
        {
            _providerRepository.Setup(r => r.GetSingleBySpec(It.IsAny<ISpecification<TaskProvider>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TaskProvider { Id = 3, Name = "GitHubProvider", Type = TaskProviderType.RepositoryProvider, RequiredServicesString = "GitHub" });

            _externalServiceRepository.Setup(r => r.GetSingleBySpec(It.IsAny<ISpecification<ExternalService>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                new ExternalService
                {
                    Id = 3,
                    Name = "generic",
                    ExternalServiceTypeId = 1,
                    ExternalServiceType = new ExternalServiceType
                    {
                        Id = 1,
                        Name = ExternalServiceTypeName.Generic
                    }
                });

            _providerAdditionalConfigRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<TaskProviderAdditionalConfig>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TaskProviderAdditionalConfig>
                {
                    new TaskProviderAdditionalConfig
                    {
                        Id = 1,
                        Name = "testconfig",
                        IsRequired = true
                    }
                });

            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            int newId = await projectJobDefinitionService.AddJobTaskDefinition(new JobTaskDefinition
            {
                JobDefinitionId = 1,
                Type = JobTaskDefinitionType.Push,
                ConfigString = @"{""GitHubExternalService"":""github-default""}",
                AdditionalConfigString = @"{""testconfig"":""testvalue""}",
                Provider = "GitHubProvider"
            });

            Assert.True(newId > 1);
            Assert.True(_dataTask.Count > 1);
        }

        [Fact]
        public void AddJobTaskDefinition_InvalidJobDefinition()
        {
            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var exception = Record.ExceptionAsync(() => projectJobDefinitionService.AddJobTaskDefinition(new JobTaskDefinition
            {
                JobDefinitionId = 2,
                Type = JobTaskDefinitionType.Push,
                ConfigString = "test"
            }));

            Assert.IsType<JobDefinitionNotFoundException>(exception?.Result);
        }

        [Fact]
        public void AddJobTaskDefinition_AdditionalConfigNull_AdditionalConfigRequiredException()
        {
            _providerRepository.Setup(r => r.GetSingleBySpec(It.IsAny<ISpecification<TaskProvider>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TaskProvider { Id = 3, Name = "GitHubProvider", Type = TaskProviderType.RepositoryProvider });

            _providerAdditionalConfigRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<TaskProviderAdditionalConfig>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TaskProviderAdditionalConfig>
                {
                    new TaskProviderAdditionalConfig
                    {
                        Id = 1,
                        Name = "testconfig",
                        IsRequired = true
                    }
                });

            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var exception = Record.ExceptionAsync(() => projectJobDefinitionService.AddJobTaskDefinition(new JobTaskDefinition
            {
                JobDefinitionId = 1,
                Type = JobTaskDefinitionType.Push,
                AdditionalConfigString = null,
                Provider = "GitHubProvider"
            }));

            Assert.IsType<TaskProviderAdditionalConfigRequiredException>(exception?.Result);
        }

        [Fact]
        public void AddJobTaskDefinition_AdditionalConfigNotExist_AdditionalConfigRequiredException()
        {
            _providerRepository.Setup(r => r.GetSingleBySpec(It.IsAny<ISpecification<TaskProvider>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TaskProvider { Id = 3, Name = "GitHubProvider", Type = TaskProviderType.RepositoryProvider });

            _providerAdditionalConfigRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<TaskProviderAdditionalConfig>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TaskProviderAdditionalConfig>
                {
                    new TaskProviderAdditionalConfig
                    {
                        Id = 1,
                        Name = "testconfig",
                        IsRequired = true
                    }
                });

            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var exception = Record.ExceptionAsync(() => projectJobDefinitionService.AddJobTaskDefinition(new JobTaskDefinition
            {
                JobDefinitionId = 1,
                Type = JobTaskDefinitionType.Push,
                AdditionalConfigString = "{}",
                Provider = "GitHubProvider"
            }));

            Assert.IsType<TaskProviderAdditionalConfigRequiredException>(exception?.Result);
        }

        [Fact]
        public void AddJobTaskDefinition_DuplicateJobTaskDefinitionException()
        {
            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var exception = Record.ExceptionAsync(() => projectJobDefinitionService.AddJobTaskDefinition(new JobTaskDefinition
            {
                JobDefinitionId = 1,
                Name = "Generate",
                Type = JobTaskDefinitionType.Push,
                AdditionalConfigString = "{}",
                Provider = "GitHubProvider"
            }));

            Assert.IsType<DuplicateJobTaskDefinitionException>(exception?.Result);
        }

        [Fact]
        public void AddJobTaskDefinition_InvalidProviderTypeException()
        {
            _providerRepository.Setup(r => r.GetSingleBySpec(It.IsAny<ISpecification<TaskProvider>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TaskProvider { Id = 3, Name = "GitHubProvider", Type = "InvalidType" });

            _providerAdditionalConfigRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<TaskProviderAdditionalConfig>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TaskProviderAdditionalConfig>
                {
                    new TaskProviderAdditionalConfig
                    {
                        Id = 1,
                        Name = "testconfig",
                        IsRequired = true
                    }
                });

            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var exception = Record.ExceptionAsync(() => projectJobDefinitionService.AddJobTaskDefinition(new JobTaskDefinition
            {
                JobDefinitionId = 1,
                Type = JobTaskDefinitionType.Push,
                AdditionalConfigString = "{}",
                Provider = "GitHubProvider"
            }));

            Assert.IsType<InvalidTaskProviderTypeException>(exception?.Result);
        }

        [Fact]
        public void AddJobTaskDefinition_IncorrectTaskType_InvalidProviderTypeException()
        {
            _providerRepository.Setup(r => r.GetSingleBySpec(It.IsAny<ISpecification<TaskProvider>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TaskProvider { Id = 3, Name = "VstsBuildProvider", Type = TaskProviderType.BuildProvider });

            _providerAdditionalConfigRepository.Setup(r => r.GetBySpec(It.IsAny<ISpecification<TaskProviderAdditionalConfig>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TaskProviderAdditionalConfig>
                {
                    new TaskProviderAdditionalConfig
                    {
                        Id = 1,
                        Name = "testconfig",
                        IsRequired = true
                    }
                });

            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var exception = Record.ExceptionAsync(() => projectJobDefinitionService.AddJobTaskDefinition(new JobTaskDefinition
            {
                JobDefinitionId = 1,
                Type = JobTaskDefinitionType.Push,
                AdditionalConfigString = "{}",
                Provider = "VstsBuildProvider"
            }));

            Assert.IsType<InvalidTaskProviderTypeException>(exception?.Result);

            var invalidProviderTypeException = exception?.Result as InvalidTaskProviderTypeException;
            Assert.NotEmpty(invalidProviderTypeException.TaskTypes);
        }

        [Fact]
        public void AddJobTaskDefinition_JobTaskDefinitionTypeException()
        {
            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            _data[0].IsDeletion = true;
            var exception = Record.ExceptionAsync(() => projectJobDefinitionService.AddJobTaskDefinition(new JobTaskDefinition
            {
                JobDefinitionId = 1,
                Name = "Push",
                Type = JobTaskDefinitionType.Push
            }));

            Assert.IsType<JobTaskDefinitionTypeException>(exception?.Result);
        }

        [Fact]
        public async void AddJobTaskDefinitions_ValidItem()
        {
            _providerRepository.Setup(r => r.GetSingleBySpec(It.IsAny<ISpecification<TaskProvider>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TaskProvider { Id = 3, Name = "GitHubProvider", Type = TaskProviderType.RepositoryProvider, RequiredServicesString = "GitHub" });
            
            _externalServiceRepository.Setup(r => r.GetSingleBySpec(It.IsAny<ISpecification<ExternalService>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                new ExternalService
                {
                    Id = 3,
                    Name = "github-default",
                    ExternalServiceTypeId = 1,
                    ExternalServiceType = new ExternalServiceType
                    {
                        Id = 1,
                        Name = "GitHub"
                    }
                });

            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var newIds = await projectJobDefinitionService.AddJobTaskDefinitions(1, new List<JobTaskDefinition>
            {
                new JobTaskDefinition
                {
                    JobDefinitionId = 1,
                    Name = "Clone",
                    Type = JobTaskDefinitionType.Clone,
                    Provider = "GitHubProvider",
                    ConfigString = @"{""GitHubExternalService"":""github-default""}"
                },
                new JobTaskDefinition
                {
                    JobDefinitionId = 1,
                    Name = "Push",
                    Type = JobTaskDefinitionType.Push,
                    Provider = "GitHubProvider",
                    ConfigString = @"{""GitHubExternalService"":""github-default""}"
                }
            });

            Assert.True(newIds.Count > 1);
            foreach (var newId in newIds)
            {
                Assert.True(newId > 1);
            }

            Assert.True(_dataTask.Count > 1);
            Assert.Equal(1, _dataTask[1].Sequence);
            Assert.Equal(2, _dataTask[2].Sequence);
        }

        [Fact]
        public void AddJobTaskDefinitions_JobDefinitionNotFound()
        {
            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var exception = Record.ExceptionAsync(() => projectJobDefinitionService.AddJobTaskDefinitions(2, new List<JobTaskDefinition>
            {
                new JobTaskDefinition
                {
                    JobDefinitionId = 1,
                    Type = JobTaskDefinitionType.Push,
                    ConfigString = "test"
                },
                new JobTaskDefinition
                {
                    JobDefinitionId = 1,
                    Type = JobTaskDefinitionType.Build,
                    ConfigString = "test2"
                }
            }));

            Assert.IsType<JobDefinitionNotFoundException>(exception?.Result);
        }

        [Fact]
        public async void DeleteJobTaskDefinition_ValidItem()
        {
            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            await projectJobDefinitionService.DeleteJobTaskDefinition(1);

            Assert.Empty(_dataTask);
        }

        [Fact]
        public async void GetJobTaskDefinitions_ReturnItems()
        {
            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var jobTaskDefinitions = await projectJobDefinitionService.GetJobTaskDefinitions(1);

            Assert.NotEmpty(jobTaskDefinitions);
        }

        [Fact]
        public async void GetJobTaskDefinitions_ReturnEmpty()
        {
            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var jobTaskDefinitions = await projectJobDefinitionService.GetJobTaskDefinitions(2);

            Assert.Empty(jobTaskDefinitions);
        }

        [Fact]
        public async void GetJobTaskDefinitioById_ReturnItem()
        {
            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var jobTaskDefinition = await projectJobDefinitionService.GetJobTaskDefinitionById(1);

            Assert.NotNull(jobTaskDefinition);
        }

        [Fact]
        public async void GetJobTaskDefinitionById_ReturnEmpty()
        {
            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var jobTaskDefinition = await projectJobDefinitionService.GetJobTaskDefinitionById(2);

            Assert.Null(jobTaskDefinition);
        }

        [Fact]
        public async void GetJobTaskDefinitioByName_ReturnItem()
        {
            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var jobTaskDefinition = await projectJobDefinitionService.GetJobTaskDefinitionByName(1, "Generate");

            Assert.NotNull(jobTaskDefinition);
        }

        [Fact]
        public async void GetJobTaskDefinitionByName_ReturnEmpty()
        {
            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            var jobTaskDefinition = await projectJobDefinitionService.GetJobTaskDefinitionByName(1, "Push");

            Assert.Null(jobTaskDefinition);
        }

        [Fact]
        public async void UpdateJobTaskDefinition_ValidItem()
        {
            _providerRepository.Setup(r => r.GetSingleBySpec(It.IsAny<ISpecification<TaskProvider>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TaskProvider { Id = 3, Name = "AspMvcNetCore", Type = TaskProviderType.GeneratorProvider });

            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            await projectJobDefinitionService.UpdateJobTaskDefinition(new JobTaskDefinition
            {
                Id = 1,
                JobDefinitionId = 1,
                Type = JobTaskDefinitionType.Generate,
                Provider = "AspMvcNetCore",
                ConfigString = "testUpdated"
            });

            var jobTaskDefinition = _dataTask.First(d => d.Id == 1);

            Assert.Equal("testUpdated", jobTaskDefinition.ConfigString);
        }

        [Fact]
        public async void UpdateJobTaskConfig_ValidItem()
        {
            _providerRepository.Setup(r => r.GetSingleBySpec(It.IsAny<ISpecification<TaskProvider>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TaskProvider { Id = 3, Name = "AspMvcNetCore", Type = TaskProviderType.GeneratorProvider });

            var updatedConfig = new Dictionary<string, string>()
            {
                {"config1", "config 1 value"}
            };

            var updatedConfigString = JsonConvert.SerializeObject(updatedConfig);

            var projectJobDefinitionService = new JobDefinitionService(_jobDefinitionRepository.Object, _jobTaskDefinitionRepository.Object, _projectRepository.Object, _providerRepository.Object, _externalServiceRepository.Object, _providerAdditionalConfigRepository.Object, _secretVault.Object);
            await projectJobDefinitionService.UpdateJobTaskConfig(1, updatedConfig);

            var jobTaskDefinition = _dataTask.First(d => d.Id == 1);

            Assert.Equal(updatedConfigString, jobTaskDefinition.ConfigString);
        }
    }
}
