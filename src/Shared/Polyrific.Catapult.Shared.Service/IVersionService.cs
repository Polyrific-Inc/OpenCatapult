// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;

namespace Polyrific.Catapult.Shared.Service
{
    public interface IVersionService
    {
        /// <summary>
        /// Get version of the API
        /// </summary>
        /// <returns></returns>
        Task<string> GetApiVersion();
    }
}
