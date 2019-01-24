// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;
using System;

namespace Polyrific.Catapult.Api.Data.Identity
{
    public class CatapultEngineProfile : BaseEntity
    {
        public DateTime? LastSeen { get; set; }

        public bool IsActive { get; set; }

        public string Version { get; set; }

        public int? CatapultEngineId { get; set; }
        public virtual ApplicationUser CatapultEngine { get; set; }
    }
}
