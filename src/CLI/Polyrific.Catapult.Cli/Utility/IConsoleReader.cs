// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Cli
{
    public interface IConsoleReader
    {
        string GetPassword(string prompt, ConsoleColor? promptColor = null, ConsoleColor? promptBgColor = null);
    }
}
