// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;

namespace Polyrific.Catapult.Api.Core.Entities
{
    public class ManagedFile : BaseEntity
    {
        public string FileName { get; set; }
        public byte[] File { get; set; }
    }
}
