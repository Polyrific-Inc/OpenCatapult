// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Globalization;
using McMaster.Extensions.CommandLineUtils.Abstractions;

namespace Polyrific.Catapult.Engine.Utility
{
    public class CatapultOptionParser : IValueParser<(string, string)>
    {
        public const string Separator = ":";

        public Type TargetType => typeof((string, string));

        public (string, string) Parse(string argName, string value, CultureInfo culture)
        {
            if (value == null)
                return default;

            var separatorIndex = value.IndexOf(Separator, StringComparison.Ordinal);
            if (separatorIndex < 0)
            {
                throw new FormatException($"Invalid value specified for {argName}. The string should be two key and value separated by '{Separator}'");
            }

            var optionKey = value.Substring(0, separatorIndex);
            var optionValue = value.Substring(separatorIndex + 1);
            return (optionKey, optionValue);
        }

        object IValueParser.Parse(string argName, string value, CultureInfo culture) => Parse(argName, value, culture);
    }
}
