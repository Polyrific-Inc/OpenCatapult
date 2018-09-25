// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Linq;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Specifications
{
    public class ExternalServiceTypeFilterSpecification : BaseSpecification<ExternalServiceType>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string[] ServiceTypeNames { get; set; }

        public ExternalServiceTypeFilterSpecification()
            : base(m => true)
        {
        }

        public ExternalServiceTypeFilterSpecification(string name)
            : base(m => name == null || m.Name == name)
        {
            Name = name;
        }

        public ExternalServiceTypeFilterSpecification(int id)
            : base(m => m.Id == id)
        {
            Id = id;
        }

        public ExternalServiceTypeFilterSpecification(string[] serviceTypeNames)
            : base(m => serviceTypeNames.Contains(m.Name))
        {
            ServiceTypeNames = serviceTypeNames;
        }
    }
}
