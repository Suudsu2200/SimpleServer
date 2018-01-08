using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SimpleServerTester.Data;
using SimpleServerTester.Data.DataAccessObjects;

namespace SimpleServerTester.Engines
{
    public class UserAuthEngine
    {
        private FictionDataStore _fictionDataStore;
        private SHA256 _sha256Encryptor;
        private RSACryptoServiceProvider _rsaEncryptor;

        public UserAuthEngine()
        {
            _fictionDataStore = new FictionDataStore();
            _sha256Encryptor = SHA256Managed.Create();
        }

        public int? AuthenticateUserCredentials(string username, string password)
        {
            byte[] hashBytes = _sha256Encryptor.ComputeHash(Encoding.UTF8.GetBytes(password));
            string hashedPassword = Convert.ToBase64String(hashBytes);
            UserDao userDao = _fictionDataStore.GetUserByUsername(username);

            if (userDao == null)
            {
                return null;
            }

            if (hashedPassword == userDao.HashedPassword)
                return userDao.UserId;
            return (int?)null;
        }

        public string GenerateUserAuthHeader(int userId)
        {
            return "hi";
        }
    }
}
