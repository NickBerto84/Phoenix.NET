using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.NET.Common.Packets
{
    /// <summary>
    /// Class for internal/logical error within the library.
    /// </summary>
    [Serializable]
    public class PhoenixError : PhoenixPacket
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="publisherId">Guid of the client who sent this packet.</param>
        internal PhoenixError(Guid publisherId) : base(publisherId) { }

        /// <summary>
        /// The exception that caused this packet.
        /// </summary>
        public Exception Exception { get; internal set; }

        /// <summary>
        /// A log string.
        /// </summary>
        public string Log { get; internal set; }
    }
}
