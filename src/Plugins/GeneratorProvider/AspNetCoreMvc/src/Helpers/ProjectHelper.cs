// Copyright (c) Polyrific, Inc 2018. All rights reserved.
///<OpenCatapultModelId>1</OpenCatapultModelId>

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AspNetCoreMvc.Helpers
{
    public class ProjectHelper
    {
        private readonly string _projectName;
        private readonly string _outputLocation;
        private readonly ILogger _logger;

        private const int _maxSearchLine = 10;
        private const string ModelIdLine = "///OpenCatapultModelId:";
        private const string ModelIdCshtmlLine = "<!--OpenCatapultModelId:";
        public const string GenericGeneratedCodeBlockBeginLine = "/*** Generated Code Begin ***/";
        public const string GenericGeneratedCodeBlockEndLine = "/*** Generated Code End ***/";

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

        public void AddFileToProject(string projectName, string filePath, string contents, bool overwrite = false, int modelId = 0)
        {
            var fullFilePath = Path.Combine(_outputLocation, projectName, filePath);
            var file = new FileInfo(fullFilePath);

            if (file.Exists && !overwrite && modelId == 0)
                return;

            if (modelId > 0)
            {
                switch (file.Extension)
                {
                    case ".cs":
                        if (!file.Exists)
                            CleanUpRenamedFile(file, modelId);

                        contents = contents.Insert(0, $"{ModelIdLine}{modelId}{Environment.NewLine}");
                        break;
                    case ".cshtml":
                        if (!file.Exists)
                            CleanUpRenamedViewFile(file, modelId);

                        contents = contents.Insert(0, $"{ModelIdCshtmlLine}{modelId}-->{Environment.NewLine}");
                        break;
                }
            }

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

        public void CleanUpFiles(string projectName, string folderPath, int[] modelIds)
        {
            var fullFolderPath = Path.Combine(_outputLocation, projectName, folderPath);

            var di = new DirectoryInfo(fullFolderPath);
            if (!di.Exists)
                return;

            var files = di.GetFiles();
            string line;
            int maxSearchLine = 10;
            foreach (var file in files)
            {
                int counter = 0;
                int modelId = 0;
                using (var fs = new StreamReader(file.FullName))
                {
                    while(counter <= maxSearchLine && (line = fs.ReadLine()) != null)
                    {
                        modelId = GetModelId(line, file);

                        if (modelId > 0)
                            break;

                        counter++;
                    }

                    fs.Close();
                }

                if (modelId > 0 && !modelIds.Contains(modelId))
                    File.Delete(file.FullName);
            }
        }

        public void CleanUpFolders(string projectName, string folderPath, int[] modelIds)
        {
            var fullFolderPath = Path.Combine(_outputLocation, projectName, folderPath);

            var di = new DirectoryInfo(fullFolderPath);
            if (!di.Exists)
                return;

            // go one level deeper
            var dirs = di.GetDirectories();
            foreach (var dir in dirs)
                CleanUpFiles(projectName, dir.FullName, modelIds);
        }

        private void CleanUpRenamedFile(FileInfo file, int modelId)
        {
            if (!file.Directory.Exists)
                return;

            var files = file.Directory.EnumerateFiles();
            foreach (var otherFile in files)
            {
                if (DeleteFileByModelId(otherFile, modelId))
                    break;
            }
        }

        private void CleanUpRenamedViewFile(FileInfo file, int modelId)
        {
            if (!(file.Directory.Parent?.Exists ?? false))
                return;

            var otherDirs = file.Directory.Parent.EnumerateDirectories();
            foreach (var otherDir in otherDirs)
            {
                if (file.Directory.Name == otherDir.Name)
                    continue;

                var otherFiles = otherDir.EnumerateFiles();
                bool cleaned = false;
                foreach (var otherFile in otherFiles)
                {
                    if (file.Name == otherFile.Name)
                    {
                        cleaned = DeleteFileByModelId(otherFile, modelId);
                        break;
                    }
                }

                if (cleaned)
                    break;
            }
        }

        private bool DeleteFileByModelId(FileInfo file, int modelId)
        {
            string line;
            int counter = 0;
            int fileModelId = 0;
            using (var fs = new StreamReader(file.FullName))
            {
                while (counter <= _maxSearchLine && (line = fs.ReadLine()) != null)
                {
                    fileModelId = GetModelId(line, file);
                    if (fileModelId > 0)
                        break;

                    counter++;
                }
            }

            if (fileModelId == modelId)
            {
                File.Delete(file.FullName);
                return true;
            }

            return false;
        }
        
        private int GetModelId(string line, FileSystemInfo file)
        {
            switch (file.Extension)
            {
                case ".cs":
                    if (line.StartsWith(ModelIdLine))
                        if (int.TryParse(line.Split(':').LastOrDefault(), out var modelId))
                            return modelId;
                    break;
                case ".cshtml":
                    if (line.Contains(ModelIdCshtmlLine))
                        if (int.TryParse(line.Replace("-->", "").Split(':').LastOrDefault(), out var modelId))
                            return modelId;
                    break;
            }                

            return 0;
        }
    }
}
