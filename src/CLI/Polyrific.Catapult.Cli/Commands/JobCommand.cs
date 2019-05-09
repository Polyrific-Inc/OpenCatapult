// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Commands.Job;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command(Description = "Job definition related command")]
    [Subcommand(typeof(AddCommand))]
    [Subcommand(typeof(SetDefaultCommand))]
    [Subcommand(typeof(GetCommand))]
    [Subcommand(typeof(ListCommand))]
    [Subcommand(typeof(RemoveCommand))]
    [Subcommand(typeof(UpdateCommand))]
    public class JobCommand : BaseCommand
    {
        public JobCommand(IHelpContextService helpContextService, IConsole console, ILogger<JobCommand> logger) 
            : base(console, logger, helpContextService, HelpContextSection.JobDefinition)
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
