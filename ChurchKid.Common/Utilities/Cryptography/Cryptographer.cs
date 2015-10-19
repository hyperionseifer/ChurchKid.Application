using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace ChurchKid.Common.Utilities.Cryptography
{
    public abstract class Cryptographer : ConfigurationBased
    {

        private static void SetConfigurationPath()
        {
            var uriAssemblyFolder = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase));
            var appPath = uriAssemblyFolder.LocalPath;
            var properPath = string.Format("{0}\\{1}.dll", appPath, Assembly.GetExecutingAssembly().GetName().Name);

            if (!ConfigurationPath.Equals(properPath))
                ConfigurationPath = properPath;
        }

        public static string EncryptionKey
        {
            get
            {
                SetConfigurationPath();
                return AppSettingsConfiguration.Settings["Cryptographer.EncryptionKey"].Value;
            }
        }

        /// <summary>
        /// Deciphers the specified encrypted text using the supplied encryption key in the application's config file (Web.config).
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <returns></returns>
        public static string Decrypt(string encryptedText)
        { return Decrypt(encryptedText, EncryptionKey); }

        /// <summary>
        /// Deciphers the specified encrypted text using the supplied encryption key.
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Decrypt(string encryptedText, string key)
        {
            string decryptedText = encryptedText;

            MD5Hash md5 = new MD5Hash(key); TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();

            try
            {
                des.Key = md5.Hash; des.Mode = CipherMode.ECB;
                byte[] bytes = Convert.FromBase64String(encryptedText);
                decryptedText = ASCIIEncoding.ASCII.GetString(des.CreateDecryptor().TransformFinalBlock(bytes, 0, bytes.Length));
            }
            catch { }
            finally
            {
                md5 = null; des.Clear(); des = null;
            }

            return decryptedText;
        }

        /// <summary>
        /// Encrypts the supplied human-readable plain text using the specified encryption key in the application's config file (Web.config). 
        /// </summary>
        /// <param name="plaintext"></param>
        /// <returns></returns>
        public static string Encrypt(string plaintext)
        { return Encrypt(plaintext, EncryptionKey); }

        /// <summary>
        /// Encrypts the supplied human-readable plain text using the specified encryption key.
        /// </summary>
        /// <param name="plaintext"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encrypt(string plaintext, string key)
        {
            string encryptedText = plaintext;

            MD5Hash md5 = new MD5Hash(key);
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();

            try
            {
                des.Key = md5.Hash; des.Mode = CipherMode.ECB;
                byte[] bytes = ASCIIEncoding.ASCII.GetBytes(plaintext);
                encryptedText = Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length));
            }
            catch { }
            finally
            { md5 = null; des.Clear(); des = null; }

            return encryptedText;
        }

    }
}
