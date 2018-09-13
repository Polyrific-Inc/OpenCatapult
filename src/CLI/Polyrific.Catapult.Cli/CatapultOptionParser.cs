// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils.Abstractions;
using System;
using System.Globalization;

namespace Polyrific.Catapult.Cli
{
    public class CatapultOptionParser : IValueParser<(string, string)>
    {
        public const string Separator = ":";

        public Type TargetType => typeof((string, string));

        public (string, string) Parse(string argName, string value, CultureInfo culture)
        {
            if (value == null)
                return default((string, string));

            var keyValue = value.Split(Separator);
            if (keyValue.Length == 0)
            {
                throw new FormatException($"Invalid value specified for {argName}. The string should be two key and value separated by '{Separator}'");
            }
            else if (keyValue.Length == 2)
            {
                return (keyValue[0], keyValue[1]);
            }
            else
            {
                return (keyValue[0], null);
            }
        }

        object IValueParser.Parse(string argName, string value, CultureInfo culture)
            => Parse(argName, value, culture);
    }
}
