using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polyrific.Catapult.TaskProviders.AzureAppService
{
    internal class ProgressableStreamContent : HttpContent
    {
        private const int DefaultBufferSize = 4096;

        private readonly int bufferSize;
        private readonly ILogger _logger;

        private Stream content;
        private bool contentConsumed;

        public ProgressableStreamContent(Stream content, ILogger logger) : this(content, DefaultBufferSize, logger) { }

        public ProgressableStreamContent(Stream content, int bufferSize, ILogger logger)
        {
            if (bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException("bufferSize");
            }

            this.content = content ?? throw new ArgumentNullException("content");
            this.bufferSize = bufferSize;
            this._logger = logger;
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            Contract.Assert(stream != null);

            PrepareContent();
            
            var buffer = new Byte[this.bufferSize];
            var size = content.Length;
            var uploaded = 0;
            double percent = 0;

            _logger.LogInformation("Begin uploading");

            using (content)
            {
                while (true)
                {
                    var length = content.Read(buffer, 0, buffer.Length);
                    if (length <= 0)
                        break;

                    uploaded += length;
                    percent = ((double)uploaded / size) * 100;
                    _logger.LogInformation("{percent:0}% uploaded", percent);

                    await stream.WriteAsync(buffer, 0, length);
                }
            }
        }

        protected override bool TryComputeLength(out long length)
        {
            length = content.Length;
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                content.Dispose();
            }
            base.Dispose(disposing);
        }


        private void PrepareContent()
        {
            if (contentConsumed)
            {
                // If the content needs to be written to a target stream a 2nd time, then the stream must support
                // seeking (e.g. a FileStream), otherwise the stream can't be copied a second time to a target 
                // stream (e.g. a NetworkStream).
                if (content.CanSeek)
                {
                    content.Position = 0;
                }
                else
                {
                    throw new InvalidOperationException("SR.net_http_content_stream_already_read");
                }
            }

            contentConsumed = true;
        }
    }
}
