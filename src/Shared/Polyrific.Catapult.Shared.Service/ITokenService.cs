// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.CatapultEngine;
using Polyrific.Catapult.Shared.Dto.User;

namespace Polyrific.Catapult.Shared.Service
{
    public interface ITokenService
    {
        /// <summary>
        /// Request authorization token
        /// </summary>
        /// <param name="dto">DTO containing details to request token</param>
        /// <returns>Authorization token</returns>
        Task<string> RequestToken(RequestTokenDto dto);

        /// <summary>
        /// Get a new authorization token
        /// </summary>
        /// <returns></returns>
        Task<string> RefreshToken();

        /// <summary>
        /// Request authorization token for an engine
        /// </summary>
        /// <param name="engineId">Id of the engine</param>
        /// <param name="dto">DTO containing details to request engine's token</param>
        /// <returns>Engine's authorization token</returns>
        Task<string> RequestEngineToken(int engineId, RequestEngineTokenDto dto);
    }
}
