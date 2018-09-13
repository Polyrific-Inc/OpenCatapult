using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Shared.Common.Interface
{
    public interface ISecretVault
    {
        /// <summary>
        /// Add a secret
        /// </summary>
        /// <param name="name">Name of the secret</param>
        /// <param name="value">The secret</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task Add(string name, string value, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Update a secret
        /// </summary>
        /// <param name="name">Name of the secret</param>
        /// <param name="value">The updated secret</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task Update(string name, string value, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a secret
        /// </summary>
        /// <param name="name">Name of the secret</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The secret value</returns>
        Task<string> Get(string name, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Delete a secret
        /// </summary>
        /// <param name="name">Name of the secret</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task Delete(string name, CancellationToken cancellationToken = default(CancellationToken));
    }
}
