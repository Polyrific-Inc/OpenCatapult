// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.HelpContext;

namespace Polyrific.Catapult.Shared.Service
{
    public interface IHelpContextService
    {
        /// <summary>
        /// Get the help contexts by the secton
        /// </summary>
        /// <param name="section">Section of the help context</param>
        /// <returns></returns>
        Task<List<HelpContextDto>> GetHelpContextsBySection(string section);
    }
}
