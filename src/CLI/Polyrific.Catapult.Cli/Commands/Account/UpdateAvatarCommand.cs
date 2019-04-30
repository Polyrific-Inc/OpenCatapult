// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Account
{
    [Command("updateavatar", Description = "Update user avatar")]
    public class UpdateAvatarCommand : BaseCommand
    {
        private readonly IAccountService _accountService;
        private readonly IManagedFileService _managedFileService;

        public UpdateAvatarCommand(IConsole console, ILogger<UpdateAvatarCommand> logger, IAccountService accountService, IManagedFileService managedFileService) : base(console, logger)
        {
            _accountService = accountService;
            _managedFileService = managedFileService;
        }

        [Required]
        [Option("-u|--user <USER>", "Username of the user", CommandOptionType.SingleValue)]
        public string User { get; set; }

        [Option("-a|--avatar <AVATAR>", "The avatar image file path of the user", CommandOptionType.SingleValue)]
        [FileExists]
        [Required]
        public string Avatar { get; set; }

        public override string Execute()
        {
            Console.WriteLine($"Trying to update user {User} avatar...");

            string message;

            var user = _accountService.GetUserByUserName(User).Result;
            if (user != null)
            {
                var userId = int.Parse(user.Id);

                var fileName = Path.GetFileName(Avatar);
                var file = File.ReadAllBytes(Avatar);

                var avatarFileId = user.AvatarFileId;
                if (avatarFileId > 0)
                {
                    _managedFileService.UpdateManagedFile(user.AvatarFileId.Value, fileName, file).Wait();
                }
                else
                {
                    avatarFileId = _managedFileService.CreateManagedFile(fileName, file).Result.Id;
                }

                _accountService.UpdateAvatar(userId, avatarFileId).Wait();

                message = $"User {User} avatar has been updated";
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
