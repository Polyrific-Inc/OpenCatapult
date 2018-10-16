// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.IO;

namespace Polyrific.Catapult.Cli.UnitTests.Commands.Utilities
{
    internal class FakeTextReader : TextReader
    {
        private Queue<string> _inputString;

        public FakeTextReader(params string[] inputString)
        {
            this._inputString = new Queue<string>(inputString);
        }

        public override string ReadLine()
        {
            if (_inputString.Count > 1)
                return _inputString.Dequeue();
            else if (_inputString.Count == 1)
                return _inputString.Peek();
            else
                return "";
        }
    }
}
