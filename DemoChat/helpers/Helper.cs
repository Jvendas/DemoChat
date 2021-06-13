using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DemoChat
{
    // class criada para ajudar e que simplificam a repetição de codigo
    public class Helper
    {
        private static string privateKeyPath = "privateKey.txt";
        private static string publicKeyPath = "publicKey.txt";

        // funcao para ser chamada sempre que for necessario desencriptar uma mensagem com a chave privada
        public static string DesencryptMessageWithPrivateKey(string msg)
        {
            string pk = GetPrivateKey();
            return DesencryptMessageWithPrivateKey(pk, msg);
        }

        private static string GetPrivateKey()
        {
            return File.ReadAllText(privateKeyPath);
        }

        private static string DesencryptMessageWithPrivateKey(string privateKey, string msg)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
            rsa.FromXmlString(privateKey);

            //cifrar dados utilizado o rsa
            byte[] dadosDesencriptados = Convert.FromBase64String(msg);
            byte[] dadosDesencriptadosBytes = rsa.Decrypt(dadosDesencriptados, true);

            return Encoding.UTF8.GetString(dadosDesencriptadosBytes);
        }

        public static string GetPublicKey()
        {
            return File.ReadAllText(publicKeyPath);
        }

        public static void CriarChaves()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);

            //criar e devolver as chaves publica e privada
            string publicKey = rsa.ToXmlString(false);
            string privateKey = rsa.ToXmlString(true);

            //guardar em ficheiro as chaves        
            SaveKeyPairToDisk(privateKey, publicKey);
        }

        public static void SaveKeyPairToDisk(string privateKey, string publicKey)
        {
            File.WriteAllText(privateKeyPath, privateKey);
            File.WriteAllText(publicKeyPath, publicKey);
        }

        // função para encriptar uma mensagem com a chave simetrica
        public static string EncryptWithSymmetricKey(string msgB64, SymetricKey symetricKey)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();

            aes.Key = Convert.FromBase64String(symetricKey.aesKey);
            aes.IV = Convert.FromBase64String(symetricKey.aesIv);

            //variavel para guardar o texto decifrado em bytes
            byte[] txtDecifrado = Encoding.UTF8.GetBytes(msgB64);

            // variavel para guardar o texto cifrado em bytes
            byte[] txtCifrado;

            //Reservar espaço em mémoria pra colocar o texto e cifrá-lo
            MemoryStream ms = new MemoryStream();

            //Inicializar o sistema de cifragem (write)
            CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);

            //Cifrar dados
            cs.Write(txtDecifrado, 0, txtDecifrado.Length);
            cs.Close();

            //guardar os dados cifrado que estão em memoria
            txtCifrado = ms.ToArray();

            //Converter os bytes da base64 para texto
            string txtCifradoB64 = Convert.ToBase64String(txtCifrado);

            //devolver os bytes criados em base64
            return txtCifradoB64;
        }


        // função para desencriptar uma mensagem com a chave simetrica
        public static string DecryptWithSymmetricKey(string msgB64, SymetricKey symetricKey)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();

            aes.Key = Convert.FromBase64String(symetricKey.aesKey);
            aes.IV = Convert.FromBase64String(symetricKey.aesIv);

            //variavel para guaradr o texto cifrado em bytes
            byte[] txtCifrado = Convert.FromBase64String(msgB64);

            // reservar espaço em memoria para coocar o texto e cifra-lo
            MemoryStream ms = new MemoryStream(txtCifrado);

            //Inicializar o sistema de cifragem(read)
            CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);

            //Variavel para guardar o texto decifrado
            byte[] txtDecifrado = new byte[ms.Length];

            //Variavel para ter o numero de bytes decifrados
            int bytesLidos = 0;

            //Decifrar os dados
            bytesLidos = cs.Read(txtDecifrado, 0, txtDecifrado.Length);
            cs.Close();

            //Converter para texto
            string textoDecifrado = Encoding.UTF8.GetString(txtDecifrado, 0, bytesLidos);

            //devolver o texto decifrado
            return textoDecifrado;
        }

        public static string EncryptMessageWithPublicKey(string msg)
        {
            string publicKey = GetPublicKey();

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
            rsa.FromXmlString(publicKey);

            //cifrar dados utilizado o rsa
            byte[] dadosencriptadosbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(msg), true);
            string dadosencriptadosb64 = Convert.ToBase64String(dadosencriptadosbytes);

            return dadosencriptadosb64;
        }
    }
}
