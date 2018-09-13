// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Core.Services
{
    public interface IJobCounterService
    {
        /// <summary>
        /// Get the next job counter sequence
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The next job counter sequence</returns>
        Task<int> GetNextSequence(CancellationToken cancellationToken = default(CancellationToken));
    }
}