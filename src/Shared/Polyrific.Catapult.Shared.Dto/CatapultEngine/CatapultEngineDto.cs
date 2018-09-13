// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Shared.Dto.CatapultEngine
{
    public class CatapultEngineDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? LastSeen { get; set; }
    }
}