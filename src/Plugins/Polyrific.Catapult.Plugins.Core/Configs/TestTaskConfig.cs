// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Plugins.Core.Configs
{
    public class TestTaskConfig : BaseJobTaskConfig
    {
        /// <summary>
        /// Location of the test code
        /// </summary>
        public string TestLocation { get; set; }

        /// <summary>
        /// Continue the job execution when the test is failed. Default to false
        /// </summary>
        public bool ContinueWhenFailed { get; set; }
    }
}
