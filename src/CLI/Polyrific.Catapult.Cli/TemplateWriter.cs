// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.IO;

namespace Polyrific.Catapult.Cli
{
    public class TemplateWriter : ITemplateWriter
    {
        private readonly CatapultCliConfig _config;
        private const string FileExtension = "yaml";

        public TemplateWriter(CatapultCliConfig config)
        {
            _config = config;
        }

        public string Read(string filePath)
        {
            if (!filePath.Contains('/') && !filePath.Contains('\\'))
            {
                filePath = GetDefaultTemplateFilePath(filePath);
            }

            return File.ReadAllText(filePath);
        }

        public string Write(string filePath, string content)
        {
            if (!filePath.Contains('/') && !filePath.Contains('\\'))
            {
                filePath = GetDefaultTemplateFilePath(filePath);
            }

            var folderPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            File.WriteAllText(filePath, content);

            return filePath;
        }

        private string GetDefaultTemplateFilePath(string templateName)
        {
            var defaultPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _config.AppDataFolderPath, _config.InstalledTemplateFolder);
            return Path.Combine(defaultPath, $"{templateName}.{FileExtension}");
        }
    }
}
