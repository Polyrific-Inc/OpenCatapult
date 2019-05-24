// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Commands.Setting;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command(Description = "Setting related commands")]
    [Subcommand(typeof(ListCommand))]
    [Subcommand(typeof(UpdateCommand))]
    public class SettingCommand : BaseCommand
    {
        public SettingCommand(IConsole console, ILogger<SettingCommand> logger)
            : base(console, logger)
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
