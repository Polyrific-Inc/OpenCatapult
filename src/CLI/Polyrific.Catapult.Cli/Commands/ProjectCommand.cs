// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Commands.Project;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command(Description = "Project related command")]
    [Subcommand("archive", typeof(ArchiveCommand))]
    [Subcommand("create", typeof(CreateCommand))]
    [Subcommand("list", typeof(ListCommand))]
    [Subcommand("remove", typeof(RemoveCommand))]
    [Subcommand("restore", typeof(RestoreCommand))]
    [Subcommand("clone", typeof(CloneCommand))]
    [Subcommand("export", typeof(ExportCommand))]
    [Subcommand("update", typeof(UpdateCommand))]
    public class ProjectCommand : BaseCommand
    {
        public ProjectCommand(IConsole console, ILogger<ProjectCommand> logger) : base(console, logger)
        {
        }

        public override string Execute()
        {
            return string.Empty;
        }

        protected override int OnExecute(CommandLineApplication app)
        {
            base.OnExecute(app);
            app.ShowHelp();
            return 0;
        }
    }
}