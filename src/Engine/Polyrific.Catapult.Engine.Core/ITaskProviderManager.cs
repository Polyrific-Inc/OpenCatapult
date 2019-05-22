// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Engine.Core
{
    public interface ITaskProviderManager
    {
        /// <summary>
        /// Task provider locations
        /// </summary>
        List<string> TaskProviderLocations { get; }

        /// <summary>
        /// Add new location of the task provider
        /// </summary>
        /// <param name="location"></param>
        void AddTaskProviderLocation(string location);

        /// <summary>
        /// Get task providers by the provider type
        /// </summary>
        /// <param name="providerType"></param>
        /// <returns></returns>
        List<TaskProviderItem> GetTaskProviders(string providerType);

        /// <summary>
        /// Get the task provider by name
        /// </summary>
        /// <param name="providerType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        TaskProviderItem GetTaskProvider(string providerType, string name);

        /// <summary>
        /// Refresh task provider collection
        /// </summary>
        void RefreshTaskProviders();

        /// <summary>
        /// Invoke task provider
        /// </summary>
        /// <param name="taskProviderDll">Path to the task provider dll file</param>
        /// <param name="taskProviderArgs">Arguments to be passed when executing task provider</param>
        /// <param name="securedTaskProviderArgs">Arguments that has been stripped from secret values, to be shown in UI for traceability</param>
        /// <returns></returns>
        Task<Dictionary<string, object>> InvokeTaskProvider(string taskProviderDll, string taskProviderArgs, string securedTaskProviderArgs = null);
    }
}
