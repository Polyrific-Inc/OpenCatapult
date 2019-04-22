// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Api.Core.Entities
{
    public class TaskProviderAdditionalConfig : BaseEntity
    {
        /// <summary>
        /// Related task provider Id
        /// </summary>
        public int TaskProviderId { get; set; }

        /// <summary>
        /// Related task provider object
        /// </summary>
        public virtual TaskProvider TaskProvider { get; set; }

        /// <summary>
        /// Name of the configuration
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Display label of the configuration
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Data type of the configuration (string, number, boolean)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Is the configuration mandatory?
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Is the configuration contains sensitive value?
        /// </summary>
        public bool IsSecret { get; set; }

        /// <summary>
        /// Is the configuration masked during input?
        /// </summary>
        public bool? IsInputMasked { get; set; }

        /// <summary>
        /// Allowed value of the additional config
        /// </summary>
        public string AllowedValues { get; set; }

        /// <summary>
        /// A short hint to inform user what should be inputted in the additional config
        /// </summary>
        public string Hint { get; set; }
    }
}
