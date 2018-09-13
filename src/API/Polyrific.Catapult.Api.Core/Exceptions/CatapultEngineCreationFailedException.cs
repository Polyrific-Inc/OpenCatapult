// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Api.Core.Exceptions
{
    public class CatapultEngineCreationFailedException : Exception
    {
        public string Name { get; set; }

        public CatapultEngineCreationFailedException(string name)
            : base($"Catapult engine \"{name}\" was failed to create.")
        {
            Name = name;
        }

        public CatapultEngineCreationFailedException(string name, Exception ex)
            : base($"Catapult engine \"{name}\" was failed to create.", ex)
        {
            Name = name;
        }
    }
}
