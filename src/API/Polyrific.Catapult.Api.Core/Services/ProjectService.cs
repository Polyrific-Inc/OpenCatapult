// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Specifications;
using Polyrific.Catapult.Shared.Common;
using Polyrific.Catapult.Shared.Common.Notification;
using Polyrific.Catapult.Shared.Dto.Constants;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Polyrific.Catapult.Api.Core.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;
        private readonly IProjectDataModelPropertyRepository _projectDataModelPropertyRepository;
        private readonly IMapper _mapper;
        private readonly IJobDefinitionService _jobDefinitionService;
        private readonly IJobQueueService _jobQueueService;
        private readonly INotificationProvider _notificationProvider;

        public ProjectService(IProjectRepository projectRepository, IProjectMemberRepository projectMemberRepository, IProjectDataModelPropertyRepository projectDataModelPropertyRepository, 
            IMapper mapper, IJobDefinitionService jobDefinitionService, IJobQueueService jobQueueService, INotificationProvider notificationProvider)
        {
            _projectRepository = projectRepository;
            _projectMemberRepository = projectMemberRepository;
            _projectDataModelPropertyRepository = projectDataModelPropertyRepository;
            _mapper = mapper;
            _jobDefinitionService = jobDefinitionService;
            _jobQueueService = jobQueueService;
            _notificationProvider = notificationProvider;
        }

        public async Task ArchiveProject(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var project = await _projectRepository.GetById(id, cancellationToken);
            if (project == null)
                throw new ProjectNotFoundException(id);
            
            project.Status = ProjectStatusFilterType.Archived;
            await _projectRepository.Update(project, cancellationToken);
        }

        public async Task<Project> CloneProject(int sourceProjectId, string newProjectName, string newDisplayName, string newClient, int ownerUserId, bool includeMembers = false,
            bool includeJobDefinitions = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            newProjectName = GetNormalizedProjectName(newProjectName);

            // set default display name
            newDisplayName = !string.IsNullOrEmpty(newDisplayName) ? newDisplayName : newProjectName;

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
                DisplayName = newDisplayName,
                Client = newClient ?? sourceProject.Client,
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
                        Provider = sourceTask.Provider,
                        Sequence = sourceTask.Sequence,
                        ConfigString = sourceTask.ConfigString,
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
            else
            {
                newProject.Members = new List<ProjectMember>
                {
                    new ProjectMember
                    {
                        UserId = ownerUserId,
                        ProjectMemberRoleId = MemberRole.OwnerId,
                        Created = DateTime.UtcNow
                    }
                };
            }

            var newProjectId = await _projectRepository.Create(newProject, cancellationToken);

            return newProject;
        }

        public async Task<Project> CreateProject(string projectName, string displayName, string client, List<(int userId, int projectMemberRoleId)> projectMembers, List<ProjectDataModel> models, List<JobDefinition> jobs, int currentUserId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            // set default display name
            displayName = !string.IsNullOrEmpty(displayName) ? displayName : TextHelper.HumanizeText(projectName);

            projectName = GetNormalizedProjectName(projectName);

            var projectByNameSpec = new ProjectFilterSpecification(projectName);
            var duplicateProjectsCount = await _projectRepository.CountBySpec(projectByNameSpec, cancellationToken);
            if (duplicateProjectsCount > 0)
                throw new DuplicateProjectException(projectName);
            
            var newProject = new Project{ Name = projectName, DisplayName = displayName, Client = client };
            newProject.Models = models;
            newProject.Jobs = jobs;

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

            List<ProjectDataModelProperty> propertiesWithRelational = null;
            if (newProject.Models != null)
            {
                // validate related models
                propertiesWithRelational = newProject.Models.SelectMany(m => m.Properties).Where(p => !string.IsNullOrEmpty(p.RelatedProjectDataModelName)).ToList();
                var propertyWithInvalidRelational = propertiesWithRelational.FirstOrDefault(p => !models.Any(m => m.Name == p.RelatedProjectDataModelName));
                if (propertyWithInvalidRelational != null)
                {
                    throw new ProjectDataModelNotFoundException(propertyWithInvalidRelational.RelatedProjectDataModelName);
                }

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
                        var duplicateTasks = job.Tasks.GroupBy(x => x.Name.ToLower())
                          .Where(g => g.Count() > 1)
                          .Select(y => y.Key)
                          .ToList();
                        if (duplicateTasks.Count > 0)
                        {
                            throw new DuplicateJobTaskDefinitionException(string.Join(DataDelimiter.Comma.ToString(), duplicateTasks));
                        }

                        int sequence = 1;
                        foreach (var task in job.Tasks)
                        {
                            task.Created = DateTime.UtcNow;
                            task.Sequence = sequence++;
                            await _jobDefinitionService.ValidateJobTaskDefinition(job, task);
                        }
                    }
                }
            }

            var newProjectId = await _projectRepository.Create(newProject, cancellationToken);

            // map the relational property from RelatedProjectDataModelName
            if (propertiesWithRelational != null)
            {
                foreach (var property in propertiesWithRelational)
                {
                    property.RelatedProjectDataModelId = newProject.Models.FirstOrDefault(m => m.Name == property.RelatedProjectDataModelName)?.Id;
                    await _projectDataModelPropertyRepository.Update(property);
                }
            }

            return newProject;
        }

        public async Task DeleteProject(int id, bool sendNotification, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            Project project = null;
            List<ProjectMember> members = null;
            if (sendNotification)
            {
                var projectSpec = new ProjectFilterSpecification(id);
                project = await _projectRepository.GetSingleBySpec(projectSpec, cancellationToken);

                var memberSpec = new ProjectMemberFilterSpecification(id, 0, null, MemberRole.OwnerId);
                memberSpec.Includes.Add(m => m.User);
                members = (await _projectMemberRepository.GetBySpec(memberSpec, cancellationToken)).ToList();
            }

            await _projectRepository.Delete(id, cancellationToken);

            if (sendNotification && project != null && members != null)
            {
                await _notificationProvider.SendNotification(new SendNotificationRequest
                {
                    MessageType = NotificationConfig.ProjectDeleted,
                    Emails = members.Select(p => p.User.Email).ToList()
                }, new Dictionary<string, string>
                    {
                        {MessageParameter.ProjectName, project.Name}
                    });
            }
        }

        public async Task<string> ExportProject(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var projectFilter = new ProjectFilterSpecification(id);
            projectFilter.IncludeStrings.Add("Models.Properties.RelatedProjectDataModel");
            projectFilter.IncludeStrings.Add("Jobs.Tasks");
            var project = await _projectRepository.GetSingleBySpec(projectFilter, cancellationToken);

            if (project != null)
            {
                // set the relational property
                var propertiesWithRelational = project.Models?.SelectMany(m => m.Properties).Where(p => p.RelatedProjectDataModel != null).ToList();
                if (propertiesWithRelational != null)
                {
                    foreach (var property in propertiesWithRelational)
                    {
                        property.RelatedProjectDataModelName = property.RelatedProjectDataModel.Name;
                    }
                }

                if (project.Jobs != null)
                {
                    foreach (var job in project.Jobs)
                    {
                        job.Tasks = job.Tasks.OrderBy(t => t.Sequence).ToList();

                        foreach (var task in job.Tasks)
                        {
                            await _jobDefinitionService.DecryptSecretAdditionalConfigs(task, cancellationToken);
                        }
                    }
                }

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

        public async Task<List<(Project, ProjectMemberRole)>> GetProjectsByUser(int userId, string status = null, bool getAll = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            switch (status)
            {
                case null:
                case "":
                case ProjectStatusFilterType.All:
                    status = null;
                    break;
                case ProjectStatusFilterType.Active:
                case ProjectStatusFilterType.Archived:
                case ProjectStatusFilterType.Deleting:
                    // Leave status as is
                    break;
                default:
                    throw new FilterTypeNotFoundException(status);
            }

            var projectMembersByUserSpec = new ProjectMemberFilterSpecification(0, getAll ? 0 : userId, status);
            projectMembersByUserSpec.Includes.Add(p => p.ProjectMemberRole);
            var projectMembers = await _projectMemberRepository.GetBySpec(projectMembersByUserSpec, cancellationToken);
            var projects = projectMembers.GroupBy(m => m.ProjectId).Select(g => g.FirstOrDefault())
                .OrderBy(m => m.Project.Name)
                .Select(m => (m.Project, m.ProjectMemberRole)).ToList();

            return projects;
        }

        public async Task RestoreProject(int projectId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var project = await _projectRepository.GetById(projectId, cancellationToken);
            if (project == null)
                throw new ProjectNotFoundException(projectId);

            project.Status = ProjectStatusFilterType.Active;
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
                entity.DisplayName = project.DisplayName;
                await _projectRepository.Update(entity);
            }
        }

        public async Task MarkProjectDeleting(int id, string currentUrl, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var entity = await _projectRepository.GetById(id);
            if (entity != null)
            {
                entity.Status = ProjectStatusFilterType.Deleting;
                await _projectRepository.Update(entity);

                var deletionJob = await this._jobDefinitionService.GetDeletionJobDefinition(id);

                if (deletionJob != null)
                {
                    await _jobQueueService.AddJobQueue(id, currentUrl, JobType.Delete, deletionJob.Id, cancellationToken);
                }
                else
                {
                    // Marking project as deleting only make sense if we're trying to delete resources first
                    throw new DeletionJobDefinitionNotFound(id);
                }
            }
        }

        private string YamlSerialize(ProjectTemplate template)
        {
            var serializer = new SerializerBuilder().WithNamingConvention(new HyphenatedNamingConvention()).Build();
            return serializer.Serialize(template);
        }

        private string GetNormalizedProjectName(string projectName)
        {
            projectName = projectName.Trim();

            if (projectName.Contains(" "))
            {
                projectName = Regex.Replace(projectName, @"\s+", "-");
            }

            return projectName;
        }
    }
}
