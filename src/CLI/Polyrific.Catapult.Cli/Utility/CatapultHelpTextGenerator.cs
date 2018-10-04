// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.IO;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.HelpText;
using Polyrific.Catapult.Cli.Commands;

namespace Polyrific.Catapult.Cli
{
    public class CatapultHelpTextGenerator : DefaultHelpTextGenerator
    {
        protected override void GenerateFooter(CommandLineApplication application, TextWriter output)
        {
            base.GenerateFooter(application, output);
            
            var customFooter = InvokeGetHelpFooter(application);
            if (!string.IsNullOrEmpty(customFooter))
                output.WriteLine(customFooter);
        }

        private string InvokeGetHelpFooter(CommandLineApplication application)
        {
            var commandInstance = application.GetType().GetProperty("Model").GetValue(application, null);

            if (commandInstance is BaseCommand command)
                return command.GetHelpFooter();

            return null;
        }
    }
}
