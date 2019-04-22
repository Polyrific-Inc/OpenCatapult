﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Commands.Account;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command(Description = "User account related command")]
    [Subcommand(typeof(ListCommand))]
    [Subcommand(typeof(ActivateCommand))]
    [Subcommand(typeof(RegisterCommand))]
    [Subcommand(typeof(RemoveCommand))]
    [Subcommand(typeof(SuspendCommand))]
    [Subcommand(typeof(UpdateCommand))]
    [Subcommand(typeof(UpdateAvatarCommand))]
    [Subcommand(typeof(PasswordCommand))]
    [Subcommand(typeof(SetRoleCommand))]
    public class AccountCommand : BaseCommand
    {
        private readonly IHelpContextService _helpContextService;

        [Option("-c|--helpcontext", "Show help context", CommandOptionType.NoValue)]
        public bool HelpContext { get; set; }

        public AccountCommand(IHelpContextService helpContextService, IConsole console, ILogger<AccountCommand> logger) : base(console, logger)
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
                var helpContexts = _helpContextService.GetHelpContextsBySection(HelpContextSection.User).Result;
                return helpContexts.ToHelpContextString("Help context for the account commands:");
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
