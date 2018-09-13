// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Shared.Dto.CatapultEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Shared.Service
{
    public interface ICatapultEngineService
    {
        /// <summary>
        /// Register an engine
        /// </summary>
        /// <param name="dto">DTO containing the engine details</param>
        /// <returns>DTO containing registration result</returns>
        Task<RegisterCatapultEngineResponseDto> RegisterEngine(RegisterCatapultEngineDto dto);

        /// <summary>
        /// Suspend an engine
        /// </summary>
        /// <param name="engineId">Id of the engine</param>
        /// <returns></returns>
        Task Suspend(int engineId);

        /// <summary>
        /// Reactivate a suspended engine
        /// </summary>
        /// <param name="engineId">Id of the engine</param>
        /// <returns></returns>
        Task Reactivate(int engineId);

        /// <summary>
        /// Get an engine by name
        /// </summary>
        /// <param name="name">Name of the engine</param>
        /// <returns></returns>
        Task<CatapultEngineDto> GetCatapultEngineByName(string name);

        /// <summary>
        /// Get list of engines filtered by the status
        /// </summary>
        /// <param name="status">Status of the engine (all | active | suspended | running)</param>
        /// <returns></returns>
        Task<List<CatapultEngineDto>> GetCatapultEngines(string status);

        Task RemoveCatapultEngine(int engineId);
    }
}
