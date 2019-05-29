// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Text;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Account.TwoFactor
{
    [Command("resetrecovery", Description = "Reset 2fa recovery code command")]
    public class ResetRecoveryCommand : BaseCommand
    {
        private readonly IAccountService _accountService;

        [Option("-ac|--autoconfirm", "Execute the command without the need of confirmation prompt", CommandOptionType.NoValue)]
        public bool AutoConfirm { get; set; }

        public ResetRecoveryCommand(IConsole console, ILogger<ResetRecoveryCommand> logger, IAccountService accountService) : base(console, logger)
        {
            _accountService = accountService;
        }

        public override string Execute()
        {
            if (!(AutoConfirm || Console.GetYesNo($"Are you sure you want to reset the two factor recovery code?", false)))
                return string.Empty;

            Console.WriteLine($"Trying to reset 2FA recovery code for current user...");

            string message;
            var dto = _accountService.Generate2faRecoveryCodes().Result;

            var sb = new StringBuilder("2FA recovery code has been reset. Put these codes in a safe place. If you lose your device and don't have the recovery codes you will lose access to your account");
            sb.AppendLine("Following are the new recovery codes:");
            foreach (var recoveryCode in dto.RecoveryCodes)
            {
                sb.AppendLine(recoveryCode);
            }

            message = sb.ToString();

            Logger.LogInformation("2FA recovery code has been reset");

            return message;
        }
    }
}
