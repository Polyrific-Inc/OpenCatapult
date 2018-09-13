// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Shared.Dto.CatapultEngine
{
    public class RegisterCatapultEngineDto
    {
        /// <summary>
        /// The engine name
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}