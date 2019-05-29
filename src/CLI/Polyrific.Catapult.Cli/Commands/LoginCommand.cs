// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.Constants;
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

            var password = _consoleReader.GetPassword("Enter password:");
            var token = _tokenService.RequestToken(new RequestTokenDto
            {
                UserName = Username,
                Password = password
            }).Result;

            if (token == TokenResponses.RequiresTwoFactor)
            {
                token = LoginWith2fa(password);
            }

            if (!string.IsNullOrEmpty(token))
            {
                _tokenStore.SaveToken(token).Wait();

                return $"Logged in as {Username}";
            }

            return null;
        }

        private string LoginWith2fa(string password)
        {
            string token = null;
            bool retry = false;
            bool loginUsingRecovery = false;
            string input;
            do
            {
                input = Console.GetString("Your login is protected with an authenticator app. Enter your authenticator code:");

                if (!string.IsNullOrEmpty(input))
                {
                    try
                    {
                        token = _tokenService.RequestToken(new RequestTokenDto
                        {
                            UserName = Username,
                            Password = password,
                            AuthenticatorCode = input
                        }).Result;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("The authenticator code is invalid. You can:");
                        Console.WriteLine("1. Try entering authenticator code again");
                        Console.WriteLine("2. Login using recovery code");

                        var nextStep = Console.GetString("Please enter the number for the action you would like to choose:");
                        retry = nextStep == "1";
                        loginUsingRecovery = nextStep == "2";
                    }
                }
            } while ((retry || string.IsNullOrEmpty(input)) && !loginUsingRecovery && string.IsNullOrEmpty(token));
            
            if (loginUsingRecovery)
            {
                token = LoginWithRecoveryCode(password);
            }

            return token;
        }

        private string LoginWithRecoveryCode(string password)
        {
            string token = null;
            bool retry = false;
            string input;
            do
            {
                input = Console.GetString("You have requested to log in with a recovery code. Enter the recovery code:");

                if (!string.IsNullOrEmpty(input))
                {
                    try
                    {
                        token = _tokenService.RequestToken(new RequestTokenDto
                        {
                            UserName = Username,
                            Password = password,
                            RecoveryCode = input
                        }).Result;
                    }
                    catch (Exception)
                    {
                        retry = Console.GetYesNo("The recovery code is invalid. Retry?", false);
                    }
                }
            } while ((retry || string.IsNullOrEmpty(input)) && string.IsNullOrEmpty(token));

            return token;
        }
    }
}
