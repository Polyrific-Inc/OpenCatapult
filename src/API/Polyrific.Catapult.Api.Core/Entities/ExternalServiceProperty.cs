// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Api.Core.Entities
{
    public class ExternalServiceProperty : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string AllowedValues { get; set; }

        public bool IsRequired { get; set; }

        public bool IsSecret { get; set; }

        public string AdditionalLogic { get; set; }

        public int Sequence { get; set; }

        public int? ExternalServiceTypeId { get; set; }
    }
}
