using System;
using System.Collections.Generic;
using System.Text;

namespace OATH.Net
{  
   
    /// <summary>
   ///     Represents a class which can compute a cryptographic HMAC digest.
   /// </summary>
    public interface IHMACAlgorithm
    {
        /// <summary>
        ///     Computes a HMAC digest.
        /// </summary>
        /// <param name="key">The key to use.</param>
        /// <param name="buffer">The text to hash.</param>
        /// <returns>The HMAC digest.</returns>
        byte[] ComputeHash(byte[] key, byte[] buffer);
    }
}
