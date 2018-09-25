// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using McMaster.Extensions.CommandLineUtils;

namespace Polyrific.Catapult.Cli
{
    public class ConsoleReader : IConsoleReader
    {
        public string GetPassword(string prompt, ConsoleColor? promptColor = null, ConsoleColor? promptBgColor = null)
        {
            return Prompt.GetPassword(prompt, promptColor, promptBgColor);
        }
    }
}
