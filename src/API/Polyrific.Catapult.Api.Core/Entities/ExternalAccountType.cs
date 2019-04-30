// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Api.Core.Entities
{
    public class ExternalAccountType : BaseEntity
    {
        /// <summary>
        /// Key of the external account type
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Label of the external account type
        /// </summary>
        public string Label { get; set; }
    }
}
