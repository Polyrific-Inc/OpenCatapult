// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Moq;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.Core.Specifications;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Core.Services
{
    public class ProjectDataModelServiceTests
    {
        private readonly List<ProjectDataModel> _data;
        private readonly List<ProjectDataModelProperty> _dataProperty;
        private readonly Mock<IProjectDataModelRepository> _dataModelRepository;
        private readonly Mock<IProjectDataModelPropertyRepository> _propertyRepository;
        private readonly Mock<IProjectRepository> _projectRepository;

        public ProjectDataModelServiceTests()
        {
            _data = new List<ProjectDataModel>
            {
                new ProjectDataModel
                {
                    Id = 1,
                    ProjectId = 1,
                    Name = "Product"
                }
            };

            _dataProperty = new List<ProjectDataModelProperty>
            {
                new ProjectDataModelProperty
                {
                    Id = 1,
                    ProjectDataModelId = 1,
                    Name = "Name"
                }
            };

            _dataModelRepository = new Mock<IProjectDataModelRepository>();
            _dataModelRepository.Setup(r =>
                    r.GetBySpec(It.IsAny<ProjectDataModelFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProjectDataModelFilterSpecification spec, CancellationToken cancellationToken) =>
                    _data.Where(spec.Criteria.Compile()));
            _dataModelRepository.Setup(r =>
                    r.GetSingleBySpec(It.IsAny<ProjectDataModelFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProjectDataModelFilterSpecification spec, CancellationToken cancellationToken) =>
                    _data.FirstOrDefault(spec.Criteria.Compile()));
            _dataModelRepository.Setup(r =>
                    r.CountBySpec(It.IsAny<ProjectDataModelFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProjectDataModelFilterSpecification spec, CancellationToken cancellationToken) =>
                    _data.Count(spec.Criteria.Compile()));
            _dataModelRepository.Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask).Callback((int id, CancellationToken cancellationToken) =>
                {
                    var entity = _data.FirstOrDefault(d => d.Id == id);
                    if (entity != null)
                        _data.Remove(entity);
                });
            _dataModelRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => _data.FirstOrDefault(d => d.Id == id));
            _dataModelRepository.Setup(r => r.Create(It.IsAny<ProjectDataModel>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(2).Callback((ProjectDataModel entity, CancellationToken cancellationToken) =>
                {
                    entity.Id = 2;
                    _data.Add(entity);
                });
            _dataModelRepository.Setup(r => r.Update(It.IsAny<ProjectDataModel>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask).Callback((ProjectDataModel entity, CancellationToken cancellationToken) =>
                {
                    var oldEntity = _data.FirstOrDefault(d => d.Id == entity.Id);
                    if (oldEntity != null)
                    {
                        _data.Remove(oldEntity);
                        _data.Add(entity);
                    }
                });

            _propertyRepository = new Mock<IProjectDataModelPropertyRepository>();
            _propertyRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) =>
                    _dataProperty.FirstOrDefault(d => d.Id == id));
            _propertyRepository.Setup(r =>
                    r.GetBySpec(It.IsAny<ProjectDataModelPropertyFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProjectDataModelPropertyFilterSpecification spec, CancellationToken cancellationToken) =>
                    _dataProperty.Where(spec.Criteria.Compile()));
            _propertyRepository.Setup(r =>
                    r.GetSingleBySpec(It.IsAny<ProjectDataModelPropertyFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProjectDataModelPropertyFilterSpecification spec, CancellationToken cancellationToken) =>
                    _dataProperty.FirstOrDefault(spec.Criteria.Compile()));
            _propertyRepository.Setup(r => r.CountBySpec(It.IsAny<ProjectDataModelPropertyFilterSpecification>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProjectDataModelPropertyFilterSpecification spec, CancellationToken cancellationToken) =>
                    _dataProperty.Count(spec.Criteria.Compile()));
            _propertyRepository
                .Setup(r => r.Create(It.IsAny<ProjectDataModelProperty>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(2).Callback((ProjectDataModelProperty entity, CancellationToken cancellationToken) =>
                {
                    entity.Id = 2;
                    _dataProperty.Add(entity);
                });
            _propertyRepository
                .Setup(r => r.Update(It.IsAny<ProjectDataModelProperty>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask).Callback(
                    (ProjectDataModelProperty entity, CancellationToken cancellationToken) =>
                    {
                        var oldEntity = _dataProperty.FirstOrDefault(d => d.Id == entity.Id);
                        if (oldEntity != null)
                        {
                            _dataProperty.Remove(oldEntity);
                            _dataProperty.Add(entity);
                        }
                    });
            _propertyRepository.Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask).Callback((int id, CancellationToken cancellationToken) =>
                {
                    var entity = _dataProperty.FirstOrDefault(d => d.Id == id);
                    if (entity != null)
                        _dataProperty.Remove(entity);
                });

            _projectRepository = new Mock<IProjectRepository>();
            _projectRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) =>
                    id == 1 ? new Project() {Id = id} : null);
        }

        [Fact]
        public async void AddProjectDataModel_ValidItem()
        {
            var projectDataModelService = new ProjectDataModelService(_dataModelRepository.Object, _propertyRepository.Object, _projectRepository.Object);
            int newId = await projectDataModelService.AddProjectDataModel(1, "Category", null, null);

            Assert.True(newId > 1);
            Assert.True(_data.Count > 1);

            var newData = _data.First(a => a.Id == newId);
            Assert.NotNull(newData.Label);
        }

        [Fact]
        public void AddProjectDataModel_DuplicateItem()
        {
            var projectDataModelService = new ProjectDataModelService(_dataModelRepository.Object, _propertyRepository.Object, _projectRepository.Object);
            var exception = Record.ExceptionAsync(() => projectDataModelService.AddProjectDataModel(1, "Product", null, null));

            Assert.IsType<DuplicateProjectDataModelException>(exception?.Result);
        }

        [Fact]
        public void AddProjectDataModel_InvalidProject()
        {
            var projectDataModelService = new ProjectDataModelService(_dataModelRepository.Object, _propertyRepository.Object, _projectRepository.Object);
            var exception = Record.ExceptionAsync(() => projectDataModelService.AddProjectDataModel(2, "Category", null, null));

            Assert.IsType<ProjectNotFoundException>(exception?.Result);
        }

        [Fact]
        public async void GetProjectDataModels_ReturnItems()
        {
            var projectDataModelService = new ProjectDataModelService(_dataModelRepository.Object, _propertyRepository.Object, _projectRepository.Object);
            var dataModels = await projectDataModelService.GetProjectDataModels(1);

            Assert.NotEmpty(dataModels);
        }

        [Fact]
        public async void GetProjectDataModels_ReturnEmpty()
        {
            var projectDataModelService = new ProjectDataModelService(_dataModelRepository.Object, _propertyRepository.Object, _projectRepository.Object);
            var dataModels = await projectDataModelService.GetProjectDataModels(2);

            Assert.Empty(dataModels);
        }

        [Fact]
        public async void GetProjectDataModelById_ReturnItem()
        {
            var projectDataModelService = new ProjectDataModelService(_dataModelRepository.Object, _propertyRepository.Object, _projectRepository.Object);
            var projectDataModel = await projectDataModelService.GetProjectDataModelById(1);

            Assert.NotNull(projectDataModel);
            Assert.Equal(1, projectDataModel.Id);
        }

        [Fact]
        public async void GetProjectDataModelById_ReturnNull()
        {
            var projectDataModelService = new ProjectDataModelService(_dataModelRepository.Object, _propertyRepository.Object, _projectRepository.Object);
            var projectDataModel = await projectDataModelService.GetProjectDataModelById(2);

            Assert.Null(projectDataModel);
        }

        [Fact]
        public async void GetProjectDataModelByName_ReturnItem()
        {
            var projectDataModelService = new ProjectDataModelService(_dataModelRepository.Object, _propertyRepository.Object, _projectRepository.Object);
            var projectDataModel = await projectDataModelService.GetProjectDataModelByName(1, "Product");

            Assert.NotNull(projectDataModel);
            Assert.Equal(1, projectDataModel.Id);
        }

        [Fact]
        public async void GetProjectDataModelByName_ReturnNull()
        {
            var projectDataModelService = new ProjectDataModelService(_dataModelRepository.Object, _propertyRepository.Object, _projectRepository.Object);
            var projectDataModel = await projectDataModelService.GetProjectDataModelByName(2, "Product");

            Assert.Null(projectDataModel);
        }

        [Fact]
        public async void DeleteDataModel_ValidItem()
        {
            var projectDataModelService = new ProjectDataModelService(_dataModelRepository.Object, _propertyRepository.Object, _projectRepository.Object);
            await projectDataModelService.DeleteDataModel(1);

            Assert.Empty(_data);
            Assert.Empty(_dataProperty);
        }

        [Fact]
        public async void RenameDataModel_ValidItem()
        {
            var projectDataModelService = new ProjectDataModelService(_dataModelRepository.Object, _propertyRepository.Object, _projectRepository.Object);
            await projectDataModelService.UpdateDataModel(new ProjectDataModel
            {
                Id = 1,
                Name = "newName"
            });

            var dataModel = _data.First(d => d.Id == 1);

            Assert.Equal("newName", dataModel.Name);
        }

        [Fact]
        public void RenameDataModel_DuplicateItem()
        {
            _data.Add(new ProjectDataModel
            {
                Id = 2,
                ProjectId = 1,
                Name = "newName"
            });

            var projectDataModelService = new ProjectDataModelService(_dataModelRepository.Object, _propertyRepository.Object, _projectRepository.Object);
            var exception = Record.ExceptionAsync(() => projectDataModelService.UpdateDataModel(new ProjectDataModel
            {
                Id = 1,
                Name = "newName"
            }));

            Assert.IsType<DuplicateProjectDataModelException>(exception?.Result);
        }

        [Fact]
        public async void AddDataModelProperty_ValidItem()
        {
            var projectDataModelService = new ProjectDataModelService(_dataModelRepository.Object, _propertyRepository.Object, _projectRepository.Object);
            int newId = await projectDataModelService.AddDataModelProperty(1, "Price", "Price", "int", "input-text", false, null, null);

            Assert.True(newId > 1);
            Assert.True(_dataProperty.Count > 1);
        }

        [Fact]
        public void AddDataModelProperty_DuplicateItem()
        {
            var projectDataModelService = new ProjectDataModelService(_dataModelRepository.Object, _propertyRepository.Object, _projectRepository.Object);
            var exception = Record.ExceptionAsync(() => projectDataModelService.AddDataModelProperty(1, "Name", "Name", "string", "input-text", true, null, null));

            Assert.IsType<DuplicateProjectDataModelPropertyException>(exception?.Result);
        }

        [Fact]
        public void AddDataModelProperty_InvalidDataModel()
        {
            var projectDataModelService = new ProjectDataModelService(_dataModelRepository.Object, _propertyRepository.Object, _projectRepository.Object);
            var exception = Record.ExceptionAsync(() => projectDataModelService.AddDataModelProperty(2, "Price", "Price", "int", "input-text", false, null, null));

            Assert.IsType<ProjectDataModelNotFoundException>(exception?.Result);
        }

        [Fact]
        public async void DeleteDataModelProperty_ValidItem()
        {
            var projectDataModelService = new ProjectDataModelService(_dataModelRepository.Object, _propertyRepository.Object, _projectRepository.Object);
            await projectDataModelService.DeleteDataModelProperty(1);

            Assert.Empty(_dataProperty);
        }

        [Fact]
        public async void GetDataModelProperties_ReturnItems()
        {
            var projectDataModelService = new ProjectDataModelService(_dataModelRepository.Object, _propertyRepository.Object, _projectRepository.Object);
            var dataModelProperties = await projectDataModelService.GetDataModelProperties(1);

            Assert.NotEmpty(dataModelProperties);
        }

        [Fact]
        public async void GetDataModelProperties_ReturnEmpty()
        {
            var projectDataModelService = new ProjectDataModelService(_dataModelRepository.Object, _propertyRepository.Object, _projectRepository.Object);
            var dataModelProperties = await projectDataModelService.GetDataModelProperties(2);

            Assert.Empty(dataModelProperties);
        }

        [Fact]
        public async void UpdateDataModelProperty_ValidItem()
        {
            var projectDataModelService = new ProjectDataModelService(_dataModelRepository.Object, _propertyRepository.Object, _projectRepository.Object);
            await projectDataModelService.UpdateDataModelProperty(new ProjectDataModelProperty
            {
                Id = 1,
                ProjectDataModelId = 1,
                Name = "newName"
            });

            var dataModelProperty = _dataProperty.First(d => d.Id == 1);

            Assert.Equal("newName", dataModelProperty.Name);
        }

        [Fact]
        public void UpdateDataModelProperty_DuplicateItem()
        {
            _dataProperty.Add(new ProjectDataModelProperty
            {
                Id = 2,
                ProjectDataModelId = 1,
                Name = "newName"
            });

            var projectDataModelService = new ProjectDataModelService(_dataModelRepository.Object, _propertyRepository.Object, _projectRepository.Object);
            var exception = Record.ExceptionAsync(() => projectDataModelService.UpdateDataModelProperty(new ProjectDataModelProperty
            {
                Id = 1,
                ProjectDataModelId = 1,
                Name = "newName"
            }));

            Assert.IsType<DuplicateProjectDataModelPropertyException>(exception?.Result);
        }

        [Fact]
        public async void GetProjectDataModelPropertyById_ReturnItem()
        {
            var projectDataModelService = new ProjectDataModelService(_dataModelRepository.Object, _propertyRepository.Object, _projectRepository.Object);
            var property = await projectDataModelService.GetProjectDataModelPropertyById(1);

            Assert.NotNull(property);
            Assert.Equal(1, property.Id);
        }

        [Fact]
        public async void GetProjectDataModelPropertyById_ReturnNull()
        {
            var projectDataModelService = new ProjectDataModelService(_dataModelRepository.Object, _propertyRepository.Object, _projectRepository.Object);
            var property = await projectDataModelService.GetProjectDataModelPropertyById(2);

            Assert.Null(property);
        }

        [Fact]
        public async void GetProjectDataModelPropertyByName_ReturnItem()
        {
            var projectDataModelService = new ProjectDataModelService(_dataModelRepository.Object, _propertyRepository.Object, _projectRepository.Object);
            var property = await projectDataModelService.GetProjectDataModelPropertyByName(1, "Name");

            Assert.NotNull(property);
            Assert.Equal(1, property.Id);
        }

        [Fact]
        public async void GetProjectDataModelPropertyByName_ReturnNull()
        {
            var projectDataModelService = new ProjectDataModelService(_dataModelRepository.Object, _propertyRepository.Object, _projectRepository.Object);
            var property = await projectDataModelService.GetProjectDataModelPropertyByName(2, "Name");

            Assert.Null(property);
        }
    }
}