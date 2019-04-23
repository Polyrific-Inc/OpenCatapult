// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Commands.Provider;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command(Description = "Task provider registration commands")]
    [Subcommand(typeof(ListCommand))]
    [Subcommand(typeof(GetCommand))]
    [Subcommand(typeof(RegisterCommand))]
    [Subcommand(typeof(RemoveCommand))]
    public class ProviderCommand : BaseCommand
    {
        public ProviderCommand(IHelpContextService helpContextService, IConsole console, ILogger<ProviderCommand> logger)
            : base(console, logger, helpContextService, HelpContextSection.TaskProvider)
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
