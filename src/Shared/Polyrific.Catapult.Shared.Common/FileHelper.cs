// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.IO;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Shared.Common
{
    /// <summary>
    /// Helper to support unavailable implementations of <see cref="System.IO.File"/> in netstandard2.0
    /// </summary>
    public static class FileHelper
    {
        public static async Task<string> ReadAllTextAsync(string path)
        {
            string text;
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = new StreamReader(fileStream))
                {
                    text = await reader.ReadToEndAsync();
                }
            }

            return text;
        }

        public static async Task WriteAllTextAsync(string path, string content)
        {
            using (var writer = File.CreateText(path))
            {
                await writer.WriteAsync(content);

                await writer.FlushAsync();
            }
        }

        public static async Task AppendAllTextAsync(string path, string content)
        {
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
            {
                using (var writer = new StreamWriter(fileStream))
                {
                    await writer.WriteAsync(content);

                    await writer.FlushAsync();
                }
            }
        }
    }
}
