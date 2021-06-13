
using DemoChat.helpers;
using EI.SI;
using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using DemoChat.models;

namespace DemoChat
{
    public partial class FormChat : Form
    {
        FormLogin formlogin;

        NetworkStream networkStream;
        ProtocolSI protocolSI;


        public FormChat(FormLogin formlogin, ProtocolSI protocolSI, NetworkStream networkStream)
        {
            InitializeComponent();

            this.formlogin = formlogin;
            this.protocolSI = protocolSI;
            this.networkStream = networkStream;
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                LogHelper.logToFile("======= ProtocolSICmdType.DATA =======");

                string msg = tbMessage.Text;
                if (msg.Length == 0)
                {
                    return;
                }

                // adiciona a mensagem à listbox
                lbChat.Items.Add("ENVIO: " + msg);
                tbMessage.Clear();

                // encripta a mensagem com a chave simetrica
                byte[] msgBytes = Encoding.UTF8.GetBytes(msg);
                string msgCifradaSymetricKeyB64 = Helper.EncryptWithSymmetricKey(Convert.ToBase64String(msgBytes), formlogin.serverSymetricKey);
                LogHelper.logToFile("Encriptada a mensagem com a chave simetrica: " + msgCifradaSymetricKeyB64);

                // encripta mensagem com chave publica
                string msgCrifradaB64 = Helper.EncryptMessageWithPublicKey(msgCifradaSymetricKeyB64);
                LogHelper.logToFile("Encriptada a mensagem com a publica do destinatario (o proprio cliente): " + msgCrifradaB64);

                // constroi o pacote de envio de msg cifrada
                byte[] packet = protocolSI.Make(ProtocolSICmdType.DATA, msgCrifradaB64);
                networkStream.Write(packet, 0, packet.Length);
                LogHelper.logToFile("Enviada mensagem para o servidor: " + msgCrifradaB64);

                // espera e processa reposta do servidor
                while (protocolSI.GetCmdType() != ProtocolSICmdType.ACK)
                {
                    networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);

                    // recebe a mensagem do servidor
                    if (protocolSI.GetCmdType() == ProtocolSICmdType.DATA)
                    {
                        string msgRecebida = protocolSI.GetStringFromData();
                        LogHelper.logToFile("Recebida mensagem do servidor: " + msgRecebida);

                        // desencripta a mensagem com a chave privada
                        string msgRecebidaDecryptedPrivateKey = Helper.DesencryptMessageWithPrivateKey(msgRecebida);
                        LogHelper.logToFile("Desencriptada a mensagem com a privada do remetente (o proprio cliente): " + msgRecebida);

                        // desencripta a mensagem com a chave simetrica
                        string msgRecebidaDecryptedSymetricKey = Helper.DecryptWithSymmetricKey(msgRecebidaDecryptedPrivateKey, formlogin.serverSymetricKey);
                        LogHelper.logToFile("Desencriptada a mensagem com a chave simetrica: " + msgRecebidaDecryptedPrivateKey);

                        lbChat.Items.Add("RESPOSTA: " + Encoding.UTF8.GetString(Convert.FromBase64String(msgRecebidaDecryptedSymetricKey)));

                        byte[] ack = protocolSI.Make(ProtocolSICmdType.ACK);
                        networkStream.Write(ack, 0, ack.Length);
                        LogHelper.logToFile("Enviado o ACK para o servidor da resposta recebida");
                    }

                    if (protocolSI.GetCmdType() == ProtocolSICmdType.ACK)
                    {
                        LogHelper.logToFile("Recebido ACK do servidor pelo envio da mensagem");
                    }
                }

                // limpar o buffer do protocolSI para remover o command type do ACK anterior para evitar abrir e fechar a transmissao constantemente (EOT)
                Array.Clear(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
            }
            catch (Exception)
            {
                MessageBox.Show("Erro ao enviar a mensagem :(");
            }
        }

        private void btnAnexar_Click(object sender, EventArgs e)
        {

        }

        private void tbMessage_TextChanged(object sender, EventArgs e)
        {
            if (tbMessage.Text.Length > 0)
            {
                btnEnviar.Enabled = true;
            } else
            {
                btnEnviar.Enabled = false;
            }
        }

        private void FormChat_FormClosed(object sender, FormClosedEventArgs e)
        {
            formlogin.Show();
        }

        private void tbMessage_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                btnEnviar_Click(sender, null);
            }
        }
    }
}


//Teste Adicionar comentario

