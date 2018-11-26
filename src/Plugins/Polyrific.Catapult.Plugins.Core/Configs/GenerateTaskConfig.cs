// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Plugins.Core.Configs
{
    public class GenerateTaskConfig : BaseJobTaskConfig
    {
        /// <summary>
        /// Location of the generated code to be put
        /// </summary>
        public string OutputLocation { get; set; }
    }
}
