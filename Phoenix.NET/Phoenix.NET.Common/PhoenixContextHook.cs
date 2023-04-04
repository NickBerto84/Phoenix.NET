﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.NET.Common
{
    /// <summary>
    /// Class used to inform a content handler about the context of the operation
    /// (e.g.: content is null because peer is disconnected or because
    /// the node is unsubscribing from the channel).
    /// </summary>
    public sealed class PhoenixContextHook
    {
        /// <summary>
        /// Indicates if the node is unsubscribing from the channel.
        /// </summary>
        public bool Unsubscribing { get; internal set; }

        /// <summary>
        /// Indicates the target channel of the current context.
        /// </summary>
        public string TargetChannel { get; internal set; }

        /// <summary>
        /// Indicates the content's publication datetime.
        /// </summary>
        public DateTime PublicationDateTime { get; internal set; }
    }
}
