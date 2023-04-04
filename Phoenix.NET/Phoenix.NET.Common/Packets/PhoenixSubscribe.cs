using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.NET.Common.Packets
{
    /// <summary>
    /// Class for requesting to subscribe to a channel.
    /// </summary>
    [Serializable]
    public sealed class PhoenixSubscribe : PhoenixPacket
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="publisherId">Guid of the client who sent this packet.</param>
        /// <param name="channel">The channel to which subscribe.</param>
        public PhoenixSubscribe(Guid publisherId, string channel) : base(publisherId)
        {
            Channel = channel;
            IsClientSubmitted = true;
        }

        /// <summary>
        /// The channel to which subscribe.
        /// </summary>
        public string Channel { get; }
    }
}
