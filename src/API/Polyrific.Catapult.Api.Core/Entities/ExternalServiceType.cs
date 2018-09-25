// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;

namespace Polyrific.Catapult.Api.Core.Entities
{
    public class ExternalServiceType : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<ExternalServiceProperty> ExternalServiceProperties { get; set; }
    }
}
