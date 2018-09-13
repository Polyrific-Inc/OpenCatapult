// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;

namespace Polyrific.Catapult.Shared.Dto.ProjectDataModel
{
    public class CreateProjectDataModelWithPropertiesDto : CreateProjectDataModelDto
    {
        /// <summary>
        /// Properties of the new data model
        /// </summary>
        public List<CreateProjectDataModelPropertyDto> Properties { get; set; }
    }
}
