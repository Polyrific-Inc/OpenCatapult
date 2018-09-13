// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Newtonsoft.Json;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Api.Core.Specifications;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Core.Services
{
    public class JobDefinitionService : IJobDefinitionService
    {
        private readonly IJobDefinitionRepository _jobDefinitionRepository;
        private readonly IJobTaskDefinitionRepository _jobTaskDefinitionRepository;
        private readonly IProjectRepository _projectRepository;

        public JobDefinitionService(IJobDefinitionRepository dataModelRepository,
            IJobTaskDefinitionRepository jobTaskDefinitionRepository,
            IProjectRepository projectRepository)
        {
            _jobDefinitionRepository = dataModelRepository;
            _jobTaskDefinitionRepository = jobTaskDefinitionRepository;
            _projectRepository = projectRepository;
        }

        public async Task<int> AddJobDefinition(int projectId, string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var project = await _projectRepository.GetById(projectId, cancellationToken);
            if (project == null)
            {
                throw new ProjectNotFoundException(projectId);
            }

            var projectJobDefinitionPropertyByProjectSpec = new JobDefinitionFilterSpecification(name, projectId);
            if (await _jobDefinitionRepository.CountBySpec(projectJobDefinitionPropertyByProjectSpec, cancellationToken) > 0)
            {
                throw new DuplicateJobDefinitionException(name);
            }


            var newJobDefinition = new JobDefinition { ProjectId = projectId, Name = name };
            return await _jobDefinitionRepository.Create(newJobDefinition, cancellationToken);
        }

        public async Task DeleteJobDefinition(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var taskByJobSpec = new JobTaskDefinitionFilterSpecification(id);
            var tasks = await _jobTaskDefinitionRepository.GetBySpec(taskByJobSpec, cancellationToken);
            foreach (var task in tasks.ToList())
            {
                await DeleteJobTaskDefinition(task.Id, cancellationToken);
            }

            await _jobDefinitionRepository.Delete(id, cancellationToken);
        }

        public async Task<JobDefinition> GetJobDefinitionById(int modelId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _jobDefinitionRepository.GetById(modelId, cancellationToken);
        }

        public async Task<JobDefinition> GetJobDefinitionByName(int projectId, string jobDefinitionName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var projectMemberByProjectSpec = new JobDefinitionFilterSpecification(jobDefinitionName, projectId);
            return await _jobDefinitionRepository.GetSingleBySpec(projectMemberByProjectSpec, cancellationToken);
        }

        public async Task<List<JobDefinition>> GetJobDefinitions(int projectId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var projectMemberByProjectSpec = new JobDefinitionFilterSpecification(projectId);
            var projectMembers = await _jobDefinitionRepository.GetBySpec(projectMemberByProjectSpec, cancellationToken);

            return projectMembers.ToList();
        }

        public async Task RenameJobDefinition(int id, string newName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var dataModel = await _jobDefinitionRepository.GetById(id, cancellationToken);

            if (dataModel != null)
            {
                var dataModelByNameSpec = new JobDefinitionFilterSpecification(newName, dataModel.ProjectId, id);
                if (await _jobDefinitionRepository.CountBySpec(dataModelByNameSpec, cancellationToken) > 0)
                {
                    throw new DuplicateJobDefinitionException(newName);
                }

                dataModel.Name = newName;
                await _jobDefinitionRepository.Update(dataModel, cancellationToken);
            }
        }
        
        public async Task<int> AddJobTaskDefinition(JobTaskDefinition jobTaskDefinition, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var jobDefinition = await _jobDefinitionRepository.GetById(jobTaskDefinition.JobDefinitionId, cancellationToken);
            if (jobDefinition == null)
            {
                throw new JobDefinitionNotFoundException(jobTaskDefinition.JobDefinitionId);
            }

            return await _jobTaskDefinitionRepository.Create(jobTaskDefinition, cancellationToken);
        }

        public async Task<List<int>> AddJobTaskDefinitions(int jobDefinitionId, List<JobTaskDefinition> jobTaskDefinitions, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var jobDefinition = await _jobDefinitionRepository.GetById(jobDefinitionId, cancellationToken);
            if (jobDefinition == null)
            {
                throw new JobDefinitionNotFoundException(jobDefinitionId);
            }
            
            jobTaskDefinitions.ForEach(j => j.JobDefinitionId = jobDefinitionId);

            return await _jobTaskDefinitionRepository.CreateRange(jobTaskDefinitions, cancellationToken);
        }

        public async Task UpdateJobTaskDefinition(JobTaskDefinition editedJobTaskDefinition, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var jobTaskDefinition = await _jobTaskDefinitionRepository.GetById(editedJobTaskDefinition.Id, cancellationToken);

            if (jobTaskDefinition != null)
            {
                jobTaskDefinition.Type = editedJobTaskDefinition.Type;
                jobTaskDefinition.ConfigString = editedJobTaskDefinition.ConfigString;
                jobTaskDefinition.ContinueWhenError = editedJobTaskDefinition.ContinueWhenError;
                jobTaskDefinition.Sequence = editedJobTaskDefinition.Sequence;
                await _jobTaskDefinitionRepository.Update(jobTaskDefinition, cancellationToken);
            }
        }

        public async Task UpdateJobTaskConfig(int taskDefinitionId, Dictionary<string, string> jobTaskConfig, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var jobTaskDefinition = await _jobTaskDefinitionRepository.GetById(taskDefinitionId, cancellationToken);

            if (jobTaskDefinition != null)
            {
                jobTaskDefinition.ConfigString = JsonConvert.SerializeObject(jobTaskConfig);
                
                await _jobTaskDefinitionRepository.Update(jobTaskDefinition, cancellationToken);
            }
        }

        public async Task DeleteJobTaskDefinition(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _jobTaskDefinitionRepository.Delete(id, cancellationToken);
        }

        public async Task<List<JobTaskDefinition>> GetJobTaskDefinitions(int jobDefinitionId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var taskByJobSpec = new JobTaskDefinitionFilterSpecification(jobDefinitionId);
            var tasks = await _jobTaskDefinitionRepository.GetBySpec(taskByJobSpec, cancellationToken);

            return tasks.ToList();
        }

        public async Task<JobTaskDefinition> GetJobTaskDefinitionById(int jobTaskDefinitionId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _jobTaskDefinitionRepository.GetById(jobTaskDefinitionId, cancellationToken);
        }

        public async Task<JobTaskDefinition> GetJobTaskDefinitionByName(int jobDefinitionId, string jobTaskDefinitionName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _jobTaskDefinitionRepository.GetSingleBySpec(new JobTaskDefinitionFilterSpecification(jobDefinitionId, jobTaskDefinitionName), cancellationToken);
        }
    }
}