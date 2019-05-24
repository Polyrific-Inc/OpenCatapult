// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Cli.Extensions;
using Polyrific.Catapult.Shared.Dto.ApplicationSetting;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Cli.Commands.Setting
{
    [Command("update", Description = "Update an application setting")]
    public class UpdateCommand : BaseCommand
    {
        private readonly IConsoleReader _consoleReader;
        private readonly IApplicationSettingService _applicationSettingService;

        public UpdateCommand(IConsole console, ILogger<UpdateCommand> logger, IConsoleReader consoleReader, IApplicationSettingService applicationSettingService) : base(console, logger)
        {
            _consoleReader = consoleReader;
            _applicationSettingService = applicationSettingService;
        }

        public override string Execute()
        {
            Console.WriteLine($"Trying to update application settings...");

            string message;

            var settings = _applicationSettingService.GetApplicationSettings().Result;

            Console.WriteLine("Please enter the updated application settings (leave blank if it's unchanged):");
            var dto = new UpdateApplicationSettingDto
            {
                UpdatedSettings = new Dictionary<string, string>()
            };

            foreach (var setting in settings)
            {
                string input = null;

                string prompt = $"{setting.Label}:";

                bool validInput;
                do
                {
                    if (setting.DataType == ApplicationSettingDataTypes.Bool)
                        input = Console.GetYesNoNullable(prompt)?.ToString().ToLower();
                    else
                        input = Console.GetString(prompt);

                    if (setting.AllowedValues != null && setting.AllowedValues.Length > 0 && !string.IsNullOrEmpty(input) && !setting.AllowedValues.Contains(input))
                    {
                        Console.WriteLine($"Input is not valid. Please enter the allowed values: {string.Join(',', setting.AllowedValues)}");
                        validInput = false;
                    }
                    else
                    {
                        validInput = true;
                    }

                } while (!validInput);

                if (!string.IsNullOrEmpty(input))
                    dto.UpdatedSettings.Add(setting.Key, input);
            }

            _applicationSettingService.UpdateApplicationSetting(dto).Wait();
            message = $"Application settings has been updated successfully";
            Logger.LogInformation(message);

            return message;
        }
    }
}
