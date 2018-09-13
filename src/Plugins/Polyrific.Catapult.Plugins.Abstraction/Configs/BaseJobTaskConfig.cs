// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Plugins.Abstraction.Configs
{
    public class BaseJobTaskConfig
    {
        /// <summary>
        /// Name of the provider
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// Continue to the next task although the current execution is failed.
        /// </summary>
        public bool ContinueWhenError { get; set; }

        /// <summary>
        /// Location of the working directory
        /// </summary>
        public string WorkingLocation { get; set; }
    }
}