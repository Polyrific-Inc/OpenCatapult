// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Humanizer;

namespace Polyrific.Catapult.TaskProviders.AspNetCoreMvc.Helpers
{
    public static class TextHelper
    {
        public const string Tab = "    ";

        public static string Pluralize(string text)
        {
            return text.Pluralize(false);
        }

        public static string Camelize(string text)
        {
            return text.Camelize();
        }

        public static string Pascalize(string text)
        {
            return text.Pascalize();
        }
    }
}
