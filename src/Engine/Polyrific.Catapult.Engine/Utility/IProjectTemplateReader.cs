// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Shared.Dto.Project;

namespace Polyrific.Catapult.Engine.Utility
{
    public interface IProjectTemplateReader
    {
        /// <summary>
        /// Read a template file and returns <see cref="NewProjectDto"/> instance.
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <returns><see cref="NewProjectDto"/> instance</returns>
        NewProjectDto Read(string filePath);
    }
}
