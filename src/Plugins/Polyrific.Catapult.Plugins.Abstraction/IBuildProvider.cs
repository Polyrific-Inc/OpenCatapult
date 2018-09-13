// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;
using Polyrific.Catapult.Plugins.Abstraction.Configs;

namespace Polyrific.Catapult.Plugins.Abstraction
{
    public interface IBuildProvider
    {
        /// <summary>
        /// Name of the provider
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Build the code, and produce a ready to deploy artifact
        /// </summary>
        /// <param name="config">Build task configuration</param>
        /// <returns></returns>
        Task<(string returnValue, string errorMessage)> Build(BuildTaskConfig config);
    }
}