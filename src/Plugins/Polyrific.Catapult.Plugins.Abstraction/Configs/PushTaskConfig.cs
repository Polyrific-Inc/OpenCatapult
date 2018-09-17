// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Plugins.Abstraction.Configs
{
    public class PushTaskConfig : BaseJobTaskConfig
    {
        /// <summary>
        /// Repository branch to push
        /// </summary>
        public string Branch { get; set; }
    }
}
