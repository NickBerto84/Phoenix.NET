using Phoenix.NET.Common.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.NET.Common
{
    /// <summary>
    /// Interface for a handler that when disposed unsubscribes the content handler from the channel.
    /// </summary>
    public interface IDisposableSubscription : IDisposable
    {
        /// <summary>
        /// The channel from which unsubscribe.
        /// </summary>
        string Channel { get; }

        /// <summary>
        /// The content handler to unsubscribe.
        /// </summary>
        ContentHandler ContentHandler { get; }
    }

    /// <summary>
    /// An implementation of IDisposableSubscription.
    /// </summary>
    class PhoenixDisposableSubscription : IDisposableSubscription
    {
        /// <summary>
        /// Indicates if this subscription is already disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// The channel from which unsubscribe.
        /// </summary>
        public string Channel { get; }

        /// <summary>
        /// The content handler to unsubscribe.
        /// </summary>
        public ContentHandler ContentHandler { get; private set; }

        private IPhoenixNode _phoenixNode;

        /// <summary>
        /// Create an already disposed subscription.
        /// </summary>
        internal PhoenixDisposableSubscription()
        {
            IsDisposed = true;
        }

        public PhoenixDisposableSubscription(IPhoenixNode phoenixNode, string channel, ContentHandler contentHandler)
        {
            _phoenixNode = phoenixNode;
            Channel = channel;
            ContentHandler = contentHandler;
            IsDisposed = false;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Dispose()
        {
            if (!IsDisposed)
            {
                if (Channel == null)
                    _phoenixNode?.UnsubscribeFromBroadcast(ContentHandler);
                else
                    _phoenixNode?.Unsubscribe(Channel, ContentHandler);

                _phoenixNode = null;
                this.ContentHandler = null;
                IsDisposed = true;
            }
        }
    }
}
