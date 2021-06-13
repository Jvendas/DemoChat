using DemoChat.helpers;
using EI.SI;
using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace DemoChat
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            LogHelper.logToFile("=====================================================================");
            LogHelper.logToFile(" DemoChat Client | " + DateTime.Now.ToString());
            LogHelper.logToFile("=====================================================================");

            try
            {
                // iniciar a ligação TCP ao cliente
                int PORT = 10000;
                IPAddress IP_ADDRESS = IPAddress.Loopback;

                ProtocolSI protocolSI = new ProtocolSI();
                IPEndPoint endPoint = new IPEndPoint(IP_ADDRESS, PORT);
                TcpClient client = new TcpClient();
                NetworkStream networkStream;

                while (!client.Connected)
                {
                    try
                    {
                        client.Connect(endPoint);
                    }
                    catch (Exception)
                    {
                    }
                }

                LogHelper.logToFile("Cliente conectado ao servidor com sucesso");
                networkStream = client.GetStream();

                // inicializar o form do login
                FormLogin formLogin = new FormLogin(protocolSI, networkStream, client);

                // inicilizar a aplicação
                Application.EnableVisualStyles();
                Application.Run(formLogin);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
