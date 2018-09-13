// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class CatapultEngineDeletionFailedException : Exception
    {
        public int CatapultEngineId { get; set; }

        public CatapultEngineDeletionFailedException(int catapultEngineId)
            : base($"Catapult engine \"{catapultEngineId}\" was failed to delete.")
        {
            CatapultEngineId = catapultEngineId;
        }

        public CatapultEngineDeletionFailedException(int catapultEngineId, Exception ex)
            : base($"Catapult engine \"{catapultEngineId}\" was failed to delete.", ex)
        {
            CatapultEngineId = catapultEngineId;
        }
    }
}
