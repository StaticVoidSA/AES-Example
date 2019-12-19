using System;
using System.IO;
using System.Security.Cryptography;

namespace ConsoleAES
{
    class Program
    {
        static void DumpBytes(string title, byte[] bytes)
        {
            Console.Write(title);

            foreach (byte b in bytes)
            {
                Console.Write("{0:X} ", b);
            }

            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            // String to be encrypted
            string plainText = "This is my super duper secret data...";

            // byte array to hold encrypted message
            byte[] cipherText;

            // byte array to hold the key used for encryption
            byte[] key;

            // byte array to hold initialization vector used for encryption
            byte[] initializationVector;

            // Create an AES instance
            // Creates a random key and initialization vector
            using (Aes aes = Aes.Create())
            {
                // Copy the key and the initialization vector
                key = aes.Key;
                initializationVector = aes.IV;

                // Create an encryptor to encrypt some data
                // should be wrapped in a using for production code
                ICryptoTransform encryptor = aes.CreateEncryptor();

                // Create a new memory stream to recieve the encrypted data
                using (MemoryStream encryptMemoryStream = new MemoryStream())
                {
                    // Create a CryptoStream, give it the stream to write to and
                    // the encryptor to use
                    using (CryptoStream encryptCryptoStream = new CryptoStream(encryptMemoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        // Make a stream writer from the cryptostream
                        using (StreamWriter swEncrypt = new StreamWriter(encryptCryptoStream))
                        {
                            // Write the secret message to the stream
                            swEncrypt.Write(plainText);
                        }

                        // Get the encrypted message from the stream
                        cipherText = encryptMemoryStream.ToArray();
                    }
                }
            }

            // Dump out data
            Console.WriteLine("String to encrypt: {0}", plainText);
            DumpBytes("Key: ", key);
            DumpBytes("Initialization Vector: ", initializationVector);
            DumpBytes("Encrypted: ", cipherText);

            Console.ReadLine();
        }
    }
}
