// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Entities
{
    public class CatapultEngine : BaseEntity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastSeen { get; set; }
        public string Version { get; set; }
    }
}
