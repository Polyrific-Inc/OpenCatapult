// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;

namespace Polyrific.Catapult.Plugins.DotNetCoreTest
{
    public interface ITestRunner
    {
        /// <summary>
        /// Run all unit tests found in a specified path
        /// </summary>
        /// <param name="projectPath">Path of the project</param>
        /// <param name="resultPath">Path in which the test result will be stored</param>
        /// <param name="continueWhenFailed">Continue current job when test is failed</param>
        /// <returns>Error message and the result file path</returns>
        Task<(string error, string resultFilePath)> RunTest(string projectPath, string resultPath, bool continueWhenFailed);
    }
}
