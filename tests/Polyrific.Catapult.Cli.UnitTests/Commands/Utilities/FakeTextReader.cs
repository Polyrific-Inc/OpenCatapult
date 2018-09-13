// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.IO;

namespace Polyrific.Catapult.Cli.UnitTests.Commands.Utilities
{
    internal class FakeTextReader : TextReader
    {
        private string _inputString;

        public FakeTextReader(string inputString)
        {
            this._inputString = inputString;
        }

        public override string ReadLine()
        {
            return _inputString;
        }
    }
}
