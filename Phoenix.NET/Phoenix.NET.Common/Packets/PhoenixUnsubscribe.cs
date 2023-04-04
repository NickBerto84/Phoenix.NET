using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.NET.Common.Packets
{
    /// <summary>
    /// Class for requesting to unsubscribe from a channel.
    /// </summary>
    [Serializable]
    public class PhoenixUnsubscribe : PhoenixPacket
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="publisherId">Guid of the client who sent this packet.</param>
        /// <param name="channel">The channel from which unsubscribe.</param>
        public PhoenixUnsubscribe(Guid publisherId, string channel) : base(publisherId)
        {
            Channel = channel;
            IsClientSubmitted = true;
        }

        /// <summary>
        /// The channel from which unsubscribe.
        /// </summary>
        public string Channel { get; }
    }
}
