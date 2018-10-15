// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace Polyrific.Catapult.Engine.Core.JobLogger
{
    internal class JobLoggerScope : IDisposable
    {
        private readonly JobLoggerProvider _provider;
        private readonly IDisposable _chainedDisposable;

        // An optimization only, no problem if there are data races on this.
        private bool _disposed;
        
        public object State { get; private set; }

        public JobLoggerScope(JobLoggerProvider provider, object state, IDisposable chainedDisposable = null)
        {
            _provider = provider;
            State = state;

            Parent = _provider.CurrentScope;
            _provider.CurrentScope = this;
            _chainedDisposable = chainedDisposable;
        }

        public JobLoggerScope Parent { get; }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                // In case one of the parent scopes has been disposed out-of-order, don't
                // just blindly reinstate our own parent.
                for (var scan = _provider.CurrentScope; scan != null; scan = scan.Parent)
                {
                    if (ReferenceEquals(scan, this))
                        _provider.CurrentScope = Parent;
                }

                _chainedDisposable?.Dispose();
            }
        }
    }
}
