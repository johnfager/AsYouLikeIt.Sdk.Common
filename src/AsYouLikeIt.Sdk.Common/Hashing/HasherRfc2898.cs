
namespace AsYouLikeIt.Sdk.Common.Hashing
{
    using System;
    using System.Security.Cryptography;

    /// <summary>
    /// PBKDF2 with HMAC-SHA1, 128-bit salt, 256-bit subkey, 1000 iterations. (See also: SDL crypto guidelines v5.1, Part III); Format: { 0x00, salt, subkey }
    /// </summary>
    public static class HasherRfc2898
    {
        private const int PBKDF2SubkeyLength = 256 / 8; // 256 bits
        private const int SaltSize = 128 / 8; // 128 bits

        /// <summary>
        /// Returns an RFC 2898 hash value for the specified password.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string HashValue(string input, int interations = 10000)
        {
            byte[] salt;
            byte[] buffer2;
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            using (var bytes = new Rfc2898DeriveBytes(input, SaltSize, interations))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(PBKDF2SubkeyLength);
            }
            byte[] dst = new byte[PBKDF2SubkeyLength + SaltSize + 1];
            Buffer.BlockCopy(salt, 0, dst, 1, SaltSize);
            Buffer.BlockCopy(buffer2, 0, dst, 1 + SaltSize, 32);
            return Convert.ToBase64String(dst);
        }

        // hashedPassword must be of the format of HashWithPassword (salt + Hash(salt+input))
        public static bool VerifyHashedValues(string hashedValue, string input, int interations = 10000)
        {
            if (hashedValue == null)
            {
                throw new ArgumentNullException("hashedValue");
            }
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            var hashedPasswordBytes = Convert.FromBase64String(hashedValue);

            // Verify a version 0 (see comment above) password hash.

            if (hashedPasswordBytes.Length != (1 + SaltSize + PBKDF2SubkeyLength) || hashedPasswordBytes[0] != 0x00)
            {
                // Wrong length or version header.
                return false;
            }

            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(hashedPasswordBytes, 1, salt, 0, SaltSize);
            byte[] storedSubkey = new byte[PBKDF2SubkeyLength];
            Buffer.BlockCopy(hashedPasswordBytes, 1 + SaltSize, storedSubkey, 0, PBKDF2SubkeyLength);

            byte[] generatedSubkey;
            using (var deriveBytes = new Rfc2898DeriveBytes(input, salt, interations))
            {
                generatedSubkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
            }
            return ByteArraysEqual(storedSubkey, generatedSubkey);
        }


        // Compares two byte arrays for equality. The method is specifically written so that the loop is not optimized.
        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }

            bool areSame = true;
            for (int i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }

    }
}
