// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.IO;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.HelpText;

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
            const BindingFlags binding = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            var commandInstance = application.GetType().GetProperty("Model").GetValue(application, null);
            var commandType = commandInstance.GetType();
            var method = commandType.GetMethod("GetHelpFooter", binding);

            return method?.Invoke(commandInstance, null) as string;
        }
    }
}
