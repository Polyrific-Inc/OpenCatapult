// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.IO;
using Polyrific.Catapult.Shared.Dto.Project;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Polyrific.Catapult.Engine.Utility
{
    public class ProjectTemplateReader : IProjectTemplateReader
    {
        public NewProjectDto Read(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            var content = File.ReadAllText(filePath);
            var deserializer = new DeserializerBuilder().WithNamingConvention(new HyphenatedNamingConvention()).IgnoreUnmatchedProperties().Build();
            return deserializer.Deserialize<NewProjectDto>(content);
        }
    }
}
