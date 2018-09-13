// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Dto.CatapultEngine;
using Polyrific.Catapult.Shared.Service;
using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Cli.Commands.Engine
{

    [Command(Description = "Generate a token for the engine")]
    public class TokenCommand : BaseCommand
    {
        private readonly ITokenService _tokenService;
        private readonly ICatapultEngineService _catapultEngineService;

        public TokenCommand(IConsole console, ILogger<TokenCommand> logger, ITokenService tokenService, ICatapultEngineService catapultEngineService) : base(console, logger)
        {
            _tokenService = tokenService;
            _catapultEngineService = catapultEngineService;
        }

        [Required]
        [Option("-n|--name <NAME>", "Name of the engine", CommandOptionType.SingleValue)]
        public string Name { get; set; }
        
        public override string Execute()
        {
            string message = string.Empty;

            var engine = _catapultEngineService.GetCatapultEngineByName(Name).Result;

            if (engine != null)
            {

                var token = _tokenService.RequestEngineToken(engine.Id, new RequestEngineTokenDto()).Result;
                message = $"Token: {token}";
            }
            else
            {
                message = $"Engine {Name} is not found";
            }

            return message;
        }
    }
}
