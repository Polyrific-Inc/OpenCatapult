// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Shared.Dto.JobQueue
{
    public class JobTaskStatusDto
    {
        public int Sequence { get; set; }
        public string TaskName { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}
