// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Plugins.Abstraction.Configs
{
    public class TestTaskConfig : BaseJobTaskConfig
    {
        /// <summary>
        /// Location of the test code
        /// </summary>
        public string TestLocation { get; set; }
    }
}
