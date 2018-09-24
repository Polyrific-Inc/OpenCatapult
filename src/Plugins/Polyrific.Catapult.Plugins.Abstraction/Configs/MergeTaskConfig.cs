﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Plugins.Abstraction.Configs
{
    public class MergeTaskConfig : BaseJobTaskConfig
    {
        /// <summary>
        /// Remote repository
        /// </summary>
        public string Repository { get; set; }
    }
}
