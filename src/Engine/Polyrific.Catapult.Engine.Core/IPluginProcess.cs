// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Diagnostics;
using System.IO;

namespace Polyrific.Catapult.Engine.Core
{
    public interface IPluginProcess
    {
        Process Start(ProcessStartInfo startInfo);
        StreamReader GetStandardOutput(Process process);
        StreamReader GetStandardError(Process process);
    }
}
