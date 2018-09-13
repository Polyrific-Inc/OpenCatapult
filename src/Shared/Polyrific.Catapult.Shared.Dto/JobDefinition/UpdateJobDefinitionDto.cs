// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Shared.Dto.JobDefinition
{
    public class UpdateJobDefinitionDto
    {
        /// <summary>
        /// Id of the job definition
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Name of the data model
        /// </summary>
        public string Name { get; set; }
    }
}