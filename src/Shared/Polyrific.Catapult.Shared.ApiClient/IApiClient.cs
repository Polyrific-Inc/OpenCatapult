// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;

namespace Polyrific.Catapult.Shared.ApiClient
{
    public interface IApiClient
    {
        /// <summary>
        /// Send GET request
        /// </summary>
        /// <typeparam name="TResult">Type of the object to return</typeparam>
        /// <param name="path">Uri path</param>
        /// <returns></returns>
        Task<TResult> Get<TResult>(string path);

        /// <summary>
        /// Send HEAD request
        /// </summary>
        /// <param name="path">Uri path</param>
        /// <returns></returns>
        Task<bool> Head(string path);

        /// <summary>
        /// Send POST request
        /// </summary>
        /// <typeparam name="TContent">Type of the object to send</typeparam>
        /// <typeparam name="TResult">Type of the object to return</typeparam>
        /// <param name="path">Uri path</param>
        /// <param name="content">Object to send</param>
        /// <returns></returns>
        Task<TResult> Post<TContent, TResult>(string path, TContent content);

        /// <summary>
        /// Send POST request and receive plain text
        /// </summary>
        /// <typeparam name="TContent">Type of the object to send</typeparam>
        /// <param name="path">Uri path</param>
        /// <param name="content">Object to send</param>
        /// <returns></returns>
        Task<string> Post<TContent>(string path, TContent content);

        /// <summary>
        /// Send POST request with file
        /// </summary>
        /// <typeparam name="TResult">Type of the object to return</typeparam>
        /// <param name="path">Uri path</param>
        /// <param name="fileName">Name of the file</param>
        /// <param name="file">Content of the file</param>
        /// <returns></returns>
        Task<TResult> PostFile<TResult>(string path, string fileName, byte[] file);

        /// <summary>
        /// Send PUT request
        /// </summary>
        /// <typeparam name="TContent">Type of the object to send</typeparam>
        /// <param name="path">Uri path</param>
        /// <param name="content">Object to send</param>
        /// <returns></returns>
        Task<bool> Put<TContent>(string path, TContent content);

        /// <summary>
        /// Send PUT request with file
        /// </summary>
        /// <param name="path">Uri path</param>
        /// <param name="fileName">Name of the file</param>
        /// <param name="file">Content of the file</param>
        /// <returns></returns>
        Task<bool> PutFile(string path, string fileName, byte[] file);

        /// <summary>
        /// Send DELETE request
        /// </summary>
        /// <param name="path">Uri path</param>
        /// <returns></returns>
        Task<bool> Delete(string path);
    }
}
