
using EI.SI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoChat
{
    public partial class FormChat : Form
    {
        //Definir varivaveis
        private FormLogin formlogin;
        private const int PORT = 10000;
        NetworkStream networkStream;
        ProtocolSI protocolSI;
        TcpClient client;


        public FormChat(FormLogin formlogin)
        {
            InitializeComponent();

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, PORT);
            client = new TcpClient();
            client.Connect(endPoint);
            networkStream = client.GetStream();
            protocolSI = new ProtocolSI();
            this.formlogin = formlogin;
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                // guardar a mensagem escrita na variavel msg
                string msg = tbMessage.Text;
                lbChat.Items.Add(msg);
                tbMessage.Clear();

                byte[] packet = protocolSI.Make(ProtocolSICmdType.DATA, msg);
                networkStream.Write(packet, 0, packet.Length);

                while (protocolSI.GetCmdType() != ProtocolSICmdType.ACK) //ProtocolSICmdType is the first byte of the communication protoco
                {
                    networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Erro ao enviar a mensagem :(");
            }

        }

        //Metodo para fechar o Client
        private void CloseClient()
        {
            try
            {
                // definição da variavel EOT (end of transmission) do tipo array de byte
                byte[] eot = protocolSI.Make(ProtocolSICmdType.EOT); // // utilização de método Make.ProcolSICmdType serve para enviar dados

                //A classe networkstream disponibiliza métodos para enviar/receber
                // * dados através de socket Stream
                // * o socket de rede é um endpoint interno para o envio e recepcao de daddos com um PC presente na rede
                networkStream.Write(eot, 0, eot.Length);
                networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                networkStream.Close();
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        //Metodo para fechar a aplicação
        private void FormChat_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseClient();
            formlogin.Close();
           
        }

        private void tbMessage_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void FormChat_Load(object sender, EventArgs e)
        {

        }
    }
}


//Teste Adicionar coment«ario

