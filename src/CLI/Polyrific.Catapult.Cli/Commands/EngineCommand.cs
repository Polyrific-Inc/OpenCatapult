﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Commands.Engine;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command(Description = "Catapult Engine registration commands")]
    [Subcommand(typeof(ListCommand))]
    [Subcommand(typeof(GetCommand))]
    [Subcommand(typeof(RegisterCommand))]
    [Subcommand(typeof(TokenCommand))]
    [Subcommand(typeof(SuspendCommand))]
    [Subcommand(typeof(ActivateCommand))]
    [Subcommand(typeof(RemoveCommand))]
    public class EngineCommand : BaseCommand
    {
        private readonly IHelpContextService _helpContextService;

        [Option("-c|--helpcontext", "Show help context", CommandOptionType.NoValue)]
        public bool HelpContext { get; set; }

        public EngineCommand(IHelpContextService helpContextService, IConsole console, ILogger<EngineCommand> logger) : base(console, logger)
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
                var helpContexts = _helpContextService.GetHelpContextsBySection(HelpContextSection.Engine).Result;
                return helpContexts.ToHelpContextString("Help context for the engine commands:");
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
