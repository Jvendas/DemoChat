using ChatServer.helpers;
using ChatServer.models;
using EI.SI;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace ChatServer
{
    class Program
    {
        // porto do servidor
        private const int PORT = 10000;

        static void Main(string[] args)
        {
            LogHelper.logToFile("=====================================================================");
            LogHelper.logToFile(" DemoChat Server | " + DateTime.Now.ToString());
            LogHelper.logToFile("=====================================================================");

            try
            {
                // apagar a lista de client ids no arranque
                CleanClientTableFromDatabase();

                // colocar o servidor à escuta
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, PORT);
                TcpListener listener = new TcpListener(endPoint);
                listener.Start();

                // colocar o servidor constantemente à escuta de pedidos de clientes
                int clientCounter = 0;

                while (true)
                {
                    // receber pedido do client
                    TcpClient client = listener.AcceptTcpClient();
                    clientCounter++;

                    // registar o handler
                    ClientHandler clientHandler = new ClientHandler(client, clientCounter);
                    clientHandler.Handle();

                    LogHelper.logToFile("Cliente conectado com o client id: " + clientCounter);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("EXCEPTION: " + e.Message);
                Console.WriteLine("\nPress enter to close the server...");
                Console.ReadLine();
            }
        }


        private static void CleanClientTableFromDatabase()
        {
            LogHelper.logToFile("Cleaning previous client ids from database");
            SqlCommand sqlCommand = new SqlCommand("DELETE FROM Client;");

            DbHelper db = new DbHelper();
            db.ExecutarComandoSQL(sqlCommand);
            db.CloseConnection();
        }
    }

    class ClientHandler
    {
        private TcpClient client;
        private int clientID;

        public ClientHandler(TcpClient client, int clientID)
        {
            this.client = client;
            this.clientID = clientID;
        }

        public void Handle()
        {
            // defenicao da variavel thread e arranque da mesma 
            // threads sao unidades de execução dentro de um processo
            // permitem executar varias instrucoes em simulataneo

            Thread thread = new Thread(threadHandler);
            thread.Start();
        }

        private void threadHandler()
        {
            NetworkStream networkStream = this.client.GetStream();
            ProtocolSI protocolSI = new ProtocolSI();

            while (protocolSI.GetCmdType() != ProtocolSICmdType.EOT)
            {
                networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                byte[] ack;

                switch (protocolSI.GetCmdType())
                {
                    case ProtocolSICmdType.PUBLIC_KEY:
                        {
                            LogHelper.logToFile("======= ProtocolSICmdType.PUBLIC_KEY =======");

                            // obter chave privada do servidor
                            string publicKeyB64 = protocolSI.GetStringFromData();
                            string pubKey = Encoding.UTF8.GetString(Convert.FromBase64String(publicKeyB64));
                            LogHelper.logToFile("Recebida chave publica do cliente: " + publicKeyB64);

                            // gerar chave simetrica
                            SymetricKey symetricKey = new SymetricKey();
                            LogHelper.logToFile("Gerada chave simetrica: " + symetricKey.ToString());

                            // guardar chaves na BD
                            RegistaCliente(clientID, publicKeyB64, symetricKey);
                            LogHelper.logToFile("Guardada a chave publica e a chave simetrica na base dados");

                            // envia a chave simetrica para o cliente
                            string responseB64 = Helper.EncryptMessageWithPublicKey(pubKey, symetricKey.ToString());

                            byte[] packet = protocolSI.Make(ProtocolSICmdType.PUBLIC_KEY, responseB64);
                            networkStream.Write(packet, 0, packet.Length);
                            LogHelper.logToFile("Enviado chave simetrica para o cliente");

                            while (protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
                            {
                                networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);

                                if (protocolSI.GetCmdType() == ProtocolSICmdType.ACK)
                                {
                                    LogHelper.logToFile("Recebido ACK do cliente pelo envio da chave simetrica");
                                }
                            }

                            // limpar o buffer do protocolSI para remover o command type do ACK anterior para evitar abrir e fechar a transmissao constantemente (EOT)
                            Array.Clear(protocolSI.Buffer, 0, protocolSI.Buffer.Length);

                            // envia o ack para o cliente para indicar que a comunicaçao terminou
                            ack = protocolSI.Make(ProtocolSICmdType.ACK);
                            networkStream.Write(ack, 0, ack.Length);
                            LogHelper.logToFile("Enviado o ACK para o cliente da receção chave publica");

                            break;
                        }

                    case ProtocolSICmdType.DATA:
                        {
                            LogHelper.logToFile("======= ProtocolSICmdType.DATA =======");

                            // recebe uma mensagem do cliente
                            string msgRecebida = protocolSI.GetStringFromData();
                            LogHelper.logToFile("Recebida mensagem de chat do client: " + msgRecebida);

                            // envia a mensagem para o cliente de novo porque neste exemplo só temos um cliente.
                            // como a mensagem foi encriptada com uma chave publica apenas o dono da chave privada consegue desencriptar.
                            // então é enviado de volta para o mesmo cliente a mesma mensagem
                            byte[] packet = protocolSI.Make(ProtocolSICmdType.DATA, msgRecebida);
                            networkStream.Write(packet, 0, packet.Length);
                            LogHelper.logToFile("Enviado a mensagem de chat de volta para o cliente: " + msgRecebida);

                            while (protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
                            {
                                networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);

                                if (protocolSI.GetCmdType() == ProtocolSICmdType.ACK)
                                {
                                    LogHelper.logToFile("Recebido o ACK do cliente pelo envio da mensagem de chat");
                                }
                            }

                            // envia o ack para o cliente para indicar que a comunicaçao terminou
                            ack = protocolSI.Make(ProtocolSICmdType.ACK);
                            networkStream.Write(ack, 0, ack.Length);
                            LogHelper.logToFile("Enviado o ACK para o cliente pelo envio da mensagem de chat");

                            break;
                        }

                    // Registo
                    case ProtocolSICmdType.USER_OPTION_1:
                        {
                            LogHelper.logToFile("======= ProtocolSICmdType.USER_OPTION_1 - Registo =======");

                            // receive credentials msg
                            string receivedCredentials = protocolSI.GetStringFromData();
                            LogHelper.logToFile("Recebidas as credenciais: " + receivedCredentials);

                            // get symmetric key from database for this clientID to decrypt the credentials
                            SymetricKey symetricKey = GetSymetricKeyFromDB();
                            if (symetricKey == null)
                            {
                                string response = "ERROR_SYMETRIC_KEY";
                                string responseB64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(response));

                                byte[] packet = protocolSI.Make(ProtocolSICmdType.USER_OPTION_1, responseB64);
                                networkStream.Write(packet, 0, packet.Length);
                                LogHelper.logToFile("Enviado para o cliente que não foi encontrada uma symetric key para o client id: " + clientID);

                                while (protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
                                {
                                    networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);

                                    if (protocolSI.GetCmdType() == ProtocolSICmdType.ACK)
                                    {
                                        LogHelper.logToFile("Recebido o ACK do cliente pelo envio do feedback no registo");
                                    }
                                }

                                // limpar o buffer do protocolSI para remover o command type do ACK anterior para evitar abrir e fechar a transmissao constantemente (EOT)
                                Array.Clear(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                            }
                            else
                            {
                                // decrypt msg with client symmetric key
                                string msgDecifrada = Helper.DecryptWithSymmetricKey(receivedCredentials, symetricKey);

                                // get credentials from decrypted msg
                                Credentials credentials = JsonSerializer.Deserialize<Credentials>(msgDecifrada);
                                LogHelper.logToFile("Credenciais desencriptadas com a chave simetrica para o username: " + credentials.username);

                                // check if exist credentials on table "User"
                                bool usernameExists = CheckUsernameExists(credentials.username);

                                if (usernameExists)
                                {
                                    // if yes send message to client "This username is already registered"
                                    string response = "ERROR_USERNAME";
                                    string responseB64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(response));

                                    byte[] packet = protocolSI.Make(ProtocolSICmdType.USER_OPTION_1, responseB64);
                                    networkStream.Write(packet, 0, packet.Length);
                                    LogHelper.logToFile("Enviado para o client que o username já se encontra registado");

                                    while (protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
                                    {
                                        networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);

                                        if (protocolSI.GetCmdType() == ProtocolSICmdType.ACK)
                                        {
                                            LogHelper.logToFile("Recebido o ACK para o cliente pelo envio das credenciais");
                                        }
                                    }

                                    // limpar o buffer do protocolSI para remover o command type do ACK anterior para evitar abrir e fechar a transmissao constantemente (EOT)
                                    Array.Clear(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                                }
                                else
                                {
                                    // if not store the credentials to table "User"
                                    byte[] salt = Helper.GenerateSalt(Helper.SALTSIZE);
                                    byte[] passwordHash = Helper.GenerateSaltedHash(credentials.password, salt);
                                    LogHelper.logToFile("Gerada a hash da password e o salt para o username: " + credentials.username);

                                    bool userRegistado = RegistarUser(credentials.username, passwordHash, salt);

                                    if (userRegistado == false)
                                    {
                                        string response = "ERROR_REGISTER_INSERT_BD";
                                        string responseB64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(response));

                                        byte[] packet = protocolSI.Make(ProtocolSICmdType.USER_OPTION_1, responseB64);
                                        networkStream.Write(packet, 0, packet.Length);
                                        LogHelper.logToFile("Enviado feedback que o registo falhou a registar o user na BD");

                                        while (protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
                                        {
                                            networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);

                                            if (protocolSI.GetCmdType() == ProtocolSICmdType.ACK)
                                            {
                                                LogHelper.logToFile("Recebido ACK do envio do feedback no registo");
                                            }
                                        }

                                        // limpar o buffer do protocolSI para remover o command type do ACK anterior para evitar abrir e fechar a transmissao constantemente (EOT)
                                        Array.Clear(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                                    }
                                    else
                                    {
                                        LogHelper.logToFile("Registado com sucesso o user: " + credentials.username);

                                        string response = "REGISTER_OK";
                                        string responseB64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(response));

                                        byte[] packet = protocolSI.Make(ProtocolSICmdType.USER_OPTION_1, responseB64);
                                        networkStream.Write(packet, 0, packet.Length);
                                        LogHelper.logToFile("Enviado registo OK para o cliente no registo");

                                        while (protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
                                        {
                                            networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);

                                            if (protocolSI.GetCmdType() == ProtocolSICmdType.ACK)
                                            {
                                                LogHelper.logToFile("Recebido o ACK do cliente no envio do feedback no registo");
                                            }
                                        }

                                        // limpar o buffer do protocolSI para remover o command type do ACK anterior para evitar abrir e fechar a transmissao constantemente (EOT)
                                        Array.Clear(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                                    }
                                }
                            }

                            // Enviar ack para o cliente
                            ack = protocolSI.Make(ProtocolSICmdType.ACK);
                            networkStream.Write(ack, 0, ack.Length);
                            LogHelper.logToFile("Enviado o ACK para o cliente do pedido de registo");

                            break;
                        }

                    // login
                    case ProtocolSICmdType.USER_OPTION_2:
                        {
                            LogHelper.logToFile("======= ProtocolSICmdType.USER_OPTION_2 - Login =======");

                            // receive credentials msg
                            string receivedCredentials = protocolSI.GetStringFromData();
                            LogHelper.logToFile("Recebidas as credenciais: " + receivedCredentials);

                            // get symmetric key from database for this clientID to decrypt the credentials
                            SymetricKey symetricKey = GetSymetricKeyFromDB();
                            if (symetricKey == null)
                            {
                                string response = "ERROR_SYMETRIC_KEY";
                                string responseB64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(response));

                                byte[] packet = protocolSI.Make(ProtocolSICmdType.USER_OPTION_2, responseB64);
                                networkStream.Write(packet, 0, packet.Length);
                                LogHelper.logToFile("Enviado para o cliente que não foi encontrada uma symetric key para o client id: " + clientID);

                                while (protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
                                {
                                    networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);

                                    if (protocolSI.GetCmdType() == ProtocolSICmdType.ACK)
                                    {
                                        LogHelper.logToFile("Recebido o ACK do cliente pelo envio do feedback no login");
                                    }
                                }

                                // limpar o buffer do protocolSI para remover o command type do ACK anterior para evitar abrir e fechar a transmissao constantemente (EOT)
                                Array.Clear(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                            }
                            else
                            {
                                // decrypt msg with client symmetric key
                                string msgDecifrada = Helper.DecryptWithSymmetricKey(receivedCredentials, symetricKey);

                                // get credentials from decrypted msg
                                Credentials credentials = JsonSerializer.Deserialize<Credentials>(msgDecifrada);
                                LogHelper.logToFile("Credenciais desencriptadas com a chave simetrica para o username: " + credentials.username);

                                // check if login credentials are valid
                                bool loginIsValid = VerifyLogin(credentials);

                                // enviar resposta do login com ou sem sucesso
                                string response = "";

                                if (loginIsValid)
                                {
                                    response = "LOGIN_OK";
                                    LogHelper.logToFile("Enviado login com sucesso para o username: " + credentials.username);
                                }
                                else
                                {
                                    response = "LOGIN_ERROR";
                                    LogHelper.logToFile("Enviado login falhou para o username: " + credentials.username);
                                }

                                string responseB64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(response));
                                byte[] packet = protocolSI.Make(ProtocolSICmdType.USER_OPTION_2, responseB64);
                                networkStream.Write(packet, 0, packet.Length);

                                while (protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
                                {
                                    networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);

                                    if (protocolSI.GetCmdType() == ProtocolSICmdType.ACK)
                                    {
                                        LogHelper.logToFile("Recebido o ACK do cliente no envio do feedback do login");
                                    }
                                }

                                // limpar o buffer do protocolSI para remover o command type do ACK anterior para evitar abrir e fechar a transmissao constantemente (EOT)
                                Array.Clear(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                            }

                            // Enviar ack para o cliente
                            ack = protocolSI.Make(ProtocolSICmdType.ACK);
                            networkStream.Write(ack, 0, ack.Length);
                            LogHelper.logToFile("Enviado o ACK para o cliente do pedido de login");

                            break;
                        }

                    case ProtocolSICmdType.EOT:
                        {
                            LogHelper.logToFile("======= ProtocolSICmdType.EOT =======");

                            // to send the signal of End of Transmission EOT.
                            Console.WriteLine("Ending Thread from client {0}", clientID);
                            ack = protocolSI.Make(ProtocolSICmdType.ACK);
                            networkStream.Write(ack, 0, ack.Length);
                            break;
                        }

                    default:
                        LogHelper.logToFile("Erro ProtocolSICmdType invalido: " + protocolSI.GetCmdType());
                        break;
                }
            }

            // fecho do netWorkStream e do cliente (tcpClient)
            networkStream.Close();
            client.Close();
        }

        private void RegistaCliente(int clientID, string pulicKey, SymetricKey symetricKey)
        {
            try
            {
                // declaração do comando SQL
                string sql = "INSERT INTO Client (ClientId, PublicKey, aesKey, aesIv) VALUES (@ClientId,@PublicKey,@aesKey,@aesIv)";

                // declaração dos parâmetros do comando SQL
                SqlParameter paramClientId = new SqlParameter("@ClientId", clientID);
                SqlParameter paramPublickey = new SqlParameter("@PublicKey", pulicKey);
                SqlParameter paramAesKey = new SqlParameter("@aesKey", symetricKey.aesKey);
                SqlParameter paramAesIv = new SqlParameter("@aesIv", symetricKey.aesIv);

                // instancia o comando SQL para ser executado na Base de Dados
                SqlCommand cmd = new SqlCommand(sql);

                // introduzir valores aos parâmentros registados no comando SQL
                cmd.Parameters.Add(paramClientId);
                cmd.Parameters.Add(paramPublickey);
                cmd.Parameters.Add(paramAesKey);
                cmd.Parameters.Add(paramAesIv);

                // executar o comando sql
                DbHelper db = new DbHelper();
                int lines = db.ExecutarComandoSQL(cmd);
                db.CloseConnection();

                if (lines == 0)
                {
                    // se forem devolvidas 0 linhas alteradas então o não foi executado com sucesso
                    throw new Exception("Erro ao registar a chave publica e a chave simetrica");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("EXCEPTION: " + e.Message);
            }
        }

        private SymetricKey GetSymetricKeyFromDB()
        {
            try
            {
                string sql = "SELECT * FROM CLIENT WHERE ClientId = @ClientId;";
                SqlParameter paramClientId = new SqlParameter("@ClientId", clientID);

                SqlCommand cmdFindClientId = new SqlCommand(sql);
                cmdFindClientId.Parameters.Add(paramClientId);

                DbHelper db = new DbHelper();
                int lines = db.ExecutarComandoSQL(cmdFindClientId);

                if (lines == 0)
                {
                    return null;
                }

                // obter a symetric key da resposta no pedidos à base de dados
                SymetricKey symetricKey = new SymetricKey();
                SqlDataReader reader = cmdFindClientId.ExecuteReader();

                while (reader.Read())
                {
                    symetricKey.aesIv = reader["aesiv"].ToString();
                    symetricKey.aesKey = reader["aeskey"].ToString();
                }

                db.CloseConnection();

                if (symetricKey.aesIv.Length == 0 || symetricKey.aesKey.Length == 0)
                {
                    return null;
                }

                return symetricKey;
            }
            catch (Exception e)
            {
                Console.WriteLine("EXCEPTION: " + e.Message);
                return null;
            }
        }

        private bool CheckUsernameExists(string username)
        {
            try
            {
                string sql = "SELECT * FROM USERS WHERE Username = @Username;";

                SqlParameter paramUsername = new SqlParameter("@Username", username);

                // comando SQL para ser executado na Base de Dados
                SqlCommand cmdFindUsername = new SqlCommand(sql);
                cmdFindUsername.Parameters.Add(paramUsername);

                DbHelper db = new DbHelper();
                int lines = db.ExecutarComandoSQL(cmdFindUsername);

                if (lines == 0)
                {
                    return false;
                }

                bool usernameExists = cmdFindUsername.ExecuteReader().HasRows;

                db.CloseConnection();

                return usernameExists;
            }
            catch (Exception e)
            {
                Console.WriteLine("EXCEPTION: " + e.Message);
                return false;
            }
        }

        private bool RegistarUser(string username, byte[] saltedPasswordHash, byte[] salt)
        {
            try
            {
                string sql = "INSERT INTO USERS (Username, SaltedPasswordHash, Salt) VALUES (@Username, @SaltedPasswordHash, @Salt);";

                SqlParameter paramUsername = new SqlParameter("@Username", username);
                SqlParameter paramSaltedPasswordHash = new SqlParameter("@SaltedPasswordHash", saltedPasswordHash);
                SqlParameter paramSalt = new SqlParameter("@Salt", salt);

                // comando SQL para ser executado na Base de Dados
                SqlCommand cmdInserCredentials = new SqlCommand(sql);
                cmdInserCredentials.Parameters.Add(paramUsername);
                cmdInserCredentials.Parameters.Add(paramSaltedPasswordHash);
                cmdInserCredentials.Parameters.Add(paramSalt);

                // executar o comando sql
                DbHelper db = new DbHelper();
                int lines = db.ExecutarComandoSQL(cmdInserCredentials);
                db.CloseConnection();

                if (lines == 0)
                {
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("EXCEPTION: " + e.Message);
                return false;
            }
        }

        private bool VerifyLogin(Credentials credentials)
        {
            try
            {
                string sql = "SELECT * FROM USERS WHERE Username = @Username;";
                SqlParameter paramUsername1 = new SqlParameter("@Username", credentials.username);

                SqlCommand cmdFindUsername = new SqlCommand(sql);
                cmdFindUsername.Parameters.Add(paramUsername1);

                DbHelper db = new DbHelper();
                db.ExecutarComandoSQL(cmdFindUsername);
                SqlDataReader reader = cmdFindUsername.ExecuteReader();

                bool loginIsValid = false;

                while (reader.Read())
                {
                    // get login credentials da BD
                    string userNameStored = reader["Username"].ToString();
                    byte[] saltedPasswordHashStored = (byte[])reader["SaltedPasswordHash"];
                    byte[] saltStored = (byte[])reader["Salt"];

                    bool usernameMatch = credentials.username == userNameStored.Trim(); // o trim remove os espaços em branco do inicio e fim da string
                    bool passowordMatch = Helper.CheckSaltedPasswordHash(saltedPasswordHashStored, credentials.password, saltStored);

                    loginIsValid = usernameMatch && passowordMatch;
                }

                db.CloseConnection();

                return loginIsValid;

            }
            catch (Exception e)
            {
                Console.WriteLine("EXCEPTION: " + e.Message);
                return false;
            }
        }
    }
}





