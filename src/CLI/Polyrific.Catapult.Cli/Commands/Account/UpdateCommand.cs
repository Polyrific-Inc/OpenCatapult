// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Dto.ManagedFile;
using Polyrific.Catapult.Shared.Dto.User;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Account
{
    [Command("update", Description = "Update user profile")]
    public class UpdateCommand : BaseCommand
    {
        private readonly IAccountService _accountService;

        public UpdateCommand(IConsole console, ILogger<UpdateCommand> logger, IAccountService accountService) : base(console, logger)
        {
            _accountService = accountService;
        }

        [Required]
        [Option("-u|--user <USER>", "Username (email) of the user", CommandOptionType.SingleValue)]
        public string User { get; set; }

        [Option("-fn|--firstname <FIRSTNAME>", "First name  of the user", CommandOptionType.SingleValue)]
        public string FirstName { get; set; }

        [Option("-ln|--lastname <LASTNAME>", "Last name of the user", CommandOptionType.SingleValue)]
        public string LastName { get; set; }

        [Option("-a|--avatar <AVATAR>", "The avatar image file path of the user", CommandOptionType.SingleValue)]
        [FileExists]
        public string Avatar { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to update user {User}...");

            string message;

            var user = _accountService.GetUserByEmail(User).Result;
            if (user != null)
            {
                var userId = int.Parse(user.Id);
                _accountService.UpdateUser(userId, new UpdateUserDto
                {
                    Id = userId,
                    FirstName = FirstName ?? user.FirstName,
                    LastName = LastName ?? user.LastName,
                    AvatarFile = !string.IsNullOrEmpty(Avatar) ? new ManagedFileDto
                    {
                        Id = user.AvatarFile?.Id ?? 0,
                        FileName = Path.GetFileName(Avatar),
                        File = File.ReadAllBytes(Avatar)
                    } : null
                }).Wait();

                message = $"User {User} has been updated";
                Logger.LogInformation(message);
            }
            else
            {
                message = $"User {User} was not found";
            }

            return message;
        }
    }
}
