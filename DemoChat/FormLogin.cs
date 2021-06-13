using EI.SI;
using System;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Text.Json;
using DemoChat.models;
using DemoChat.helpers;

namespace DemoChat
{
    public partial class FormLogin : Form
    {
        private NetworkStream networkStream;
        private ProtocolSI protocolSI;
        private TcpClient client;

        private FormChat chatForm;

        public SymetricKey serverSymetricKey { get; set; }

        public FormLogin(ProtocolSI protocolSI, NetworkStream networkStream, TcpClient client)
        {
            InitializeComponent();

            this.protocolSI = protocolSI;
            this.networkStream = networkStream;
            this.client = client;

            LogHelper.logToFile("Criando o par de chaves RSA");
            Helper.CriarChaves();

            enviarChavePublica();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                LogHelper.logToFile("======= ProtocolSICmdType.USER_OPTION_1 - Registo =======");

                string username = tbUserName.Text;
                string pass = tbPassword.Text;

                if (username.Length == 0 || pass.Length == 0)
                {
                    MessageBox.Show("Por favor introduza as credenciais", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                LogHelper.logToFile("Encriptadas as credenciais do utlizador com a chave simetrica enviada pelo servidor");
                Credentials credentials = new Credentials(username, pass);
                string credentialsEncrypted = Helper.EncryptWithSymmetricKey(credentials.ToString(), serverSymetricKey);

                // enviar um pacote de dados com as credencias do cliente (user e password) com o auxilio do modelo Credentials criado
                byte[] packet = protocolSI.Make(ProtocolSICmdType.USER_OPTION_1, credentialsEncrypted);
                networkStream.Write(packet, 0, packet.Length);
                LogHelper.logToFile("Enviadas as credenciais do utilizador: " + credentialsEncrypted);

                // Cliente fica à espera do feedback do servidor no registo
                while (protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
                {
                    networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);

                    // ler feedback do registo (OK/FAIL)
                    if (protocolSI.GetCmdType() == ProtocolSICmdType.USER_OPTION_1)
                    {
                        string msgRecebida = protocolSI.GetStringFromData();
                        msgRecebida = Encoding.UTF8.GetString(Convert.FromBase64String(msgRecebida));

                        byte[] ack = protocolSI.Make(ProtocolSICmdType.ACK);
                        networkStream.Write(ack, 0, ack.Length);
                        LogHelper.logToFile("Enviado o ACK para o servidor do feedback do registo");

                        switch (msgRecebida)
                        {
                            case "ERROR_SYMETRIC_KEY":
                                LogHelper.logToFile("O registo falhou porque não foi encontrada a symetric key");
                                MessageBox.Show("O registo falhou porque não foi encontrada a symetric key", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;

                            case "ERROR_USERNAME":
                                LogHelper.logToFile("O registo falhou porque o username já existe");
                                MessageBox.Show("O registo falhou porque o username já existe", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;

                            case "ERROR_REGISTER_INSERT_BD":
                                LogHelper.logToFile("O registo falhou porque as credenciais são inválidas");
                                MessageBox.Show("O registo falhou porque as credenciais são inválidas", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;

                            case "REGISTER_OK":
                                LogHelper.logToFile("Registo efetuado com sucesso");
                                MessageBox.Show("Registo efetuado com sucesso", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                        }
                    }

                    if (protocolSI.GetCmdType() == ProtocolSICmdType.ACK)
                    {
                        LogHelper.logToFile("Recebido ACK do servidor no envio das credenciais");
                    }
                }

                // limpar o buffer do protocolSI para remover o command type do ACK anterior para evitar abrir e fechar a transmissao constantemente (EOT)
                Array.Clear(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
            }
            catch (Exception)
            {
                MessageBox.Show("Erro no registo :(");
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                LogHelper.logToFile("======= ProtocolSICmdType.USER_OPTION_2 - Login =======");

                string username = tbUserName.Text;
                string pass = tbPassword.Text;

                if (username.Length == 0 || pass.Length == 0)
                {
                    MessageBox.Show("Por favor introduza as credenciais", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                LogHelper.logToFile("Encriptadas as credenciais do utlizador com a chave simetrica enviada pelo servidor");
                Credentials credentials = new Credentials(username, pass);
                string credentialsEncrypted = Helper.EncryptWithSymmetricKey(credentials.ToString(), serverSymetricKey);

                // enviar um pacote de dados com as credencias do cliente (user e password) com o auxilio do modelo Credentials criado
                byte[] packet = protocolSI.Make(ProtocolSICmdType.USER_OPTION_2, credentialsEncrypted);
                networkStream.Write(packet, 0, packet.Length);
                LogHelper.logToFile("Enviadas as credenciais do utilizador: " + credentialsEncrypted);

                // login difere do registo a partir daqui
                bool loginWithSuccess = false;

                // Cliente fica à espera do OK da parte do servidor em relação ao registo
                while (protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
                {
                    networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);

                    // ler feedback do registo (OK/FAIL)
                    if (protocolSI.GetCmdType() == ProtocolSICmdType.USER_OPTION_2)
                    {
                        string msgRecebida = protocolSI.GetStringFromData();
                        msgRecebida = Encoding.UTF8.GetString(Convert.FromBase64String(msgRecebida));

                        byte[] ack = protocolSI.Make(ProtocolSICmdType.ACK);
                        networkStream.Write(ack, 0, ack.Length);
                        LogHelper.logToFile("Enviado o ACK para o servidor do feedback do login");

                        switch (msgRecebida)
                        {
                            case "ERROR_CLIENTID":
                                LogHelper.logToFile("O login falhou porque o client id é inválido");
                                MessageBox.Show("O login falhou porque o client id é inválido", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;

                            case "ERROR_SYMETRIC_KEY":
                                LogHelper.logToFile("O login falhou porque não foi encontrada a symetric key");
                                MessageBox.Show("O login falhou porque não foi encontrada a symetric key", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;

                            case "LOGIN_ERROR":
                                LogHelper.logToFile("O login falhou porque as credenciais são inválidas");
                                MessageBox.Show("O login falhou porque as credenciais são inválidas", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;

                            case "LOGIN_OK":
                                LogHelper.logToFile("O login efetuado com sucesso");
                                loginWithSuccess = true;
                                break;
                        }
                    }

                    if (protocolSI.GetCmdType() == ProtocolSICmdType.ACK)
                    {
                        LogHelper.logToFile("Recebido ACK do servidor no envio das credenciais");
                    }
                }

                // limpar o buffer do protocolSI para remover o command type do ACK anterior para evitar abrir e fechar a transmissao constantemente (EOT)
                Array.Clear(protocolSI.Buffer, 0, protocolSI.Buffer.Length);

                if (loginWithSuccess)
                {
                    chatForm = new FormChat(this, protocolSI, networkStream);
                    chatForm.Show();
                    this.Hide();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Erro no login :(");
            }
        }

        private void enviarChavePublica()
        {
            try
            {
                LogHelper.logToFile("======= ProtocolSICmdType.PUBLIC_KEY =======");

                string publicKey = Helper.GetPublicKey();
                if (publicKey.Length == 0)
                {
                    MessageBox.Show("Não foi possível ler a chave publica do disco", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                LogHelper.logToFile("Lida a chave publica do disco com sucesso");

                // enviar chave publica para o servidor
                byte[] publicKeyBytes = Encoding.UTF8.GetBytes(publicKey);
                string publicKeyB64 = Convert.ToBase64String(publicKeyBytes);

                byte[] packet = protocolSI.Make(ProtocolSICmdType.PUBLIC_KEY, publicKeyB64);
                networkStream.Write(packet, 0, packet.Length);
                LogHelper.logToFile("Enviada a chave publica para o servidor: " + publicKeyB64);

                // espera e processa reposta do servidor
                while (protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
                {
                    networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);

                    // Cliente recebe a chave simetrica associado à chave publica que enviou anteriormente
                    if (protocolSI.GetCmdType() == ProtocolSICmdType.PUBLIC_KEY)
                    {
                        string msgRecebida = protocolSI.GetStringFromData();
                        msgRecebida = Helper.DesencryptMessageWithPrivateKey(msgRecebida);

                        serverSymetricKey = JsonSerializer.Deserialize<SymetricKey>(msgRecebida);
                        LogHelper.logToFile("Recebida a chave simetrica do servidor: " + serverSymetricKey.ToString());

                        byte[] ack = protocolSI.Make(ProtocolSICmdType.ACK);
                        networkStream.Write(ack, 0, ack.Length);
                        LogHelper.logToFile("Enviado o ACK para o servidor da chave simetrica recebida");
                    }

                    if (protocolSI.GetCmdType() == ProtocolSICmdType.ACK)
                    {
                        LogHelper.logToFile("Recebido ACK do servidor pelo envio da chave publica");
                    }
                }

                // limpar o buffer do protocolSI para remover o command type do ACK anterior para evitar abrir e fechar a transmissao constantemente (EOT)
                Array.Clear(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
            }
            catch (Exception)
            {
                MessageBox.Show("Erro ao enviar a chave publica :(");
            }
        }

        private void FormLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            LogHelper.logToFile("======= ProtocolSICmdType.EOT =======");

            byte[] ack = protocolSI.Make(ProtocolSICmdType.EOT);
            networkStream.Write(ack, 0, ack.Length);
            LogHelper.logToFile("Enviado o End of Transmission para o servidor");

            networkStream.Close();
            client.Close();
        }
    }
}