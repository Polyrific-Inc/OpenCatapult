// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using AutoMapper;
using Newtonsoft.Json;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Specifications;
using Polyrific.Catapult.Shared.Dto.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Polyrific.Catapult.Api.Core.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository projectRepository, IProjectMemberRepository projectMemberRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _projectMemberRepository = projectMemberRepository;
            _mapper = mapper;
        }

        public async Task ArchiveProject(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var project = await _projectRepository.GetById(id, cancellationToken);
            if (project == null)
                throw new ProjectNotFoundException(id);
            
            project.IsArchived = true;
            await _projectRepository.Update(project, cancellationToken);
        }

        public async Task<Project> CloneProject(int sourceProjectId, string newProjectName, bool includeMembers = false,
            bool includeJobDefinitions = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var projectByNameSpec = new ProjectFilterSpecification(newProjectName);
            var duplicateProjects = await _projectRepository.GetBySpec(projectByNameSpec, cancellationToken);
            if (duplicateProjects.Any())
                throw new DuplicateProjectException(newProjectName);

            var projectByIdSpec = new ProjectFilterSpecification(sourceProjectId);
            projectByIdSpec.IncludeStrings.Add("Models.Properties");
            projectByIdSpec.IncludeStrings.Add("Jobs.Tasks");
            projectByIdSpec.IncludeStrings.Add("Members");
            var sourceProject = await _projectRepository.GetSingleBySpec(projectByIdSpec, cancellationToken);
            if (sourceProject == null)
                throw new ProjectNotFoundException(sourceProjectId);

            var newProject = new Project
            {
                Name = newProjectName,
                ConfigString = sourceProject.ConfigString,
                Models = sourceProject.Models?.Select(m => new ProjectDataModel
                {
                    Name = m.Name,
                    Description = m.Description,
                    Label = m.Label,
                    Properties = m.Properties?.Select(p => new ProjectDataModelProperty
                    {
                        Name = p.Name,
                        Label = p.Label,
                        DataType = p.DataType,
                        ControlType = p.ControlType,
                        RelatedProjectDataModelId = p.RelatedProjectDataModelId,
                        RelationalType = p.RelationalType,
                        IsRequired = p.IsRequired,
                        Created = DateTime.UtcNow
                    }).ToList(),
                    Created = DateTime.UtcNow
                }).ToList()
            };

            if (includeJobDefinitions)
            {
                newProject.Jobs = sourceProject?.Jobs.Select(j => new JobDefinition
                {
                    Name = j.Name,
                    Tasks = j.Tasks?.Select(sourceTask => new JobTaskDefinition
                    {
                        Name = sourceTask.Name,
                        Type = sourceTask.Type,
                        Sequence = sourceTask.Sequence,
                        ConfigString = sourceTask.ConfigString,
                        ContinueWhenError = sourceTask.ContinueWhenError,
                        Created = DateTime.UtcNow
                    }).ToList(),
                    Created = DateTime.UtcNow
                }).ToList();
            }

            if (includeMembers)
            {
                newProject.Members = sourceProject?.Members?.Select(m => new ProjectMember
                {
                    UserId = m.UserId,
                    ProjectMemberRoleId = m.ProjectMemberRoleId,
                    Created = DateTime.UtcNow
                }).ToList();
            }

            var newProjectId = await _projectRepository.Create(newProject, cancellationToken);

            return newProject;
        }

        public async Task<Project> CreateProject(string projectName, string client, List<(int userId, int projectMemberRoleId)> projectMembers, Dictionary<string, string> config, List<ProjectDataModel> models, List<JobDefinition> jobs, int currentUserId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var projectByNameSpec = new ProjectFilterSpecification(projectName);
            var duplicateProjectsCount = await _projectRepository.CountBySpec(projectByNameSpec, cancellationToken);
            if (duplicateProjectsCount > 0)
                throw new DuplicateProjectException(projectName);

            var newProject = new Project{ Name = projectName, Client = client };
            newProject.Models = models;
            newProject.Jobs = jobs;
            newProject.ConfigString = config != null && config.Count > 0 ? JsonConvert.SerializeObject(config) : null;

            projectMembers = projectMembers ?? new List<(int userId, int projectMemberRoleId)>();
            if (!projectMembers.Any(p => p.userId == currentUserId))
            {
                projectMembers.Add((currentUserId, MemberRole.OwnerId));
            }

            if (projectMembers != null && projectMembers.Any())
            {
                newProject.Members = projectMembers.Select(m => new ProjectMember
                {
                    UserId = m.userId,
                    ProjectMemberRoleId = m.projectMemberRoleId,
                    Created = DateTime.UtcNow
                }).ToList();
            }

            if (newProject.Models != null)
            {
                foreach (var model in newProject.Models)
                {
                    model.Created = DateTime.UtcNow;

                    if (model.Properties != null)
                    {
                        foreach (var property in model.Properties)
                        {
                            property.Created = DateTime.UtcNow;
                        }
                    }
                }
            }

            if (newProject.Jobs != null)
            {
                foreach (var job in newProject.Jobs)
                {
                    job.Created = DateTime.UtcNow;

                    if (job.Tasks != null)
                    {
                        foreach (var task in job.Tasks)
                        {
                            task.Created = DateTime.UtcNow;
                        }
                    }
                }
            }

            var newProjectId = await _projectRepository.Create(newProject, cancellationToken);

            return newProject;
        }

        public async Task DeleteProject(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _projectRepository.Delete(id, cancellationToken);
        }

        public async Task<string> ExportProject(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var projectFilter = new ProjectFilterSpecification(id);
            projectFilter.IncludeStrings.Add("Models.Properties");
            projectFilter.IncludeStrings.Add("Jobs.Tasks");
            var project = await _projectRepository.GetSingleBySpec(projectFilter, cancellationToken);

            if (project != null)
            {
                var projectTemplate = _mapper.Map<ProjectTemplate>(project);
                return YamlSerialize(projectTemplate);
            }
            else
            {
                throw new ProjectNotFoundException(id);
            }
        }

        public async Task<Project> GetProjectById(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _projectRepository.GetById(id, cancellationToken);
        }

        public async Task<Project> GetProjectByName(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _projectRepository.GetSingleBySpec(new ProjectFilterSpecification(name), cancellationToken);
        }

        public async Task<List<(Project, ProjectMemberRole)>> GetProjectsByUser(int userId, string status = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            bool? isArchived;
            switch (status)
            {
                case null:
                case "":
                case ProjectStatusFilterType.All:
                    isArchived = null;
                    break;
                case ProjectStatusFilterType.Active:
                    isArchived = false;
                    break;
                case ProjectStatusFilterType.Archived:
                    isArchived = true;
                    break;
                default:
                    throw new FilterTypeNotFoundException(status);
            }

            var projectMembersByUserSpec = new ProjectMemberFilterSpecification(0, userId, isArchived);
            projectMembersByUserSpec.Includes.Add(p => p.ProjectMemberRole);
            var projectMembers = await _projectMemberRepository.GetBySpec(projectMembersByUserSpec, cancellationToken);
            var projects = projectMembers.Select(m => (m.Project, m.ProjectMemberRole)).ToList();

            return projects;
        }

        public async Task RestoreProject(int projectId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var project = await _projectRepository.GetById(projectId, cancellationToken);
            if (project == null)
                throw new ProjectNotFoundException(projectId);

            project.IsArchived = false;
            await _projectRepository.Update(project, cancellationToken);
        }

        public async Task UpdateProject(Project project, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var projectByNameSpec = new ProjectFilterSpecification(project.Name, project.Id);
            if (await _projectRepository.CountBySpec(projectByNameSpec, cancellationToken) > 0)
                throw new DuplicateProjectException(project.Name);

            var entity = await _projectRepository.GetById(project.Id);
            if (entity != null)
            {
                entity.Name = project.Name;
                entity.Client = project.Client;
                entity.ConfigString = project.ConfigString;
                await _projectRepository.Update(entity);
            }            
        }

        private string YamlSerialize(ProjectTemplate template)
        {
            var serializer = new SerializerBuilder().WithNamingConvention(new HyphenatedNamingConvention()).Build();
            return serializer.Serialize(template);
        }
    }
}