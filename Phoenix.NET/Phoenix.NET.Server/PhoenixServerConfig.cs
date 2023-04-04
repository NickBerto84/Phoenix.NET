using Phoenix.NET.Common;
using Phoenix.NET.Server.PubSub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.NET.Server
{
    /// <summary>
    /// Server node configuration.
    /// </summary>
    public class PhoenixServerConfig : PhoenixBaseConfig
    {
        /// <summary>
        /// The IPubSubRouter instance to interact with.
        /// </summary>
        public IPubSubRouter PubSubRouter { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="pubSubRouter">An implementation of IPubSubRouter.</param>
        public PhoenixServerConfig(IPubSubRouter pubSubRouter)
        {
            PubSubRouter = pubSubRouter;
        }
    }
}
