// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Entities
{
    public class JobCounter : BaseEntity
    {
        public DateTime Date { get; set; }

        public int Count { get; set; }
    }
}
