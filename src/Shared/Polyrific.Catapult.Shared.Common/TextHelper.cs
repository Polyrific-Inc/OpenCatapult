// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Get prefix tags in a text. E.g. "[TAG1][TAG2][TAG3] Message"
        /// </summary>
        /// <param name="input">The input text</param>
        /// <param name="startTag">Start tag character (default value is "[")</param>
        /// <param name="endTag">End tag character (default value is "]")</param>
        /// <returns></returns>
        public static string[] GetPrefixTags(this string input, string startTag = "[", string endTag = "]")
        {
            var tags = new List<string>();
            var text = input;
            do
            {
                var startTagIndex = text.IndexOf(startTag, StringComparison.Ordinal);
                var endTagIndex = text.IndexOf(endTag, StringComparison.Ordinal);
                if (startTagIndex >= 0 && endTagIndex > startTagIndex)
                {
                    tags.Add(text.Substring(startTagIndex + 1, endTagIndex - startTagIndex - 1));

                    text = text.Substring(endTagIndex + 1);
                }
                else
                {
                    return tags.ToArray();
                }

            } while (true);
        }
    }
}
