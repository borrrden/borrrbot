// 
//  ObsConnection.cs
// 
//  This is free and unencumbered software released into the public domain.
//
//  Anyone is free to copy, modify, publish, use, compile, sell, or
//  distribute this software, either in source code form or as a compiled
//  binary, for any purpose, commercial or non-commercial, and by any
//  means.
//
//  In jurisdictions that recognize copyright laws, the author or authors
//  of this software dedicate any and all copyright interest in the
//  software to the public domain. We make this dedication for the benefit
//  of the public at large and to the detriment of our heirs and
//  successors. We intend this dedication to be an overt act of
//  relinquishment in perpetuity of all present and future rights to this
//  software under copyright law.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//  EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//  MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//  IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//  OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//  ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//  OTHER DEALINGS IN THE SOFTWARE.
//
//  For more information, please refer to <http://unlicense.org/>
//

using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using borrrbot.API.OBS.AudioService;
using borrrbot.API.OBS.ScenesService;

using JetBrains.Annotations;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace borrrbot.API.OBS
{
    internal struct InFlightRequestToken
    {
        public IJsonRcRequest Request;

        public TaskCompletionSource<IJsonRcResponse> CompletionSource;
    }

    /// <summary>
    /// A class that will manage a connection to an OBS based piece of software
    /// (OBS Studio, SLOBS, obs.live, etc)
    /// </summary>
    public sealed class ObsConnection
    {
        #region Variables
        
        [NotNull] private readonly Dictionary<int, InFlightRequestToken> _inFlightRequests 
            = new Dictionary<int, InFlightRequestToken>();

        private bool _isUsable;
        private readonly ILogger<ObsConnection> _logger;
        [NotNull]private readonly NamedPipeClientStream _pipeline = new NamedPipeClientStream(".", "slobs", PipeDirection.InOut, PipeOptions.Asynchronous);
        private StreamReader _pipelineReader;
        [NotNull] private readonly ManualResetEvent _ready = new ManualResetEvent(false);

        #endregion

        #region Properties

        public bool IsUsable
        {
            get {
                _ready.WaitOne();
                return _isUsable;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with optional logging factory
        /// </summary>
        /// <param name="loggerFactory">The logging factory to use to create
        /// logging instances, or <c>null</c> for no logging</param>
        public ObsConnection(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory?.CreateLogger<ObsConnection>();
            _pipeline.ConnectAsync(2000).ContinueWith(t =>
            {
                if (!t.IsFaulted) {
                    _pipelineReader = new StreamReader(_pipeline);
                    ReadLoop();
                    _isUsable = true;
                }

                _ready.Set();
            });
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the audio service from the running instance of OBS.  This service
        /// can be used to query, set volume, etc on various audio components that
        /// OBS can access.
        /// </summary>
        /// <returns>The audio service for consumption</returns>
        [NotNull]
        public IAudioServiceApi GetAudioService() => new AudioServiceApi(this);

        /// <summary>
        /// Gets the scenes service from the running instance of OBS.  This service
        /// can be used to hide, show, move, etc various scene components that OBS
        /// is managing
        /// </summary>
        /// <returns>The scenes service for consumption</returns>
        [NotNull]
        public IScenesServiceApi GetScenesService() => new ScenesServiceApi(this);

        /// <summary>
        /// Sends a JSON RC request to the running OBS instance, and returns its
        /// response as type <c>T</c>
        /// </summary>
        /// <typeparam name="T">The type of the response from OBS</typeparam>
        /// <param name="request">The request to send to OBS</param>
        /// <returns>An awaitable task containing the response from OBS</returns>
        [NotNull]
        public async Task<T> SendRequestAsync<T>(IJsonRcRequest request)
        {
            var serialized = JsonConvert.SerializeObject(request, Formatting.None) + "\n";
            var bytes = Encoding.ASCII.GetBytes(serialized);
            var tcs = new TaskCompletionSource<IJsonRcResponse>();
            var token = new InFlightRequestToken
            {
                Request = request,
                CompletionSource = tcs
            };

            _inFlightRequests[request.id] = token;
            await  _pipeline.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
            await tcs.Task;

            return ((IJsonRcResponse<T>)tcs.Task.Result).result;
        }

        /// <summary>
        /// Sends a JSON RC request to the running OBS instance, and returns its
        /// response as type <c>T</c>
        /// </summary>
        /// <typeparam name="T">The type of the response from OBS</typeparam>
        /// <param name="name">The name of the method for the request</param>
        /// <param name="resource">The ID of the resource for the request</param>
        /// <param name="args">The arguments for the request</param>
        /// <returns>An awaitable task containing the response from OBS</returns>
        [NotNull]
        public Task<T> SendRequestAsync<T>(string name, string resource, params object[] args)
        {
            var request = new JsonRcRequest(name, typeof(JsonRcResponse<T>));
            request.AddParam("resource", resource);
            if (args.Length > 0) {
                request.AddParam("args", args);
            }

            return SendRequestAsync<T>(request);
        }

        /// <summary>
        /// Sends a JSON RC request to the running OBS instance, and asynchronously waits
        /// for it to finish
        /// </summary>
        /// <param name="request">The request to send to OBS</param>
        /// <returns>An awaitable task</returns>
        [NotNull]
        public async Task SendSimpleRequestAsync(IJsonRcRequest request)
        {
            var serialized = JsonConvert.SerializeObject(request, Formatting.None) + "\n";
            var bytes = Encoding.ASCII.GetBytes(serialized);
            var tcs = new TaskCompletionSource<IJsonRcResponse>();
            var token = new InFlightRequestToken
            {
                Request = request,
                CompletionSource = tcs
            };

            _inFlightRequests[request.id] = token;
            await  _pipeline.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
            await tcs.Task;
        }

        /// <summary>
        /// Sends a JSON RC request to the running OBS instance, and asynchronously waits
        /// for it to finish
        /// </summary>
        /// <param name="name">The name of the method for the request</param>
        /// <param name="resource">The ID of the resource for the request</param>
        /// <param name="args">The arguments for the request</param>
        /// <returns>An awaitable task</returns>
        [NotNull]
        public Task SendSimpleRequestAsync(string name, string resource, params object[] args)
        {
            var request = new JsonRcRequest(name, typeof(JsonRcResponse));
            request.AddParam("resource", resource);
            if (args.Length > 0) {
                request.AddParam("args", args);
            }

            return SendSimpleRequestAsync(request);
        }

        #endregion

        #region Private Methods

        private async void ReadLoop()
        {
            while (true) {
                var line = await _pipelineReader.ReadLineAsync().ConfigureAwait(false);
                var intermediate = JToken.Parse(line);
                var id = intermediate.SelectToken("id").ToObject<int>();
                if (!_inFlightRequests.TryGetValue(id, out var token)) {
                    _logger?.LogWarning($"Unexpected response received ({id})");
                    continue;
                }

                var serializer = JsonSerializer.CreateDefault();
                serializer.Converters.Add(new SceneNodeApiConverter());
                var responseType = token.Request.ResponseType();
                var response = intermediate.ToObject(responseType, serializer) as IJsonRcResponse;
                if (response.error != null) {
                    token.CompletionSource.SetException(new ObsException(response.error));
                } else {
                    token.CompletionSource.SetResult(response);
                }

                _inFlightRequests.Remove(id);
            }
        }

        #endregion
    }
}
