// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Shared.Dto.CatapultEngine
{
    public class CatapultEngineDto
    {
        /// <summary>
        /// Id of the engine
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the engine
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Version of the engine
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// When the engine last invoking the API
        /// </summary>
        public DateTime? LastSeen { get; set; }
    }
}
