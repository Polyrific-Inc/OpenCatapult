// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Specifications
{
    public class ExternalAccountTypeFilterSpecification : BaseSpecification<ExternalAccountType>
    {
        public ExternalAccountTypeFilterSpecification()
            : base(m => true)
        {
        }
    }
}
