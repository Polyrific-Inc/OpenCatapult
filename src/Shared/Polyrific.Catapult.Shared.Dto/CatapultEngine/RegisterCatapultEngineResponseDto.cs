// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Shared.Dto.CatapultEngine
{
    public class RegisterCatapultEngineResponseDto
    {
        /// <summary>
        /// Id of the engine
        /// </summary>
        public int EngineId { get; set; }

        /// <summary>
        /// Confirmation token
        /// </summary>
        public string ConfirmToken { get; set; }
    }
}
