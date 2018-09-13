// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Dto.User;
using Polyrific.Catapult.Shared.Service;
using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command(Description = "Login as a user")]
    public class LoginCommand : BaseCommand
    {
        private readonly ITokenService _tokenService;
        private readonly ITokenStore _tokenStore;

        public LoginCommand(IConsole console, ILogger<LoginCommand> logger, ITokenService tokenService, ITokenStore tokenStore) : base(console, logger)
        {
            _tokenService = tokenService;
            _tokenStore = tokenStore;
        }

        [Required]
        [Option("-u|--user <USER>", "Username", CommandOptionType.SingleValue)]
        public string Username { get; set; }

        [Required]
        [Option("-p|--password <PASSWORD>", "Passowrd", CommandOptionType.SingleValue)]
        public string Password { get; set; }

        public override string Execute()
        {
            var token = _tokenService.RequestToken(new RequestTokenDto
            {
                Email = Username,
                Password = Password
            }).Result;

            _tokenStore.SaveToken(token);

            return $"Logged in as {Username}";
        }
    }
}