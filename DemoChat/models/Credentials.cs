using System;
using System.Text.Json;

namespace DemoChat.models
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
