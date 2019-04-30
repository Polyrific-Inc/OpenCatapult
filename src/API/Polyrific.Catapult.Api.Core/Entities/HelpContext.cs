// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Api.Core.Entities
{
    public class HelpContext : BaseEntity
    {
        public string Section { get; set; }

        public string SubSection { get; set; }

        public string Text { get; set; }

        public int Sequence { get; set; }
    }
}
