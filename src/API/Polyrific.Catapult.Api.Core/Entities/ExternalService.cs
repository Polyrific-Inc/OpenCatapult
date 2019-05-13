// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Api.Core.Entities
{
    public class ExternalService : BaseEntity
    {
        /// <summary>
        /// Name of the external service
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the external service
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Id of the user
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Id of the External service type
        /// </summary>
        public int? ExternalServiceTypeId { get; set; }
        public virtual ExternalServiceType ExternalServiceType { get; set; }

        /// <summary>
        /// Serialized external service configuration value
        /// </summary>
        public string ConfigString { get; set; }

        /// <summary>
        /// Is the external service can be access globally?
        /// </summary>
        public bool IsGlobal { get; set; }
    }
}
