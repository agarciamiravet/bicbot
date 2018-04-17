namespace OATH.Net
{
    using System.Security.Cryptography;

    /// <summary>
    ///     Represents a secret key used for the one-time password generation.
    /// </summary>
    public sealed class Key
    {
        /// <summary>
        ///     Initializes a new instance of the Key class and generates a random 20-byte key.
        /// </summary>
        public Key()
            : this(20)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the Key class and generates a random key with the specified seed.
        /// </summary>
        /// <param name="keyLength">Length in bytes of the generated key.</param>
        public Key(int keyLength)
        {
            this.Binary = new byte[keyLength];
            
            //var gen = new RNGCryptoServiceProvider();
            //gen.GetBytes(this.Binary)

            var gen = RandomNumberGenerator.Create();
            gen.GetBytes(this.Binary);
        }

        /// <summary>
        ///     Initializes a new instance of the Key class.
        /// </summary>
        /// <param name="keyData">The key to initialize.</param>
        public Key(byte[] keyData)
        {
            this.Binary = keyData;
        }

        /// <summary>
        ///     Initializes a new instance of the Key class.
        /// </summary>
        /// <param name="base32key">The key to initialize.</param>
        /// <exception cref="ArgumentException">base32key is not a valid base32-encoded string.</exception>
        public Key(string base32key)
        {
            this.Binary = OATH.Net.Base32.ToBinary(base32key);
        }

        /// <summary>
        ///     Gets the key represented as a byte array.
        /// </summary>
        public byte[] Binary { get; private set; }

        /// <summary>
        ///     Gets the key represented as base32-encoded string.
        /// </summary>
        public string Base32
        {
            get
            {
                return OATH.Net.Base32.ToBase32(this.Binary);
            }
        }
    }
}
