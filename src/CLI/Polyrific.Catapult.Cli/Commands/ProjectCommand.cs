// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Commands.Project;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command(Description = "Project related command")]
    [Subcommand(typeof(ArchiveCommand))]
    [Subcommand(typeof(CreateCommand))]
    [Subcommand(typeof(GetCommand))]
    [Subcommand(typeof(ListCommand))]
    [Subcommand(typeof(RemoveCommand))]
    [Subcommand(typeof(RestoreCommand))]
    [Subcommand(typeof(CloneCommand))]
    [Subcommand(typeof(ExportCommand))]
    [Subcommand(typeof(UpdateCommand))]
    public class ProjectCommand : BaseCommand
    {
        public ProjectCommand(IHelpContextService helpContextService, IConsole console, ILogger<ProjectCommand> logger)
            : base(console, logger, helpContextService, HelpContextSection.Project)
        {
        }

        public override string Execute()
        {
            return string.Empty;
        }

        protected override int OnExecute(CommandLineApplication app)
        {
            base.OnExecute(app);

            if (!HelpContext)
            {
                app.ShowHelp();
            }

            return 0;
        }
    }
}
