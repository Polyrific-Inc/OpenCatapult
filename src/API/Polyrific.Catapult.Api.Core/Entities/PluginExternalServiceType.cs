// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Api.Core.Entities
{
    public class PluginExternalServiceType : BaseEntity
    {
        public int PluginId { get; set; }
        public Plugin Plugin { get; set; }

        public int ExternalServiceTypeId { get; set; }
        public ExternalServiceType ExternalServiceType { get; set; }
    }
}
