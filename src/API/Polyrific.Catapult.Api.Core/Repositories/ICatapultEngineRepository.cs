// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Repositories
{
    public interface ICatapultEngineRepository : IRepository<CatapultEngine>
    {
        /// <summary>
        /// Generate token which is used to confirm registeration
        /// </summary>
        /// <param name="catapultEngineId">Id of the catapult engine</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Confirmation token</returns>
        Task<string> GenerateConfirmationToken(int catapultEngineId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Confirm a registered catapult engine
        /// </summary>
        /// <param name="catapultEngineId">Id of the catapult engine</param>
        /// <param name="token">Confirmation token</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task ConfirmRegistration(int catapultEngineId, string token, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a catapult engine from claims principal
        /// </summary>
        /// <param name="principal">Claims principal</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The catapult engine</returns>
        Task<CatapultEngine> GetByPrincipal(ClaimsPrincipal principal, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a catapult engine by its name
        /// </summary>
        /// <param name="engineName">The catapult engine name</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The catapult engine entity</returns>
        Task<CatapultEngine> GetByCatapultEngineName(string engineName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Validate catapultEnginename + password
        /// </summary>
        /// <param name="catapultEngineName">CatapultEnginename of the catapult engine</param>
        /// <param name="password">Password of the catapult engine</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Validity status</returns>
        Task<bool> ValidateCatapultEnginePassword(string catapultEngineName, string password, CancellationToken cancellationToken = default(CancellationToken));

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
        /// Get all registered catapult engine
        /// </summary>
        /// <param name="isActive">Filter the active/inactive engine</param>
        /// <param name="minLastSeen">Filter the engine LastSeen > minLastSeen</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The list of catapult engine</returns>
        Task<List<CatapultEngine>> GetAll(bool? isActive, System.DateTime? minLastSeen, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get catapult engine role by engineIdid
        /// </summary>
        /// <param name="catapultEngineId">Catapult engine id</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The catapult engine role</returns>
        Task<string> GetCatapultEngineRole(int catapultEngineId, CancellationToken cancellationToken = default(CancellationToken));
    }
}