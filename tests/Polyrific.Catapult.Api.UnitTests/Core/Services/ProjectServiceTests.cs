// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.Core.Specifications;
using Polyrific.Catapult.Api.UnitTests.Utilities;
using Polyrific.Catapult.Shared.Dto.Constants;
using Xunit;

namespace Polyrific.Catapult.Api.UnitTests.Core.Services
{
    public class ProjectServiceTests
    {
        private readonly List<Project> _data;
        private readonly List<ProjectMember> _dataMember;
        private readonly Mock<IProjectRepository> _projectRepository;
        private readonly Mock<IProjectMemberRepository> _projectMemberRepository;
        private readonly Mock<IProjectDataModelPropertyRepository> _projectDataModelPropertyRepository;
        private readonly Mock<IJobDefinitionService> _jobDefinitionService;
        private readonly IMapper _mapper;

        public ProjectServiceTests()
        {
            _data = new List<Project>
            {
                new Project
                {
                    Id = 1,
                    Name = "Project-A",
                    Status = ProjectStatusFilterType.Active
                }
            };

            _dataMember = new List<ProjectMember>
            {
                new ProjectMember
                {
                    Id = 1,
                    ProjectId = 1,
                    UserId = 100,
                    ProjectMemberRoleId = 1,
                    Project = new Project()
                }
            };

            _projectRepository = new Mock<IProjectRepository>();
            _projectRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                (int id, CancellationToken cancellationToken) => { return _data.FirstOrDefault(d => d.Id == id); });
            _projectRepository.Setup(r =>
                    r.GetSingleBySpec(It.IsAny<ProjectFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProjectFilterSpecification spec, CancellationToken cancellationToken) =>
                    _data.FirstOrDefault(spec.Criteria.Compile()));
            _projectRepository.Setup(r =>
                    r.CountBySpec(It.IsAny<ProjectFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProjectFilterSpecification spec, CancellationToken cancellationToken) =>
                    _data.Count(spec.Criteria.Compile()));
            _projectRepository.Setup(r => r.Create(It.IsAny<Project>(), It.IsAny<CancellationToken>())).ReturnsAsync(2)
                .Callback((Project entity, CancellationToken cancellationToken) =>
                {
                    entity.Id = 2;
                    _data.Add(entity);
                });
            _projectRepository.Setup(r => r.Update(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask).Callback((Project entity, CancellationToken cancellationToken) =>
                {
                    var oldEntity = _data.FirstOrDefault(d => d.Id == entity.Id);
                    if (oldEntity != null)
                    {
                        _data.Remove(oldEntity);
                        _data.Add(entity);
                    }
                });
            _projectRepository.Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask).Callback((int id, CancellationToken cancellationToken) =>
                {
                    var entity = _data.FirstOrDefault(d => d.Id == id);
                    if (entity != null)
                        _data.Remove(entity);
                });

            _projectMemberRepository = new Mock<IProjectMemberRepository>();
            _projectMemberRepository.Setup(s =>
                    s.GetBySpec(It.IsAny<ProjectMemberFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProjectMemberFilterSpecification spec, CancellationToken cancellationToken) =>
                    _dataMember.Where(spec.Criteria.Compile()));
            _projectMemberRepository.Setup(r => r.Create(It.IsAny<ProjectMember>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(2).Callback((ProjectMember entity, CancellationToken cancellationToken) =>
                {
                    entity.Id = 2;
                    _dataMember.Add(entity);
                });

            _projectDataModelPropertyRepository = new Mock<IProjectDataModelPropertyRepository>();

            _jobDefinitionService = new Mock<IJobDefinitionService>();
            
            _mapper = AutoMapperUtils.GetMapper();
        }

        [Fact]
        public async void ArchiveProject_ValidItem()
        {
            var projectService = new ProjectService(_projectRepository.Object, _projectMemberRepository.Object, _projectDataModelPropertyRepository.Object, _mapper, _jobDefinitionService.Object);
            await projectService.ArchiveProject(1);
            
            Assert.Equal(ProjectStatusFilterType.Archived, _data.First(p => p.Id == 1).Status);
        }

        [Fact]
        public void ArchiveProject_ItemNotExist()
        {
            var projectService = new ProjectService(_projectRepository.Object, _projectMemberRepository.Object, _projectDataModelPropertyRepository.Object, _mapper, _jobDefinitionService.Object);
            var exception = Record.ExceptionAsync(() => projectService.ArchiveProject(2));

            Assert.IsType<ProjectNotFoundException>(exception?.Result);
        }

        [Fact]
        public async void RestoreProject_ValidItem()
        {
            var projectService = new ProjectService(_projectRepository.Object, _projectMemberRepository.Object, _projectDataModelPropertyRepository.Object, _mapper, _jobDefinitionService.Object);
            await projectService.RestoreProject(1);

            Assert.Equal(ProjectStatusFilterType.Active, _data.First(p => p.Id == 1).Status);
        }

        [Fact]
        public void RestoreProject_ItemNotExist()
        {
            var projectService = new ProjectService(_projectRepository.Object, _projectMemberRepository.Object, _projectDataModelPropertyRepository.Object, _mapper, _jobDefinitionService.Object);
            var exception = Record.ExceptionAsync(() => projectService.RestoreProject(2));

            Assert.IsType<ProjectNotFoundException>(exception?.Result);
        }

        [Fact]
        public async void UpdateProject_ValidItem()
        {
            var projectService = new ProjectService(_projectRepository.Object, _projectMemberRepository.Object, _projectDataModelPropertyRepository.Object, _mapper, _jobDefinitionService.Object);
            await projectService.UpdateProject(new Project
            {
                Id = 1,
                Name = "renamed"
            });

            Assert.Equal("renamed", _data.First(p => p.Id == 1).Name);
        }

        [Fact]
        public void UpdateProject_DuplicateProjectException()
        {
            _data.Add(new Project
            {
                Id = 2,
                Name = "existing"
            });

            var projectService = new ProjectService(_projectRepository.Object, _projectMemberRepository.Object, _projectDataModelPropertyRepository.Object, _mapper, _jobDefinitionService.Object);
            var exception = Record.ExceptionAsync(() => projectService.UpdateProject(new Project
            {
                Id = 1,
                Name = "existing"
            }));

            Assert.IsType<DuplicateProjectException>(exception?.Result);
        }

        [Fact]
        public async void CloneProject_ValidSource()
        {
            var projectService = new ProjectService(_projectRepository.Object, _projectMemberRepository.Object, _projectDataModelPropertyRepository.Object, _mapper, _jobDefinitionService.Object);
            var newProject = await projectService.CloneProject(1, "Project B", null, null, 1);
            
            Assert.True(_data.Count > 1);
            Assert.True(newProject.Id > 1);
            Assert.True(newProject.Name == "Project-B"); // check normalization logic
        }

        [Fact]
        public void CloneProject_SourceNotExist()
        {
            var projectService = new ProjectService(_projectRepository.Object, _projectMemberRepository.Object, _projectDataModelPropertyRepository.Object, _mapper, _jobDefinitionService.Object);
            var exception = Record.ExceptionAsync(() => projectService.CloneProject(2, "Project B", null, null, 1));

            Assert.IsType<ProjectNotFoundException>(exception?.Result);
        }

        [Fact]
        public async void CreateProject_ValidItem()
        {
            var projectService = new ProjectService(_projectRepository.Object, _projectMemberRepository.Object, _projectDataModelPropertyRepository.Object, _mapper, _jobDefinitionService.Object);
            var newProject = await projectService.CreateProject("Project  B   test", "Project B", "Client B", null, null, null, 1);

            Assert.True(_data.Count > 1);
            Assert.True(newProject.Id > 1);
            Assert.True(newProject.Name == "Project-B-test"); // check normalization logic
        }

        [Fact]
        public async void CreateProject_WithMembers()
        {
            var projectService = new ProjectService(_projectRepository.Object, _projectMemberRepository.Object, _projectDataModelPropertyRepository.Object, _mapper, _jobDefinitionService.Object);
            var newProject = await projectService.CreateProject("Project-B", "Project B", "Client B", null, null, null, 1);

            Assert.True(newProject.Members.Count > 0);
        }

        [Fact]
        public void CreateProject_DuplicateItemName()
        {
            var projectService = new ProjectService(_projectRepository.Object, _projectMemberRepository.Object, _projectDataModelPropertyRepository.Object, _mapper, _jobDefinitionService.Object);
            var exception = Record.ExceptionAsync(() => projectService.CreateProject("Project-A", "Project A", "Client A", null, null, null, 1));

            Assert.IsType<DuplicateProjectException>(exception?.Result);
        }

        [Fact]
        public async void DeleteProject_ValidItem()
        {
            var projectService = new ProjectService(_projectRepository.Object, _projectMemberRepository.Object, _projectDataModelPropertyRepository.Object, _mapper, _jobDefinitionService.Object);
            await projectService.DeleteProject(1);

            Assert.Empty(_data);
        }

        [Fact]
        public async void GetProjectById_ReturnItem()
        {
            var projectService = new ProjectService(_projectRepository.Object, _projectMemberRepository.Object, _projectDataModelPropertyRepository.Object, _mapper, _jobDefinitionService.Object);
            var project = await projectService.GetProjectById(1);

            Assert.NotNull(project);
            Assert.Equal(1, project.Id);
        }

        [Fact]
        public async void GetProjectById_ReturnNull()
        {
            var projectService = new ProjectService(_projectRepository.Object, _projectMemberRepository.Object, _projectDataModelPropertyRepository.Object, _mapper, _jobDefinitionService.Object);
            var project = await projectService.GetProjectById(2);

            Assert.Null(project);
        }

        [Fact]
        public async void GetProjectByName_ReturnItem()
        {
            var projectService = new ProjectService(_projectRepository.Object, _projectMemberRepository.Object, _projectDataModelPropertyRepository.Object, _mapper, _jobDefinitionService.Object);
            var project = await projectService.GetProjectByName("Project-A");

            Assert.NotNull(project);
            Assert.Equal(1, project.Id);
        }

        [Fact]
        public async void GetProjectByName_ReturnNull()
        {
            var projectService = new ProjectService(_projectRepository.Object, _projectMemberRepository.Object, _projectDataModelPropertyRepository.Object, _mapper, _jobDefinitionService.Object);
            var project = await projectService.GetProjectByName("Project B");

            Assert.Null(project);
        }

        [Fact]
        public async void GetProjectsByUser_ReturnItems()
        {
            var projectService = new ProjectService(_projectRepository.Object, _projectMemberRepository.Object, _projectDataModelPropertyRepository.Object, _mapper, _jobDefinitionService.Object);
            var projects = await projectService.GetProjectsByUser(100);

            Assert.NotEmpty(projects);
        }

        [Fact]
        public void GetProjectsByUser_FilterTypeNotFound()
        {
            var projectService = new ProjectService(_projectRepository.Object, _projectMemberRepository.Object, _projectDataModelPropertyRepository.Object, _mapper, _jobDefinitionService.Object);
            var exception = Record.ExceptionAsync(() => projectService.GetProjectsByUser(100, "test"));

            Assert.IsType<FilterTypeNotFoundException>(exception?.Result);
        }

        [Fact]
        public async void GetProjectsByUser_ReturnEmpty()
        {
            var projectService = new ProjectService(_projectRepository.Object, _projectMemberRepository.Object, _projectDataModelPropertyRepository.Object, _mapper, _jobDefinitionService.Object);
            var projects = await projectService.GetProjectsByUser(200);

            Assert.Empty(projects);
        }

        [Fact]
        public async void ExportProject_ReturnYaml()
        {
            var projectService = new ProjectService(_projectRepository.Object, _projectMemberRepository.Object, _projectDataModelPropertyRepository.Object, _mapper, _jobDefinitionService.Object);
            var projectYaml = await projectService.ExportProject(1);
            Assert.NotNull(projectYaml);
        }
    }
}
