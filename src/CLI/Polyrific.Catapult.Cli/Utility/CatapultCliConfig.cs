// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Cli
{
    public class CatapultCliConfig
    {
        /// <summary>
        /// Base address of the API
        /// </summary>
        public Uri ApiUrl { get; set; }

        /// <summary>
        /// The child folder path in AppData
        /// </summary>
        public string AppDataFolderPath { get; set; }

        /// <summary>
        /// The child folder path for installed template
        /// </summary>
        public string InstalledTemplateFolder { get; set; }
    }
}
