// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Core.Services
{
    public interface ICatapultEngineService
    {
        /// <summary>
        /// Create new catapult engine
        /// </summary>
        /// <param name="name">The name of catapult engine</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task<CatapultEngine> CreateCatapultEngine(string name, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Delete a catapult engine
        /// </summary>
        /// <param name="catapultEngineId">Id of the catapult engine</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task DeleteCatapultEngine(int catapultEngineId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Generate token which is used to confirm new catapult engine registration
        /// </summary>
        /// <param name="catapultEngineId">Id of the catapult engine</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Confirmation token</returns>
        Task<string> GenerateConfirmationToken(int catapultEngineId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Confirm catapult engine registration
        /// </summary>
        /// <param name="catapultEngineId">Id of the catapult engine</param>
        /// <param name="token">Confirmation token</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task ConfirmRegistration(int catapultEngineId, string token, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get catapult engine id from claims principal
        /// </summary>
        /// <param name="principal">Claims principal</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Id of the catapult engine</returns>
        Task<int> GetCatapultEngineId(ClaimsPrincipal principal, CancellationToken cancellationToken = default(CancellationToken));
        
        /// <summary>
        /// Validate catapultEngineName + password
        /// </summary>
        /// <param name="catapultEngineName">The catapult engine name</param>
        /// <param name="password">Password of the catapult engine</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Validity status</returns>
        Task<bool> ValidateCatapultEnginePassword(string catapultEngineName, string password, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get the catapult engine role
        /// </summary>
        /// <param name="catapultEngineId">Catapult engine id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The catapult engine role</returns>
        Task<string> GetCatapultEngineRole(int catapultEngineId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get catapult engine by catapultEngineName
        /// </summary>
        /// <param name="catapultEngineId">The catapult engine id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The catapult engine entity</returns>
        Task<CatapultEngine> GetCatapultEngine(int catapultEngineId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get catapult engine by catapultEngineName
        /// </summary>
        /// <param name="catapultEngineName">The catapult engine name</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The catapult engine entity</returns>
        Task<CatapultEngine> GetCatapultEngine(string catapultEngineName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Suspend an engine
        /// </summary>
        /// <param name="catapultEngineId">Catapult engine id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task Suspend(int catapultEngineId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Reactivate a suspended engine
        /// </summary>
        /// <param name="catapultEngineId">Catapult engine id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task Reactivate(int catapultEngineId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get catapult engine list
        /// </summary>
        /// <param name="status">Filter the catapult engine by status (all | active | suspended | running)</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task<List<CatapultEngine>> GetCatapultEngines(string status, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Update Engine's Last Seen value
        /// </summary>
        /// <param name="engineName">Name of the engine</param>
        /// <param name="version">Version of the engine</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task UpdateLastSeen(string engineName, string version, CancellationToken cancellationToken = default(CancellationToken));
    }
}
