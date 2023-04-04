using Phoenix.NET.Server.Nodes;
using Phoenix.NET.Server.PubSub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Phoenix.NET.Server
{
    /// <summary>
    /// Network PhoenixServer.
    /// </summary>
    public class PhoenixServer
    {
        #region Properties
        /// <summary>
        /// Guid of this server.
        /// </summary>
        public Guid Id => Guid.NewGuid();

        /// <summary>
        /// Indicates if this server is running.
        /// </summary>
        public bool IsRunning => _isRunning;

        /// <summary>
        /// The address on which this server is currently running.
        /// </summary>
        public IPAddress Address { get; private set; }

        /// <summary>
        /// The port on which this server is currently running.
        /// </summary>
        public int? Port { get; private set; }
        #endregion

        #region Events
        /// <summary>
        /// Triggered when an exception occurs while the server is running.
        /// </summary>
        public event ServerExceptionHandler OnServerException;

        /// <summary>
        /// Delegate for the OnServerException event.
        /// </summary>
        /// <param name="ex"></param>
        public delegate void ServerExceptionHandler(Exception ex);

        /// <summary>
        /// Triggered when the server started.
        /// </summary>
        public event VoidHandler OnStarted;

        /// <summary>
        /// Triggered when the server stopped.
        /// </summary>
        public event VoidHandler OnStopped;

        /// <summary>
        /// Delegate for the OnStarted and OnStopped events.
        /// </summary>
        public delegate void VoidHandler();
        #endregion

        private IPubSubRouter _pubSubRouter;
        private TcpListener _serverSocket;
        private volatile bool _isRunning;

        /// <summary>
        /// The thread that runs the cycle that accepts tcp connections.
        /// </summary>
        protected Thread _thread;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="pubSubRouter">An implementation of IPubSubRouter. If not specified, it will use an instance of PhoenixPubSubRouter.</param>
        public PhoenixServer(IPubSubRouter pubSubRouter = null)
        {
            _pubSubRouter = pubSubRouter ?? new PhoenixPubSubRouter();
        }

        #region Public
        /// <summary>
        /// Returns an PhoenixServerConfig with the reference to the IPubSubRouter used by this server.
        /// </summary>
        /// <returns></returns>
        public PhoenixServerConfig GetServerConfig() => new PhoenixServerConfig(_pubSubRouter);

        /// <summary>
        /// Starts the server and asynchronously runs the cycle that accepts tcp connections.
        /// </summary>
        /// <param name="port">The listening tcp port.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Start(int port) => Start(IPAddress.Any, port);

        /// <summary>
        /// Starts the server and asynchronously runs the cycle that accepts tcp connections.
        /// </summary>
        /// <param name="address">The tcp address for this server.</param>
        /// <param name="port">The listening tcp port.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Start(IPAddress address, int port)
        {
            if (IsRunning)
                return;

            _isRunning = true;
            Address = address;
            Port = port;
            _serverSocket = new TcpListener(Address, Port.Value);
            _serverSocket.Start();

            _thread = new Thread(Run);
            _thread.Start();
            // Loop until worker thread activates.
            while (!_thread.IsAlive) ;

            OnStarted?.BeginInvoke(null, null);
        }

        /// <summary>
        /// Stops the server
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Stop()
        {
            if (!IsRunning)
                return;

            _isRunning = false;
            Address = null;
            Port = null;
            _serverSocket?.Stop();
            _pubSubRouter.Dispose();
            _thread.Join();
            _thread = null;
            OnStopped?.BeginInvoke(null, null);
        }
        #endregion

        private void Run()
        {
            try
            {
                while (_isRunning)
                {
                    TcpClient clientSocket = _serverSocket.AcceptTcpClient();
                    var remote = new PhoenixRemoteClientNode(clientSocket);
                    remote.Connect(GetServerConfig());
                }
            }
            catch (Exception ex)
            {
                if (_isRunning)
                    OnServerException?.BeginInvoke(ex, null, null);
            }
            finally
            {
                if (_isRunning)
                    Stop();
            }
        }
    }

}
