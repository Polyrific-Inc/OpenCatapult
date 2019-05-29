// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.User;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Account.TwoFactor
{
    [Command("enable", Description = "Enable 2fa command")]
    public class EnableCommand : BaseCommand
    {
        private readonly IAccountService _accountService;

        public EnableCommand(IConsole console, ILogger<EnableCommand> logger, IAccountService accountService) : base(console, logger)
        {
            _accountService = accountService;
        }

        public override string Execute()
        {
            Console.WriteLine($"Trying to enable 2fa for current user...");

            string message = "";
            var authKey = _accountService.GetTwoFactorAuthKey().Result;
            var user2faInfo = _accountService.GetUser2faInfo().Result;

            Console.WriteLine($"Please enter the following key \"{authKey.SharedKey}\" into your two factor authenticator app. Spaces and casing do not matter.");
            List<string> recoveryCodes = null;
            bool retry = false;
            bool success = false;
            string input;
            do
            {
                input = Console.GetString("Once you have input the key above, your two factor authentication app will provide you with a unique code. Please enter the code:");

                if (!string.IsNullOrEmpty(input))
                {
                    try
                    {
                        _accountService.VerifyTwoFactorAuthenticatorCode(new VerifyTwoFactorCodeDto
                        {
                            VerificationCode = input
                        }).Wait();

                        success = true;

                        if (user2faInfo.RecoveryCodesLeft == 0)
                        {
                            recoveryCodes = this._accountService.Generate2faRecoveryCodes().Result.RecoveryCodes?.ToList();
                        }
                    }
                    catch (Exception)
                    {
                        retry = Console.GetYesNo("The recovery code is invalid. Retry?", false);
                    }
                }
            } while (retry || string.IsNullOrEmpty(input));

            if (success)
            {
                var sb = new StringBuilder("The 2fa has been enabled for the current user");

                if (recoveryCodes != null)
                {
                    sb.AppendLine("Put these codes in a safe place. If you lose your device and don't have the recovery codes you will lose access to your account");
                    foreach (var recoveryCode in recoveryCodes)
                    {
                        sb.AppendLine(recoveryCode);
                    }
                }

                message = sb.ToString();

                Logger.LogInformation("The 2fa has been enabled for the current user");
            }

            return message;
        }
    }
}
