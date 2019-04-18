// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Specifications
{
    public class HelpContextFilterSpecification : BaseSpecification<HelpContext>
    {
        public int Section { get; set; }

        public HelpContextFilterSpecification(int section)
            : base(m => m.Section == section)
        {
            Section = section;
        }
    }
}
