// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Engine.Core.JobLogger
{
    public class TaskScope
    {
        public TaskScope(string taskName)
        {
            TaskName = taskName;
        }

        public string TaskName { get; set; }
    }
}
