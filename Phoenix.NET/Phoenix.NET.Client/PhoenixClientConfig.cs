using Phoenix.NET.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.NET.Client
{
    /// <summary>
    /// Client node configuration.
    /// </summary>
    public class PhoenixClientConfig : PhoenixBaseConfig
    {
        /// <summary>
        /// Server's hostname.
        /// </summary>
        public string Hostname { get; set; }

        /// <summary>
        /// Server's port.
        /// </summary>
        public int Port { get; set; }
    }
}
