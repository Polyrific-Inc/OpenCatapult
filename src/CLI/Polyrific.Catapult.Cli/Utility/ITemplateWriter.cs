// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Cli
{
    public interface ITemplateWriter
    {
        /// <summary>
        /// Write a template into a file
        /// </summary>
        /// <param name="filePath">File path to write</param>
        /// <param name="content">Content of the file</param>
        /// <returns>The created file path</returns>
        string Write(string filePath, string content);

        /// <summary>
        /// Read a template file and returns the content
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <returns>The content of template file</returns>
        string Read(string filePath);
    }
}
