// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Linq;

namespace Polyrific.Catapult.TaskProviders.GenericCommand.CommandProviders
{
    public static class CommandTextHelper
    {
        public static string[] SplitOnNewLine(this string input, string[] splitters = null, bool removeEmptyEntries = true)
        {
            if (splitters == null)
                splitters = new string[] { "\r\n", "\r", "\n" };

            return input.Split(splitters, removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None)
                .Select(s => s.Trim()).ToArray();
        }
    }
}
