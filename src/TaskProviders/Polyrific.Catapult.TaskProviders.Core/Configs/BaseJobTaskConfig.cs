// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.TaskProviders.Core.Configs
{
    public class BaseJobTaskConfig
    {
        /// <summary>
        /// Required external service connections
        /// </summary>
        public string[] RequiredServices { get; set; }

        /// <summary>
        /// Continue to the next task although the current execution is failed.
        /// Default is <value>false</value>.
        /// </summary>
        public bool ContinueWhenError { get; set; } = false;

        /// <summary>
        /// Does the pre-process needs to be success before executing the main task?
        /// Default is <value>false</value>.
        /// </summary>
        public bool PreProcessMustSucceed { get; set; } = true;

        /// <summary>
        /// Does the post-process needs to be success to complete the task?
        /// Default is <value>false</value>.
        /// </summary>
        public bool PostProcessMustSucceed { get; set; } = false;

        /// <summary>
        /// Location of the working directory
        /// </summary>
        public string WorkingLocation { get; set; }
    }
}
