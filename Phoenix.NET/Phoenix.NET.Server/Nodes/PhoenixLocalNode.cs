using Phoenix.NET.Common.Nodes;
using Phoenix.NET.Common.Packets;
using Phoenix.NET.Server.PubSub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.NET.Server.Nodes
{
    /// <summary>
    /// Local client node.
    /// </summary>
    public class PhoenixLocalNode : AbstractPhoenixNode<PhoenixServerConfig>, IMessageSubscriber
    {
        private IPubSubRouter _pubSubRouter;

        private bool _isConnected;
        /// <summary>
        /// Indicates if this node is connected.
        /// </summary>
        public override bool IsConnected => _isConnected;

        /// <summary>
        /// Invoked when the node is connecting.
        /// </summary>
        /// <param name="config">The connection's configuration.</param>
        /// <returns>An AbstractPhoenixListener instance.</returns>
        protected override void OnConnect(PhoenixServerConfig config)
        {
            _pubSubRouter = _config.PubSubRouter;
            _pubSubRouter.Register(this);
            _isConnected = true;
        }

        /// <summary>
        /// Invoked when the node is disposing.
        /// </summary>
        protected override void OnDispose()
        {
            _isConnected = false;
            _pubSubRouter.Unregister(this);
            _pubSubRouter = null;
        }

        /// <summary>
        /// Handler for a meta packet received from the network.
        /// </summary>
        /// <param name="meta">The PhoenixMeta received.</param>
        protected override void OnMetaReceived(PhoenixMeta meta)
        {
        }

        /// <summary>
        /// Sends the packet to the network.
        /// </summary>
        /// <param name="packet">The packet to send.</param>
        protected override Task<bool> Publish(PhoenixPacket packet)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    if (packet is PhoenixSubscribe)
                    {
                        var subscribeCommand = packet as PhoenixSubscribe;
                        _pubSubRouter.Subscribe(this, subscribeCommand.Channel);
                    }
                    else if (packet is PhoenixUnsubscribe)
                    {
                        var unsubscribeCommand = packet as PhoenixUnsubscribe;
                        _pubSubRouter.Unsubscribe(this, unsubscribeCommand.Channel);
                    }
                    else
                    {
                        var message = packet as PhoenixMessage;
                        _pubSubRouter.SubmitMessage(this, message);
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                    return false;
                }
            });
        }

        /// <summary>
        /// Invoked when a new message is sent to this subscriber.
        /// </summary>
        /// <param name="message">The message sent.</param>
        public void ReceiveMessage(PhoenixMessage message) => OnClientSubmittedPacketReceived(message);
    }

}
