// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Specifications
{
    public class HelpContextFilterSpecification : BaseSpecification<HelpContext>
    {
        public string Section { get; set; }

        public HelpContextFilterSpecification(string section)
            : base(m => m.Section == section, m => m.Sequence)
        {
            Section = section;
        }
    }
}
