// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class TaskConfigRequiredException : Exception
    {
        public string TaskType { get; set; }
        public string ConfigName { get; set; }

        public TaskConfigRequiredException(string taskType, string configName)
            : base($"Task type {taskType} require task config \"{configName}\"")
        {
            TaskType = taskType;
            ConfigName = configName;
        }
    }
}
