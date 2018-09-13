// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Specifications
{
    public class ExternalServiceFilterSpecification : BaseSpecification<ExternalService>
    {
        public string Name { get; set; }
        public int UserId { get; set; }

        public ExternalServiceFilterSpecification(string name)
            : base(m => m.Name == name)
        {
            Name = name;
        }

        public ExternalServiceFilterSpecification(int userId)
            : base(m => m.UserId == userId)
        {
            UserId = userId;
        }
    }
}