// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Commands.Service;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command(Description = "Job task definition related command")]
    [Subcommand(typeof(AddCommand))]
    [Subcommand(typeof(GetCommand))]
    [Subcommand(typeof(ListCommand))]
    [Subcommand(typeof(UpdateCommand))]
    [Subcommand(typeof(RemoveCommand))]
    public class TaskCommand : BaseCommand
    {
        private readonly IHelpContextService _helpContextService;

        [Option("-c|--helpcontext", "Show help context", CommandOptionType.NoValue)]
        public bool HelpContext { get; set; }

        public TaskCommand(IHelpContextService helpContextService, IConsole console, ILogger<TaskCommand> logger) : base(console, logger)
        {
            _helpContextService = helpContextService;
        }

        public override string Execute()
        {
            if (!HelpContext)
            {
                return string.Empty;
            }
            else
            {
                var helpContexts = _helpContextService.GetHelpContextsBySection(HelpContextSection.JobTaskDefinition).Result;
                return helpContexts.ToHelpContextString("Help context for the job task commands:");
            }
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
