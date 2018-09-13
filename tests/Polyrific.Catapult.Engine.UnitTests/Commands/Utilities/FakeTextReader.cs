// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.IO;

namespace Polyrific.Catapult.Engine.UnitTests.Commands.Utilities
{
    public class FakeTextReader : TextReader
    {
        private readonly string _inputString;

        public FakeTextReader(string inputString)
        {
            _inputString = inputString;
        }

        public override string ReadLine()
        {
            return _inputString;
        }
    }
}