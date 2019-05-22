// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;

namespace Polyrific.Catapult.TaskProviders.Core.Extensions
{
    public static class StringArrayExtensions
    {
        public static Dictionary<string, string> ToDictionary(this string[] args)
        {
            var result = new Dictionary<string, string>();

            var key = "";
            var value = "";
            var expectingValue = false;
            foreach (var arg in args)
            {
                if (arg.StartsWith("-") || arg.StartsWith("--"))
                {
                    if (expectingValue && !string.IsNullOrEmpty(key))
                    {
                        result.Add(key, value);
                    }

                    key = arg.TrimStart('-');
                    expectingValue = true;
                }
                else
                {
                    value = arg;
                    expectingValue = false;
                }

                if (!expectingValue && !string.IsNullOrEmpty(key))
                {
                    result.Add(key, value);

                    key = "";
                    value = "";
                }
            }

            return result;
        }
    }
}
