// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Plugins.Core.Configs
{
    public class BuildTaskConfig : BaseJobTaskConfig
    {
        /// <summary>
        /// Location of the source code
        /// </summary>
        public string SourceLocation { get; set; }

        /// <summary>
        /// Location of the output artifact
        /// </summary>
        public string OutputArtifactLocation { get; set; }
    }
}
