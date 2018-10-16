// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Dto.User;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands
{
    [Command(Description = "Login as a user")]
    public class LoginCommand : BaseCommand
    {
        private readonly ITokenService _tokenService;
        private readonly ITokenStore _tokenStore;
        private readonly IConsoleReader _consoleReader;

        public LoginCommand(IConsole console, ILogger<LoginCommand> logger, ITokenService tokenService, ITokenStore tokenStore, IConsoleReader consoleReader) : base(console, logger)
        {
            _tokenService = tokenService;
            _tokenStore = tokenStore;
            _consoleReader = consoleReader;
        }

        [Required]
        [Option("-u|--user <USER>", "Username", CommandOptionType.SingleValue)]
        public string Username { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to login as {Username}...");

            var token = _tokenService.RequestToken(new RequestTokenDto
            {
                Email = Username,
                Password = _consoleReader.GetPassword("Enter password:")
            }).Result;

            _tokenStore.SaveToken(token).Wait();

            return $"Logged in as {Username}";
        }
    }
}
