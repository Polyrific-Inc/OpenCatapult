// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Shared.Dto.ProjectDataModel
{
    public class UpdateProjectDataModelPropertyDto
    {
        /// <summary>
        /// Id of the data model property
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Name of the property
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Label of the property
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Data type of the property
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Is property required?
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Control type of the property
        /// </summary>
        public string ControlType { get; set; }

        /// <summary>
        /// Id of the related project data model
        /// </summary>
        public int? RelatedProjectDataModelId { get; set; }

        /// <summary>
        /// Type of the relation with the related project data model
        /// </summary>
        public string RelationalType { get; set; }

        /// <summary>
        /// Is the property managed in the UI?
        /// </summary>
        public bool? IsManaged { get; set; }
    }
}
