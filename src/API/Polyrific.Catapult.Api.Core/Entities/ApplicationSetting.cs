// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Api.Core.Entities
{
    public class ApplicationSetting : BaseEntity
    {
        /// <summary>
        /// Key of the application setting
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Value of the application setting
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Label of the application setting
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Data type of the application setting
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Allowed values of the application setting
        /// </summary>
        public string AllowedValues { get; set; }
    }
}
