// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Plugins.Abstraction.Configs
{
    public class CloneTaskConfig : BaseJobTaskConfig
    {
        /// <summary>
        /// Remote repository
        /// </summary>
        public string Repository { get; set; }

        /// <summary>
        /// Location where the source code needs to cloned to
        /// </summary>
        public string CloneLocation { get; set; }
    }
}
