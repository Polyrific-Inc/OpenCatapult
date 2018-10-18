﻿// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AspNetCoreMvc.Helpers
{
    public class ProjectHelper
    {
        private readonly string _projectName;
        private readonly string _outputLocation;
        private readonly ILogger _logger;

        public ProjectHelper(string projectName, string outputLocation, ILogger logger)
        {
            _projectName = projectName;
            _outputLocation = outputLocation;
            _logger = logger;
        }

        public async Task<string> CreateProject(string projectName, string template, string[] projectReferences = null, (string name, string version)[] packages = null)
        {
            var projectFolder = Path.Combine(_outputLocation, projectName);
            if (!Directory.Exists(projectFolder))
                Directory.CreateDirectory(projectFolder);

            var args = $"new {template} -n {projectName} -o {projectFolder}";

            var message = await CommandHelper.RunDotnet(args, null, _logger);
            DeleteFileToProject(projectName, "Class1.cs");

            var projectFile = GetProjectFullPath(projectName);

            if (projectReferences != null)
                foreach (var projectReference in projectReferences)
                    await CommandHelper.RunDotnet($"add {projectFile} reference {projectReference}", null, _logger);

            if (packages != null)
                foreach (var package in packages)
                    await CommandHelper.RunDotnet($"add {projectFile} package {package.name} -v {package.version}", null, _logger);

            // add project to solution
            var solutionFile = Path.Combine(_outputLocation, $"{_projectName}.sln");
            await CommandHelper.RunDotnet($"sln {solutionFile} add {projectFile}", null, _logger);

            return message;
        }

        public void AddFileToProject(string projectName, string filePath, string contents, bool overwrite = false)
        {
            var fullFilePath = Path.Combine(_outputLocation, projectName, filePath);
            var file = new FileInfo(fullFilePath);

            if (file.Exists && !overwrite)
                return;

            file.Directory.Create();
            File.WriteAllText(file.FullName, contents);
        }

        public void CopyFileToProject(string projectName, string sourceFile, string targetFile)
        {
            var targetFilePath = Path.Combine(_outputLocation, projectName, targetFile);
            File.Copy(sourceFile, targetFilePath, true);
        }

        public void DeleteFileToProject(string projectName, string filePath)
        {
            var fullFilePath = Path.Combine(_outputLocation, projectName, filePath);
            var file = new FileInfo(fullFilePath);

            if (file.Exists)
                file.Delete();
        }

        public bool IsFileExistInProject(string projectName, string filePath)
        {
            var fullFilePath = Path.Combine(_outputLocation, projectName, filePath);
            return File.Exists(fullFilePath);
        }

        public string GetProjectFullPath(string projectName)
        {
            return Path.Combine(_outputLocation, $"{projectName}", $"{projectName}.csproj");
        }

        public string GetProjectFolder(string projectName)
        {
            return Path.Combine(_outputLocation, $"{projectName}");
        }
    }
}
