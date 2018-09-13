// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Dto.Project;
using Polyrific.Catapult.Shared.Service;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Polyrific.Catapult.Cli.Commands.Project
{
    [Command(Description = "Update a project")]
    public class UpdateCommand : BaseCommand
    {
        private readonly IProjectService _projectService;

        public UpdateCommand(IConsole console, ILogger<UpdateCommand> logger, IProjectService projectService) : base(console, logger)
        {
            _projectService = projectService;
        }

        [Required]
        [Option("-n|--name <NAME>", "Name of the project", CommandOptionType.SingleValue)]
        public string Name { get; set; }

        [Option("-r|--rename <RENAME>", "New Name of the project", CommandOptionType.SingleValue)]
        public string Rename { get; set; }

        [Option("-c|--client <CLIENT>", "Name of the client", CommandOptionType.SingleValue)]
        public string Client { get; set; }

        [Option("-prop|--property <KEY>:<PROPERTY>", "Property of the project", CommandOptionType.MultipleValue)]
        public (string, string)[] Property { get; set; }

        public override string Execute()
        {
            string message = string.Empty;
            
            var project = _projectService.GetProjectByName(Name).Result;

            if (project != null)
            {
                var dto = new UpdateProjectDto
                {
                    Id = project.Id,
                    Name = Rename ?? project.Name,
                    Client = Client ?? project.Client,
                    Config = Property?.ToDictionary(x => x.Item1, x => x.Item2)
                };

                _projectService.UpdateProject(project.Id, dto).Wait();

                message = $"Project {Name} was updated";
                Logger.LogInformation(message);
            }
            else
            {
                message = $"Project {Name} is not found";
            }

            return message;
        }
    }
}