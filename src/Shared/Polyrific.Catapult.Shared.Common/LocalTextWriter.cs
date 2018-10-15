// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Shared.Common.Interface;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Shared.Common
{
    public class LocalTextWriter : ITextWriter
    {
        private readonly string _basePath;

        public LocalTextWriter(string basePath)
        {
            _basePath = basePath;
        }

        public Task Delete(string folderName, string fileName)
        {
            var folderPath = GetFolderPath(folderName);
            var filePath = Path.Combine(folderPath, fileName);

            return Task.Run(() => File.Delete(filePath));
        }

        public async Task<string> Read(string folderName, string fileName)
        {
            var folderPath = GetFolderPath(folderName);

            if (!string.IsNullOrEmpty(fileName))
            {
                var filePath = Path.Combine(folderPath, fileName);

                return await FileHelper.ReadAllTextAsync(filePath);
            }
            else
            {
                var di = new DirectoryInfo(folderPath);
                if (!di.Exists)
                    return null;

                FileSystemInfo[] files = di.GetFileSystemInfos();
                var orderedFiles = files.OrderBy(f => f.CreationTime)
                                        .Select(f => f.FullName).ToList();

                var sb = new StringBuilder();
                foreach (var filePath in orderedFiles)
                {
                    sb.AppendLine(await FileHelper.ReadAllTextAsync(filePath));
                }

                return sb.ToString();
            }
        }

        public async Task Write(string folderName, string fileName, string message)
        {
            var folderPath = GetFolderPath(folderName);
            var filePath = Path.Combine(folderPath, fileName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            else
            {
                message = Environment.NewLine + message;
            }

            await FileHelper.AppendAllTextAsync(filePath, message);
        }

        private string GetFolderPath(string folderName)
        {
            return Path.Combine(_basePath ?? string.Empty, $"{folderName}");
        }
    }
}
