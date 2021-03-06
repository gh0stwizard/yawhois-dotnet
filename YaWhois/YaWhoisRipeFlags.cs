using System;
using System.Collections.Generic;
using System.Text;

namespace YaWhois
{
    [Flags]
    public enum YaWhoisRipeFlags
    {
        /// <summary>
        /// Exact match.
        /// </summary>
        EXACT = 1,

        /// <summary>
        /// Return brief IP address ranges.
        /// </summary>
        BRIEF = 2,

        /// <summary>
        /// Request to append DNS reverse delegation objects.
        /// </summary>
        REVERSE_DOMAIN = 4,

        /// <summary>
        /// Turn off object filtering (show email addresses).
        /// </summary>
        NO_FILTERING = 8,

        /// <summary>
        /// Turn off grouping of associated objects.
        /// </summary>
        NO_GROUPING = 16,
    }
}
