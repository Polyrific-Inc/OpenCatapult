// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Text.RegularExpressions;

namespace Polyrific.Catapult.Shared.Common
{
    public static class TextHelper
    {
        public static string SplitTextOnCapitalLetters(string text)
        {
            var r = new Regex(@"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

            return r.Replace(text, " ");
        }
    }
}
