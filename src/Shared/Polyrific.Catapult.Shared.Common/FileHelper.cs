// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Buffers;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Shared.Common
{
    /// <summary>
    /// Helper to support unavailable implementations of <see cref="System.IO.File"/> in netstandard2.0
    /// </summary>
    public static class FileHelper
    {
        internal const int DefaultBufferSize = 4096;

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

        public static Task AppendAllTextAsync(string path, string contents, CancellationToken cancellationToken = default(CancellationToken))
        {
            var encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            if (path.Length == 0)
                throw new ArgumentException("Path is empty", nameof(path));

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            if (string.IsNullOrEmpty(contents))
            {
                // Just to throw exception if there is a problem opening the file.
                new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read).Dispose();
                return Task.CompletedTask;
            }

            return InternalWriteAllTextAsync(AsyncStreamWriter(path, encoding, append: true), contents, cancellationToken);
        }

        private static async Task InternalWriteAllTextAsync(StreamWriter sw, string contents, CancellationToken cancellationToken)
        {
            char[] buffer = null;
            try
            {
                buffer = ArrayPool<char>.Shared.Rent(DefaultBufferSize);
                int count = contents.Length;
                int index = 0;
                while (index < count)
                {
                    int batchSize = Math.Min(DefaultBufferSize, count - index);
                    contents.CopyTo(index, buffer, 0, batchSize);
                    await sw.WriteAsync(buffer, 0, batchSize).ConfigureAwait(false);
                    index += batchSize;
                }

                cancellationToken.ThrowIfCancellationRequested();
                await sw.FlushAsync().ConfigureAwait(false);
            }
            finally
            {
                sw.Dispose();
                if (buffer != null)
                {
                    ArrayPool<char>.Shared.Return(buffer);
                }
            }
        }

        private static StreamWriter AsyncStreamWriter(string path, Encoding encoding, bool append)
        {
            var stream = new FileStream(
                path, append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.Read, DefaultBufferSize,
                FileOptions.Asynchronous | FileOptions.SequentialScan);

            return new StreamWriter(stream, encoding);
        }
    }
}
