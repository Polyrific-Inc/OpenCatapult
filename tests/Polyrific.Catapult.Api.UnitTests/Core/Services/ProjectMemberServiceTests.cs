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
    public class ProjectMemberServiceTests
    {
        private readonly List<ProjectMember> _data;
        private readonly Mock<IProjectMemberRepository> _projectMemberRepository;
        private readonly Mock<IProjectRepository> _projectRepository;
        private readonly Mock<IUserRepository> _userRepository;

        public ProjectMemberServiceTests()
        {
            _data = new List<ProjectMember>
            {
                new ProjectMember
                {
                    Id = 1,
                    ProjectId = 1,
                    UserId = 1,
                    ProjectMemberRoleId = 1
                }
            };

            var userData = new List<User>
            {
                new User
                {
                    Id = 1,
                    UserName = "test@test.com",
                    Email = "test@test.com"
                }
            };

            _projectMemberRepository = new Mock<IProjectMemberRepository>();
            _projectMemberRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                (int id, CancellationToken cancellationToken) => { return _data.FirstOrDefault(d => d.Id == id); });
            _projectMemberRepository.Setup(r =>
                    r.GetSingleBySpec(It.IsAny<ProjectMemberFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProjectMemberFilterSpecification spec, CancellationToken cancellationToken) =>
                    _data.FirstOrDefault(d => d.ProjectId == spec.ProjectId && d.UserId == spec.UserId));
            _projectMemberRepository.Setup(r =>
                    r.CountBySpec(It.IsAny<ProjectMemberFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProjectMemberFilterSpecification spec, CancellationToken cancellationToken) =>
                    _data.Count(d => d.ProjectId == spec.ProjectId && d.UserId == spec.UserId));
            _projectMemberRepository.Setup(s =>
                    s.GetBySpec(It.IsAny<ProjectMemberFilterSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProjectMemberFilterSpecification spec, CancellationToken cancellationToken) =>
                    _data.Where(m =>
                        (spec.ProjectId == 0 || m.ProjectId == spec.ProjectId) &&
                        (spec.UserId == 0 || m.UserId == spec.UserId)));
            _projectMemberRepository.Setup(r => r.Create(It.IsAny<ProjectMember>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(2).Callback((ProjectMember entity, CancellationToken cancellationToken) =>
                {
                    entity.Id = 2;
                    _data.Add(entity);
                });
            _projectMemberRepository.Setup(r => r.Update(It.IsAny<ProjectMember>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask).Callback((ProjectMember entity, CancellationToken cancellationToken) =>
                {
                    var oldEntity = _data.FirstOrDefault(d => d.Id == entity.Id);
                    if (oldEntity != null)
                    {
                        _data.Remove(oldEntity);
                        _data.Add(entity);
                    }
                });
            _projectMemberRepository.Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask).Callback((int id, CancellationToken cancellationToken) =>
                {
                    var entity = _data.FirstOrDefault(d => d.Id == id);
                    if (entity != null)
                        _data.Remove(entity);
                });

            _projectRepository = new Mock<IProjectRepository>();
            _projectRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) => id == 1 ? new Project() { Id = id } : null);

            _userRepository = new Mock<IUserRepository>();
            _userRepository.Setup(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken cancellationToken) =>
                    (new List<int> {1, 2}).Contains(id) ? new User() {Id = id} : null);
            _userRepository.Setup(s => s.GetByUserName(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string email, CancellationToken cancellationToken) =>
                    userData.FirstOrDefault(u => u.UserName.ToLower() == email.ToLower()));
            _userRepository.Setup(r => r.Create(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(2);
        }

        [Fact]
        public async void AddProjectMember_ValidItem()
        {
            var projectMemberService = new ProjectMemberService(_projectMemberRepository.Object, _projectRepository.Object, _userRepository.Object);
            var id = await projectMemberService.AddProjectMember(1, 2, 1);

            Assert.True(_data.Count > 1);
            Assert.True(id > 1);
        }

        [Fact]
        public async void AddProjectMember_DuplicateItem()
        {            
            var projectMemberService = new ProjectMemberService(_projectMemberRepository.Object, _projectRepository.Object, _userRepository.Object);
            var id = await projectMemberService.AddProjectMember(1, 1, 2);

            var member = _data.First(d => d.ProjectId == 1 && d.UserId == 1);

            Assert.True(_data.Count == 1);
            Assert.Equal(1, id);
            Assert.Equal(2, member.ProjectMemberRoleId);
        }

        [Fact]
        public void AddProjectMember_InvalidProject()
        {
            var projectMemberService = new ProjectMemberService(_projectMemberRepository.Object, _projectRepository.Object, _userRepository.Object);
            var exception = Record.ExceptionAsync(() => projectMemberService.AddProjectMember(2, 2, 1));

            Assert.IsType<ProjectNotFoundException>(exception?.Result);
        }

        [Fact]
        public void AddProjectMember_InvalidUser()
        {
            var projectMemberService = new ProjectMemberService(_projectMemberRepository.Object, _projectRepository.Object, _userRepository.Object);
            var exception = Record.ExceptionAsync(() => projectMemberService.AddProjectMember(1, 3, 1));

            Assert.IsType<UserNotFoundException>(exception?.Result);
        }

        [Fact]
        public async void AddProjectMember_NewUser()
        {
            var projectMemberService = new ProjectMemberService(_projectMemberRepository.Object, _projectRepository.Object, _userRepository.Object);
            var (memberId, userId) = await projectMemberService.AddProjectMember(1, "user@example.com", "New", "User", 1);

            Assert.True(_data.Count > 1);
            Assert.True(memberId > 1);
            Assert.True(userId > 1);
        }

        [Fact]
        public void AddProjectMember_NewUserInvalidProject()
        {
            var projectMemberService = new ProjectMemberService(_projectMemberRepository.Object, _projectRepository.Object, _userRepository.Object);
            var exception = Record.ExceptionAsync(() => projectMemberService.AddProjectMember(2, 2, 1));

            Assert.IsType<ProjectNotFoundException>(exception?.Result);
        }

        [Fact]
        public void AddProjectMember_DuplicateEmail()
        {
            var projectMemberService = new ProjectMemberService(_projectMemberRepository.Object, _projectRepository.Object, _userRepository.Object);
            var exception = Record.ExceptionAsync(() => projectMemberService.AddProjectMember(1, "test@test.com", "New", "User", 1));

            Assert.IsType<DuplicateUserEmailException>(exception?.Result);
        }

        [Fact]
        public async void GetProjectMembers_ReturnItems()
        {
            var projectMemberService = new ProjectMemberService(_projectMemberRepository.Object, _projectRepository.Object, _userRepository.Object);
            var members = await projectMemberService.GetProjectMembers(1);

            Assert.NotEmpty(members);
        }

        [Fact]
        public async void GetProjectMembers_ReturnEmpty()
        {
            var projectMemberService = new ProjectMemberService(_projectMemberRepository.Object, _projectRepository.Object, _userRepository.Object);
            var members = await projectMemberService.GetProjectMembers(2);

            Assert.Empty(members);
        }

        [Fact]
        public async void RemoveProjectMember_ValidItem()
        {
            var projectMemberService = new ProjectMemberService(_projectMemberRepository.Object, _projectRepository.Object, _userRepository.Object);
            await projectMemberService.RemoveProjectMember(1, 1, 1);

            Assert.Empty(_data);
        }

        [Fact]
        public void RemoveProjectMember_RemoveProjectOwnerException()
        {
            var projectMemberService = new ProjectMemberService(_projectMemberRepository.Object, _projectRepository.Object, _userRepository.Object);
            var exception = Record.ExceptionAsync(() => projectMemberService.RemoveProjectMember(1, 1, 2));

            Assert.IsType<RemoveProjectOwnerException>(exception?.Result);
        }

        [Fact]
        public async void UpdateProjectMemberRole_ValidItem()
        {
            var projectMemberService = new ProjectMemberService(_projectMemberRepository.Object, _projectRepository.Object, _userRepository.Object);
            await projectMemberService.UpdateProjectMemberRole(1, 1, 2);

            var member = _data.First(d => d.ProjectId == 1 && d.UserId == 1);

            Assert.Equal(2, member.ProjectMemberRoleId);
        }

        [Fact]
        public async void GetProjectMemberById_ReturnItem()
        {
            var projectMemberService = new ProjectMemberService(_projectMemberRepository.Object, _projectRepository.Object, _userRepository.Object);
            var projectMember = await projectMemberService.GetProjectMemberById(1);

            Assert.NotNull(projectMember);
            Assert.Equal(1, projectMember.Id);
        }

        [Fact]
        public async void GetProjectMemberById_ReturnNull()
        {
            var projectMemberService = new ProjectMemberService(_projectMemberRepository.Object, _projectRepository.Object, _userRepository.Object);
            var projectMember = await projectMemberService.GetProjectMemberById(2);

            Assert.Null(projectMember);
        }
    }
}
