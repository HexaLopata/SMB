using System;
using System.Security.Cryptography;
using System.Text;

namespace SMB.Models.Autentification
{
    public class PasswordHasher
    {
        private const string _key = "key";
        private MD5 _md5;

        /// <summary>
        /// Возвращает хэшированный пароль в виде строки
        /// </summary>
        /// <param name="password"></param>
        public string ReturnHashedPasswordAsString(string password)
        {
            var hash = _md5.ComputeHash(Encoding.UTF8.GetBytes(password + _key));
            return BitConverter.ToString(hash);
        }

        public PasswordHasher()
        {
            _md5 = MD5.Create();
        }
    }
}