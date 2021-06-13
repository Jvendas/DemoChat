using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;


namespace DemoChat
{
    [Serializable]
    public class SymetricKey
    {
        public string aesKey { get; set; }
        public string aesIv { get; set; }

        public SymetricKey()
        {
            // Inicializar serviço de cifragem AES
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();

            // guardar a chave gerada em B64
            this.aesKey = Convert.ToBase64String(aes.Key);
            this.aesIv = Convert.ToBase64String(aes.IV);
        }

        public SymetricKey(string aesKey, string aesIv)
        {
            this.aesKey = aesKey;
            this.aesIv = aesIv;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
