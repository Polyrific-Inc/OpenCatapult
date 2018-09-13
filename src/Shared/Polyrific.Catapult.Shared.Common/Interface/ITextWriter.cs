// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;

namespace Polyrific.Catapult.Shared.Common.Interface
{
    public interface ITextWriter
    {
        /// <summary>
        /// Write a text file
        /// </summary>
        /// <param name="folderName">Name of the folder</param>
        /// <param name="fileName">Name of the file</param>
        /// <param name="message">message to be written</param>
        /// <returns></returns>
        Task Write(string folderName, string fileName, string message);

        /// <summary>
        /// Read a text file
        /// </summary>
        /// <param name="folderName">Name of the folder</param>
        /// <param name="fileName">Name of the file</param>
        /// <returns>The text file content</returns>
        Task<string> Read(string folderName, string fileName);

        /// <summary>
        /// Delete a text file
        /// </summary>
        /// <param name="folderName">Name of the folder</param>
        /// <param name="fileName">Name of the file</param>
        /// <returns></returns>
        Task Delete(string folderName, string fileName);
    }
}
