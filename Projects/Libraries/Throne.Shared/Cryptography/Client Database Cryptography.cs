using Throne.Framework.Utilities;

namespace Throne.Framework.Cryptography
{
    /// <summary>
    ///     A really old class (that's been passed around for ages) used to decrypt the Conquer Online .dat files
    /// </summary>
    public class DatCryptography : IFileCryptography
    {
        private static readonly byte[] key =
        {
            0xAD, 0x6B, 0x4F, 0xFB, 0xDD, 0xB8, 0x0E, 0x09, 0x13, 0x33, 0x8F, 0xF5, 0x43, 0x09, 0x15, 0x88, 0x5D, 0x80,
            0xA3, 0x45, 0x2D, 0x42, 0x08, 0x56, 0x80, 0xF8, 0x19, 0xC5, 0x88, 0x1B, 0x3E, 0xEF, 0x81, 0x07, 0x30, 0x36,
            0x95, 0x52, 0x00, 0xF7, 0xFD, 0x5B, 0x5C, 0xBC, 0x6A, 0x26, 0x0E, 0xB2, 0xA3, 0x67, 0xC5, 0x5D, 0x6F, 0xDC,
            0x18, 0x8A, 0xB5, 0xE0, 0xC8, 0x85, 0xE2, 0x3E, 0x45, 0x8D, 0x8B, 0x43, 0x74, 0x85, 0x54, 0x17, 0xB0, 0xEC,
            0x10, 0x4D, 0x0F, 0x0F, 0x29, 0xB8, 0xE6, 0x7D, 0x42, 0x80, 0x8F, 0xBC, 0x1C, 0x76, 0x69, 0x3A, 0xB6, 0xA5,
            0x21, 0x86, 0xB9, 0x29, 0x30, 0xC0, 0x12, 0x45, 0xA5, 0x4F, 0xE1, 0xAF, 0x25, 0xD1, 0x92, 0x2E, 0x30, 0x58,
            0x49, 0x67, 0xA5, 0xD3, 0x84, 0xF4, 0x89, 0xCA, 0xFC, 0xB7, 0x04, 0x4F, 0xCC, 0x6E, 0xAC, 0x31, 0xD4, 0x87,
            0x07, 0x72
        };

        public static byte[] GenerateKey(int seed)
        {
            var key = new byte[128];

            var r = new rand(seed);

            for (int i = 0; i < 128; i++)
                key[i] = (byte) (r.Next()%256);

            return key;
        }

        public static void Decrypt(byte[] bytes, byte[] key)
        {
            if (bytes.Length < 0)
            {
                return;
            }
            int i = 0;
            int t1 = 0;
            int t2;
            while (i < bytes.Length)
            {
                t1 = (bytes[i] ^ key[i%0x80]) & 0xff;
                t2 = i%0x8;
                bytes[i] = (byte) (((t1 << 0x8 - t2)) | (t1 >> t2));
                i++;
            }
        }

        public static void Decrypt(byte[] bytes)
        {
            if (bytes.Length < 0)
            {
                return;
            }
            int i = 0;
            int t1 = 0;
            int t2;
            while (i < bytes.Length)
            {
                t1 = (bytes[i] ^ key[i%0x80]) & 0xff;
                t2 = i%0x8;
                bytes[i] = (byte) (((t1 << 0x8 - t2)) | (t1 >> t2));
                i++;
            }
        }

        public static void Encrypt(byte[] bytes)
        {
            if (bytes.Length < 0)
            {
                return;
            }
            int i = 0;
            byte t1 = 0;
            int t2;
            while (i < bytes.Length)
            {
                t2 = i%0x8;
                t1 = (byte) ((bytes[i] >> (0x8 - t2)) + (bytes[i] << t2));
                bytes[i] = (byte) (t1 ^ key[i%0x80]);
                i++;
            }
        }

        void IFileCryptography.Decrypt(byte[] array)
        {
            Decrypt(array);
        }
    }
}