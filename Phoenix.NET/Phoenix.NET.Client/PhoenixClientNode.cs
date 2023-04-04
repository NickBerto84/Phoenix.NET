﻿using Phoenix.NET.Common.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.NET.Client
{
    /// <summary>
    /// Network client node.
    /// </summary>
    public class PhoenixClientNode : AbstractPhoenixNetworkNode<PhoenixClientConfig>
    {
        #region Properties
        /// <summary>
        /// Indicates if this node is connected.
        /// </summary>
        public override bool IsConnected => _socket?.Connected ?? false;
        #endregion

        /// <summary>
        /// TCP client used to communicate with the remote phoenix network.
        /// </summary>
        protected TcpClient _socket;

        /// <summary>
        /// Network IO stream.
        /// </summary>
        protected volatile NetworkStream _networkStream;

        /// <summary>
        /// Invoked when the node is connecting.
        /// </summary>
        /// <returns>The network stream used to communicate to the pubsub network.</returns>
        protected override NetworkStream GetNetworkStream() => _socket?.GetStream();

        /// <summary>
        /// Invoked when the node is connecting.
        /// </summary>
        /// <param name="config">The connection's configuration.</param>
        protected override void OnConnect(PhoenixClientConfig config)
        {
            _socket = new TcpClient(config.Hostname, config.Port);
            base.OnConnect(config);
        }

        /// <summary>
        /// Handler for null data received from the PhoenixListener.
        /// Disposes the node if the connection is down.
        /// Fires a log event if LogNullsEnable is true.
        /// </summary>
        protected override async void OnNullReceived()
        {
            if (!await IsPeerAlive())
                Dispose();
            else
                _lastException = null;
        }

        /// <summary>
        /// Invoked when the node is disposing.
        /// </summary>
        protected override void OnDispose()
        {
            base.OnDispose();
            _socket?.Close();
        }
    }

}
