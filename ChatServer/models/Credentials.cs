using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatServer.models
{
    [Serializable]
    public class Credentials
    {
        public string username { get; set; }
        public string password { get; set; }

        public Credentials(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);

        }
    }
}
