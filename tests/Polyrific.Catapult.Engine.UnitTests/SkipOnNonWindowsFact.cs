// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Runtime.InteropServices;
using Xunit;

namespace Polyrific.Catapult.Engine.UnitTests
{
    public sealed class SkipOnNonWindowsFact : FactAttribute
    {
        public SkipOnNonWindowsFact()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Skip = "Skip on non-Windows platform.";
            }
        }
    }
}
