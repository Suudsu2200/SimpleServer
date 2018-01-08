using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.Contracts
{
    [Serializable]
    public class CreateUserCommand
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
