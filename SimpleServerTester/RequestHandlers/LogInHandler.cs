using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Fiction.Contracts;
using SimpleServerLib;
using SimpleServerTester.Engines;
using SimpleServerUtil;

namespace SimpleServerTester.RequestHandlers
{
    public class LogInHandler : IRequestHandler2
    {
        private UserAuthEngine _userAuthEngine;

        public LogInHandler()
        {
            _userAuthEngine = new UserAuthEngine();
        }

        public int? Handle(LogInCommand command)
        {
            int? userId = _userAuthEngine.AuthenticateUserCredentials(command.Username, command.Password);
            if (!userId.HasValue)
                return null;

            string publicKey =
                "<RSAKeyValue><Modulus>sCfKnATVPBVocjpBDlw66OKUScwoLEbwSET/MJH/pN8WwlK6ShXR2n8zursGEpKKojZmYiMDlC7kWEWmE4x5MbFDGov+XfE6+dfy8Tfo8+N18TdEC/PfpBvKTnHXryy+tOQGMjAqyJ/c3KWeLF9WPr8GMe/WGjypGZ6vBm1KCGk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

            string privateKey =
                "<RSAKeyValue><Modulus>sCfKnATVPBVocjpBDlw66OKUScwoLEbwSET/MJH/pN8WwlK6ShXR2n8zursGEpKKojZmYiMDlC7kWEWmE4x5MbFDGov+XfE6+dfy8Tfo8+N18TdEC/PfpBvKTnHXryy+tOQGMjAqyJ/c3KWeLF9WPr8GMe/WGjypGZ6vBm1KCGk=</Modulus><Exponent>AQAB</Exponent><P>2exqsE8wMgxdh3fBrcPVvyfJVWRF0XQuPXlgUcvUqvtV1j+nd/loEuZAhurJTdcBgAh2p3i2PK2PPCyvRIY8hw==</P><Q>zu8dap4J8HJGEDaGyr7Hs3hV5/2brRvFyT5lqDexUCTrQ1LHZoXEjYyLHmTgYUJA0HL0VB5CSDTM6vhYwo8/jw==</Q><DP>pY0346bU60B/kRlGNnaum+Bi0A80BxGmyya5KIqbjiUPqYqD892x1aG75YLZ2Nt8lUJYuZM3hlMnnEQqBfmgYw==</DP><DQ>xHUYvV7a2s7ym3PY1mT3XPiSvlP0Vw114g/+HQz6prbWMC1Hp5q5txTGAw4MQ+nNdNieA7pSfRC7txwd0GCFGw==</DQ><InverseQ>b2hmqznGl1QYBHPALb+AvzwJNDexkTAO+wkisieNqgq1qQRFTqu3ORtF3Hd57drDOj8vtEXWg0p9gJPRu2KCvw==</InverseQ><D>a5XB3ovtF6cThUAi33xg1j8bxf6UQoaI1fqdyLJCahV+NuyDrh+pbULm+xKiYNFzLiL/YMx1sRj6iRbwmVvf4AAiEYRQrYfj6uGRjWZoQD7luDpfcQL4ylf0W7ijsQ0G8z0gepf0sW/K/dssGBE/DgUNaPlfXV56WNkbRG5zhYk=</D></RSAKeyValue>";

            RSACryptoServiceProvider rsaEncryptor = new RSACryptoServiceProvider();
            rsaEncryptor.FromXmlString(publicKey);
            byte[] data = Encoding.UTF8.GetBytes(userId.ToString());
            byte[] encryptedData = rsaEncryptor.Encrypt(data, false);

            string encryptedUserId = Convert.ToBase64String(encryptedData);
            Console.WriteLine(encryptedUserId);

            rsaEncryptor.FromXmlString(privateKey);
            byte[] original = rsaEncryptor.Decrypt(Convert.FromBase64String(encryptedUserId), false);
            Console.WriteLine(Encoding.UTF8.GetString(original));

            return userId;
        }
    }
}
