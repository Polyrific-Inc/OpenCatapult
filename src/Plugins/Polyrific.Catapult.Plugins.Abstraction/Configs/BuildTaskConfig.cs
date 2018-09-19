// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Plugins.Abstraction.Configs
{
    public class BuildTaskConfig : BaseJobTaskConfig
    {
        /// <summary>
        /// Location of the csproj file, relative to the working location
        /// </summary>
        public string CsprojLocation { get; set; }

        /// <summary>
        /// Location of the test csproj file, relative to the working location
        /// </summary>
        public string TestCsprojLocation { get; set; }
    }
}
