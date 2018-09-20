// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Api.Core.Entities
{
    public class ExternalService : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }

        public int? ExternalServiceTypeId { get; set; }
        public virtual ExternalServiceType ExternalServiceType { get; set; }

        public string ConfigString { get; set; }
    }
}
