// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Diagnostics;
using System.IO;

namespace Polyrific.Catapult.Engine.Core
{
    public class PluginProcess : IPluginProcess
    {
        public StreamReader GetStandardOutput(Process process)
        {
            return process.StandardOutput;
        }

        public Process Start(ProcessStartInfo startInfo)
        {
            return Process.Start(startInfo);
        }
    }
}
