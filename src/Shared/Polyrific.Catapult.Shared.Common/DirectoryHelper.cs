// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.IO;
using System.Linq;

namespace Polyrific.Catapult.Shared.Common
{
    public class DirectoryHelper
    {
        public static string[] GetChildFolders(string path)
        {
            if (!Directory.Exists(path))
                return new string[0];

            var subDirectories = Directory.GetDirectories(path);
            return subDirectories.Select(sd => sd.Substring(sd.LastIndexOf("\\", StringComparison.Ordinal))).ToArray();
        }
    }
}