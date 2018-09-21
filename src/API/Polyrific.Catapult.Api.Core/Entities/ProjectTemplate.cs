// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;

namespace Polyrific.Catapult.Api.Core.Entities
{
    internal class ProjectTemplate
    {
        public string Name { get; set; }
        public string Client { get; set; }
        public Dictionary<string, string> Config { get; set; }
        public List<ProjectDataModelTemplate> Models { get; set; }
        public List<JobDefinitionTemplate> Jobs { get; set; }
    }

    internal class ProjectDataModelTemplate
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public List<ProjectDataModelPropertyTemplate> Properties { get; set; }
    }

    internal class ProjectDataModelPropertyTemplate
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string DataType { get; set; }
        public string ControlType { get; set; }
        public string RelatedProjectDataModelName { get; set; }
        public string RelationalType { get; set; }
        public bool IsRequired { get; set; }
    }

    internal class JobDefinitionTemplate
    {
        public string Name { get; set; }
        public List<JobTaskDefinitionTemplate> Tasks { get; set; }
    }

    internal class JobTaskDefinitionTemplate
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Provider { get; set; }
        public Dictionary<string, string> Config { get; set; }
        public int? Sequence { get; set; }
    }
}
