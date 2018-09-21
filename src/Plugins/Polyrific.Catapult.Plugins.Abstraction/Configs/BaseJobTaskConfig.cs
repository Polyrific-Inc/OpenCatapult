// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Plugins.Abstraction.Configs
{
    public class BaseJobTaskConfig
    {
        /// <summary>
        /// Continue to the next task although the current execution is failed.
        /// Default is <see cref="false"/>.
        /// </summary>
        public bool ContinueWhenError { get; set; } = false;

        /// <summary>
        /// Does the pre-process needs to be success before executing the main task?
        /// Default is <see cref="true"/>
        /// </summary>
        public bool PreProcessMustSucceed { get; set; } = true;

        /// <summary>
        /// Does the post-process needs to be success to complete the task?
        /// Default is <see cref="false"/>
        /// </summary>
        public bool PostProcessMustSucceed { get; set; } = false;

        /// <summary>
        /// Location of the working directory
        /// </summary>
        public string WorkingLocation { get; set; }
    }
}
