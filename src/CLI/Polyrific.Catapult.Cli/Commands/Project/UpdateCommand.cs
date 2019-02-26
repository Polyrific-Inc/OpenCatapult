// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Dto.Project;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Project
{
    [Command("update", Description = "Update a project")]
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

        [Option("-d|--displayname <DISPLAYNAME>", "Display Name of the project", CommandOptionType.SingleValue)]
        public string DisplayName { get; set; }

        [Option("-r|--rename <RENAME>", "New Name of the project", CommandOptionType.SingleValue)]
        public string Rename { get; set; }

        [Option("-c|--client <CLIENT>", "Name of the client", CommandOptionType.SingleValue)]
        public string Client { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to update project {Name}...");

            string message;
            
            var project = _projectService.GetProjectByName(Name).Result;
            if (project != null)
            {
                var dto = new UpdateProjectDto
                {
                    Id = project.Id,
                    Name = Rename ?? project.Name,
                    DisplayName = DisplayName ?? project.DisplayName,
                    Client = Client ?? project.Client
                };

                _projectService.UpdateProject(project.Id, dto).Wait();

                message = $"Project {Name} has been updated successfully";
                Logger.LogInformation(message);
            }
            else
            {
                message = $"Project {Name} was not found";
            }

            return message;
        }
    }
}
