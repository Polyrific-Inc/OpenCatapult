// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;

namespace Polyrific.Catapult.Shared.Dto.JobDefinition
{
    public class UpdateJobTaskConfigDto
    {
        public int Id { get; set; }

        public Dictionary<string, string> Config { get; set; }
    }
}