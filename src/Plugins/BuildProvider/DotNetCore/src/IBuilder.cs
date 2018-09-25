// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;

namespace DotNetCore
{
    public interface IBuilder
    {
        /// <summary>
        /// Build the source code
        /// </summary>
        /// <param name="csprojLocation">Location of the csproj file</param>
        /// <param name="buildOutputLocation">Location of the build output</param>
        /// <param name="configuration">Build configuration (default is Debug)</param>
        /// <returns>Error message</returns>
        Task<string> Build(string csprojLocation, string buildOutputLocation, string configuration = "Debug");

        /// <summary>
        /// Create build artifact
        /// </summary>
        /// <param name="buildOutputLocation">Location of the build output</param>
        /// <param name="destinationArtifact">Name of the artifact package, including the complete location</param>
        /// <returns>Error message</returns>
        Task<string> CreateArtifact(string buildOutputLocation, string destinationArtifact);

        /// <summary>
        /// Restore the NuGet packages
        /// </summary>
        /// <param name="csprojLocation">Location of the csproj file</param>
        /// <returns>Error message</returns>
        Task<string> Restore(string csprojLocation);

        /// <summary>
        /// Run test
        /// </summary>
        /// <param name="csprojLocation">csprojLocation</param>
        /// <returns>Error message</returns>
        Task<string> Test(string csprojLocation);
    }
}
