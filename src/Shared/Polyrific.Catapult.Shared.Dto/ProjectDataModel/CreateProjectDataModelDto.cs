// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Shared.Dto.ProjectDataModel
{
    public class CreateProjectDataModelDto
    {
        /// <summary>
        /// Name of the data model
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the data model
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Label of the data model
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Is the model managed in the UI?
        /// </summary>
        public bool? IsManaged { get; set; }

        /// <summary>
        /// The property name used as the key for select control
        /// </summary>
        public string SelectKey { get; set; }
    }
}
