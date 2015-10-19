using System.Security.Cryptography;
using System.Text;

namespace ChurchKid.Common.Utilities.Cryptography
{
    public class MD5Hash
    {

        #region "variables"

        private MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

        private string value = "";

        #endregion

        #region "constructors"

        /// <summary>
        /// Creates a new instance of MD5Hash.
        /// </summary>
        public MD5Hash()
            : this(string.Empty)
        { }

        /// <summary>
        /// Creates a new instance of MD5Hash.
        /// </summary>
        /// <param name="value">Text to be evaluated for the hash creation</param>
        public MD5Hash(string value)
        { this.value = value; }

        #endregion

        #region "properties"

        /// <summary>
        /// Gets the hash byte value generated for the given key value.
        /// </summary>
        public byte[] Hash
        {
            get { return GetHash(); }
        }

        #endregion

        #region "methods"

        private byte[] GetHash()
        { return md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(value)); }

        #endregion

    }
}
