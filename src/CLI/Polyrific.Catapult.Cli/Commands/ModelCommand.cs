// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Commands.Model;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command(Description = "Project data model related command")]
    [Subcommand(typeof(AddCommand))]
    [Subcommand(typeof(GetCommand))]
    [Subcommand(typeof(ListCommand))]
    [Subcommand(typeof(RemoveCommand))]
    [Subcommand(typeof(UpdateCommand))]
    public class ModelCommand : BaseCommand
    {
        private readonly IHelpContextService _helpContextService;

        [Option("-c|--helpcontext", "Show help context", CommandOptionType.NoValue)]
        public bool HelpContext { get; set; }

        public ModelCommand(IHelpContextService helpContextService, IConsole console, ILogger<ModelCommand> logger) : base(console, logger)
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
                var helpContexts = _helpContextService.GetHelpContextsBySection(HelpContextSection.ProjectModel).Result;
                return helpContexts.ToHelpContextString("Help context for the project model commands:");
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
