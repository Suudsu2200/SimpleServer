using System;
using System.Security.Cryptography;
using System.Text;
using SimpleServerUtil;
using Fiction.Contracts;
using SimpleServerLib;
using SimpleServerTester.Data;

namespace SimpleServerTester.RequestHandlers
{
    public class CreateUserHandler : IRequestHandler2
    {
        private SHA256 sha256Encryptor;
        private FictionDataStore dataStore;

        public CreateUserHandler()
        {
            dataStore = new FictionDataStore();
            sha256Encryptor = SHA256Managed.Create();
        }

        public int? Handle(CreateUserCommand command)
        {
            byte[] hashBytes = sha256Encryptor.ComputeHash(Encoding.UTF8.GetBytes(command.Password));
            string hashedPassword = Convert.ToBase64String(hashBytes);
            return dataStore.CreateUser(command.Username, hashedPassword);
        }


    }
}
