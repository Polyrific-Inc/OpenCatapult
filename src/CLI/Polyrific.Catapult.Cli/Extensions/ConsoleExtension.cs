// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using McMaster.Extensions.CommandLineUtils;

namespace Polyrific.Catapult.Cli.Extensions
{
    public static class ConsoleExtension
    {
        /// <summary>
        /// Gets a console response from the console after displaying a <paramref name="prompt" />.
        /// </summary>
        /// <param name="console">The console</param>
        /// <param name="prompt">The question to display on command line</param>
        /// <param name="defaultValue">If the user enters a blank response, return this value instead.</param>
        /// <param name="promptColor">The console color to use for the prompt</param>
        /// <param name="promptBgColor">The console background color for the prompt</param>
        /// <returns>The response the user gave. Can be null or empty</returns>
        public static string GetString(this IConsole console, string prompt, string defaultValue = null, ConsoleColor? promptColor = null, ConsoleColor? promptBgColor = null)
        {
            if (defaultValue != null)
            {
                prompt = $"{prompt} [{defaultValue}]";
            }

            Write(prompt, promptColor, promptBgColor);
            console.Write(' ');

            string resp;
            using (ShowCursor())
            {
                resp = console.In.ReadLine();
            }

            if (!string.IsNullOrEmpty(resp))
            {
                return resp;
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets a yes/no response from the console after displaying a <paramref name="prompt" />.
        /// <para>
        /// The parsing is case insensitive. Valid responses include: yes, no, y, n.
        /// </para>
        /// </summary>
        /// <param name="prompt">The question to display on the command line</param>
        /// <param name="defaultAnswer">If the user provides an empty response, which value should be returned</param>
        /// <param name="promptColor">The console color to display</param>
        /// <param name="promptBgColor">The console background color for the prompt</param>
        /// <returns>True is 'yes'</returns>
        public static bool GetYesNo(this IConsole console, string prompt, bool defaultAnswer, ConsoleColor? promptColor = null, ConsoleColor? promptBgColor = null)
        {
            var answerHint = defaultAnswer ? "[Y/n]" : "[y/N]";
            do
            {
                Write($"{prompt} {answerHint}", promptColor, promptBgColor);
                Console.Write(' ');

                string resp;
                using (ShowCursor())
                {
                    resp = console.In.ReadLine()?.ToLower()?.Trim();
                }

                if (string.IsNullOrEmpty(resp))
                {
                    return defaultAnswer;
                }

                if (resp == "n" || resp == "no")
                {
                    return false;
                }

                if (resp == "y" || resp == "yes")
                {
                    return true;
                }

                Console.WriteLine($"Invalid response '{resp}'. Please answer 'y' or 'n' or CTRL+C to exit.");
            }
            while (true);
        }
        
        /// <summary>
        /// Gets a yes/no response from the console after displaying a <paramref name="prompt" />.
        /// <para>
        /// The parsing is case insensitive. Valid responses include: yes, no, y, n.
        /// </para>
        /// </summary>
        /// <param name="prompt">The question to display on the command line</param>
        /// <param name="defaultAnswer">If the user provides an empty response, which value should be returned</param>
        /// <param name="promptColor">The console color to display</param>
        /// <param name="promptBgColor">The console background color for the prompt</param>
        /// <returns>True is 'yes'</returns>
        public static bool? GetYesNoNullable(this IConsole console, string prompt, bool? defaultAnswer = null, ConsoleColor? promptColor = null, ConsoleColor? promptBgColor = null)
        {
            var answerHint = "[y/n]";

            if (defaultAnswer != null)
            {
                answerHint = defaultAnswer.Value ? "[Y/n]" : "[y/N]";
            }

            do
            {
                Write($"{prompt} {answerHint}", promptColor, promptBgColor);
                Console.Write(' ');

                string resp;
                using (ShowCursor())
                {
                    resp = console.In.ReadLine()?.ToLower()?.Trim();
                }

                if (string.IsNullOrEmpty(resp))
                {
                    return defaultAnswer;
                }

                if (resp == "n" || resp == "no")
                {
                    return false;
                }

                if (resp == "y" || resp == "yes")
                {
                    return true;
                }

                Console.WriteLine($"Invalid response '{resp}'. Please answer 'y' or 'n' or CTRL+C to exit.");
            }
            while (true);
        }

        private static void Write(string value, ConsoleColor? foreground, ConsoleColor? background)
        {
            if (foreground.HasValue)
            {
                Console.ForegroundColor = foreground.Value;
            }

            if (background.HasValue)
            {
                Console.BackgroundColor = background.Value;
            }

            Console.Write(value);

            if (foreground.HasValue || background.HasValue)
            {
                Console.ResetColor();
            }
        }

        private static IDisposable ShowCursor() => new CursorState();

        private class CursorState : IDisposable
        {
            private readonly bool _original;

            public CursorState()
            {
                try
                {
                    _original = Console.CursorVisible;
                }
                catch
                {
                    // some platforms throw System.PlatformNotSupportedException
                    // Assume the cursor should be shown
                    _original = true;
                }

                TrySetVisible(true);
            }

            private void TrySetVisible(bool visible)
            {
                try
                {
                    Console.CursorVisible = visible;
                }
                catch
                {
                    // setting cursor may fail if output is piped or permission is denied.
                }
            }

            public void Dispose()
            {
                TrySetVisible(_original);
            }
        }
    }
}
