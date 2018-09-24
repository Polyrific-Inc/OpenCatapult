﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto;
using Polyrific.Catapult.Shared.Dto.JobDefinition;
using Polyrific.Catapult.Shared.Dto.Project;
using Polyrific.Catapult.Shared.Dto.ProjectDataModel;
using Polyrific.Catapult.Shared.Service;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Polyrific.Catapult.Cli.Commands.Project
{
    [Command(Description = "Create a project")]
    public class CreateCommand : BaseCommand
    {
        private readonly IProjectService _projectService;
        private readonly IPluginService _pluginService;
        private readonly IExternalServiceService _externalServiceService;
        private readonly ITemplateWriter _templateWriter;

        public CreateCommand(IConsole console, ILogger<CreateCommand> logger, IProjectService projectService, IPluginService pluginService, IExternalServiceService externalServiceService, ITemplateWriter templateWriter) : base(console, logger)
        {
            _projectService = projectService;
            _pluginService = pluginService;
            _externalServiceService = externalServiceService;
            _templateWriter = templateWriter;
        }

        [Required]
        [Option("-n|--name <NAME>", "Name of the project", CommandOptionType.SingleValue)]
        public string Name { get; set; }

        [Option("-c|--client <CLIENT>", "Name of the client", CommandOptionType.SingleValue)]
        public string Client { get; set; }

        [Option("-t|--template <TEMPLATE>", "Project template", CommandOptionType.SingleValue)]
        public string Template { get; set; }

        [Option("-prop|--property <KEY>:<PROPERTY>", "Property of the project", CommandOptionType.MultipleValue)]
        public (string, string)[] Property { get; set; }

        public override string Execute()
        {
            string message = string.Empty;
            NewProjectDto projectDto;
            if (!string.IsNullOrEmpty(Template))
            {
                var projectTemplate = _templateWriter.Read(Template);
                projectDto = DeserializeYaml<NewProjectDto>(projectTemplate);
                projectDto.Name = Name;
                projectDto.Client = Client;
            }
            else
            {
                projectDto = new NewProjectDto
                {
                    Name = Name,
                    Client = Client,
                    Members = new List<NewProjectMemberDto>()
                };
            }

            projectDto.Members = projectDto.Members ?? new List<NewProjectMemberDto>();
            projectDto.Models = projectDto.Models ?? new List<CreateProjectDataModelWithPropertiesDto>();
            projectDto.Jobs = projectDto.Jobs ?? new List<CreateJobDefinitionWithTasksDto>();
            projectDto.Config = Property?.ToDictionary(x => x.Item1, x => x.Item2);

            message = ValidateTask(projectDto.Jobs);
            if (!string.IsNullOrEmpty(message))
                return message;

            var project = _projectService.CreateProject(projectDto).Result;

            message = project.ToCliString($"Project created:");
            Logger.LogInformation(message);
            
            return message;
        }

        private T DeserializeYaml<T>(string templateYaml)
        {
            var deserializer = new DeserializerBuilder().WithNamingConvention(new HyphenatedNamingConvention()).IgnoreUnmatchedProperties().Build();
            return deserializer.Deserialize<T>(templateYaml);
        }

        private string ValidateTask(List<CreateJobDefinitionWithTasksDto> jobs)
        {
            string message = string.Empty;

            var tasks = jobs.SelectMany(j => j.Tasks);
            foreach (var task in tasks)
            {
                var plugin = _pluginService.GetPluginByName(task.Provider).Result;

                if (plugin == null)
                {
                    message = $"The provider \"{task.Provider}\" is not installed";
                    return message;
                }

                if (plugin.RequiredServices != null && plugin.RequiredServices.Length > 0)
                {
                    foreach (var service in plugin.RequiredServices)
                    {
                        var externalServiceKey = $"{service}ExternalService";
                        var externalServiceName = task.Config.GetValueOrDefault(externalServiceKey);

                        if (string.IsNullOrEmpty(externalServiceName))
                        {
                            message = $"The {service} external service is required for the provider {task.Provider}. Please check the template file";
                            return message;
                        }

                        var externalService = _externalServiceService.GetExternalServiceByName(externalServiceName).Result;

                        if (externalService == null)
                        {
                            message = $"The external service {externalServiceName} is not found. Please add them using \"service add\" command";
                            return message;
                        }

                        if (externalService.ExternalServiceTypeName != service)
                        {
                            message = $"The external service {externalServiceName} is not a {service} service";
                            return message;
                        }
                    }
                }
            }

            return message;
        }
    }
}
