// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using McMaster.Extensions.CommandLineUtils;
using System;
using System.IO;
using Xunit.Abstractions;

namespace Polyrific.Catapult.Engine.UnitTests.Commands.Utilities
{
    public class TestConsole : IConsole
    {
        public TestConsole(ITestOutputHelper output, string inputString = "")
        {
            Out = new XunitTextWriter(output);
            Error = new XunitTextWriter(output);
            In = new FakeTextReader(inputString);
        }

        public TextWriter Out { get; set; }

        public TextWriter Error { get; set; }

        public TextReader In { get; set; }

        public bool IsInputRedirected => true;

        public bool IsOutputRedirected => true;

        public bool IsErrorRedirected => true;

        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }

        public event ConsoleCancelEventHandler CancelKeyPress
        {
            add {}
            remove {}
        }

        public void ResetColor()
        {
            
        }
    }
}