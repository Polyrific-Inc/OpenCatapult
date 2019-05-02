// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Specifications;
using Polyrific.Catapult.Shared.Dto.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Core.Services
{
    public class ProjectMemberService : IProjectMemberService
    {
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserService _userService;

        public ProjectMemberService(IProjectMemberRepository projectMemberRepository, IProjectRepository projectRepository, IUserService userService)
        {
            _projectMemberRepository = projectMemberRepository;
            _projectRepository = projectRepository;
            _userService = userService;
        }

        public async Task<int> AddProjectMember(int projectId, int userId, int roleId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var project = await _projectRepository.GetById(projectId, cancellationToken);
            if (project == null)
            {
                throw new ProjectNotFoundException(projectId);
            }

            var user = await _userService.GetUserById(userId, cancellationToken);
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }

            var projectMemberByProjectSpec = new ProjectMemberFilterSpecification(projectId, userId);
            var projectMember = await _projectMemberRepository.GetSingleBySpec(projectMemberByProjectSpec, cancellationToken);

            if (projectMember != null)
            {
                projectMember.ProjectMemberRoleId = roleId;
                await _projectMemberRepository.Update(projectMember, cancellationToken);
                return projectMember.Id;
            }

            var newProjectMember = new ProjectMember { ProjectId = projectId, UserId = userId, ProjectMemberRoleId = roleId };
            return await _projectMemberRepository.Create(newProjectMember, cancellationToken);
        }

        public async Task<(int newProjectMemberId, int newUserId)> AddProjectMember(int projectId, string email, string firstName, string lastName, Dictionary<string, string> externalAccountIds, int roleId, string webUrl, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var project = await _projectRepository.GetById(projectId, cancellationToken);
            if (project == null)
            {
                throw new ProjectNotFoundException(projectId);
            }

            var user = await _userService.GetUser(email, cancellationToken);
            if (user != null)
            {
                throw new DuplicateUserEmailException(email);
            }

            var newUser = await _userService.CreateUser(email, firstName, lastName, null, externalAccountIds, await _userService.GeneratePassword(), webUrl, cancellationToken);

            var newProjectMember = new ProjectMember { ProjectId = projectId, ProjectMemberRoleId = roleId, UserId = newUser.Id };
            var newProjectMemberId = await _projectMemberRepository.Create(newProjectMember, cancellationToken);

            return (newProjectMemberId, newUser.Id);
        }

        public async Task<List<ProjectMember>> GetProjectMembers(int projectId, int roleId = 0, bool includeUser = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var projectMemberByProjectSpec = new ProjectMemberFilterSpecification(projectId, 0, roleId: roleId);
            
            var projectMembers = (await _projectMemberRepository.GetBySpec(projectMemberByProjectSpec, cancellationToken)).ToList();

            if (includeUser)
            {
                var users = await _userService.GetUsersByIds(projectMembers.Select(p => p.UserId).ToArray());

                foreach (var projectMember in projectMembers)
                    projectMember.User = users.FirstOrDefault(u => u.Id == projectMember.UserId);
            }

            return projectMembers;
        }

        public async Task<ProjectMember> GetProjectMemberById(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var projectMemberSpec = new ProjectMemberFilterSpecification(id);
            return await _projectMemberRepository.GetSingleBySpec(projectMemberSpec, cancellationToken);
        }

        public async Task<ProjectMember> GetProjectMemberByUserId(int projectId, int userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var projectMemberByProjectSpec = new ProjectMemberFilterSpecification(projectId, userId);
            return await _projectMemberRepository.GetSingleBySpec(projectMemberByProjectSpec, cancellationToken);
        }

        public async Task RemoveProjectMember(int projectId, int userId, int currentUserId = 0, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var projectMemberByProjectSpec = new ProjectMemberFilterSpecification(projectId, userId);
            var projectMember = await _projectMemberRepository.GetSingleBySpec(projectMemberByProjectSpec, cancellationToken);

            if (projectMember.ProjectMemberRoleId == MemberRole.OwnerId && currentUserId > 0)
            {
                var currentUserOwnerSpec = new ProjectMemberFilterSpecification(projectId, currentUserId, null, MemberRole.OwnerId);
                if (await _projectMemberRepository.CountBySpec(currentUserOwnerSpec) == 0)
                    throw new RemoveProjectOwnerException(currentUserId);
            }

            var projectMemberId = projectMember?.Id;

            if (projectMemberId > 0)
                await _projectMemberRepository.Delete(projectMemberId.GetValueOrDefault(), cancellationToken);
        }

        public async Task UpdateProjectMemberRole(int projectId, int userId, int roleId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var projectMemberByProjectSpec = new ProjectMemberFilterSpecification(projectId, userId);
            var projectMember = await _projectMemberRepository.GetSingleBySpec(projectMemberByProjectSpec, cancellationToken);

            if (projectMember != null)
            {
                projectMember.ProjectMemberRoleId = roleId;
                await _projectMemberRepository.Update(projectMember, cancellationToken);
            }
        }
    }
}
