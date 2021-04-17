using EI.SI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServer
{
    class Program
    {
        // Defenir constante da Porta
        private const int PORT = 10000;
        static void Main(string[] args)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, PORT);
            TcpListener listener = new TcpListener(endPoint);

            //Iniciar o listener; apresentacao da primeira mensagem na linha de comandos e inicializacao do contador
            listener.Start();
            Console.WriteLine("SERVER READY ");
            int clientCounter = 0;


            //servidor estar constantemente à escuta
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient(); // Para receber comunicações
                clientCounter++;
                Console.WriteLine("Client connected!"); // Mensagem no servidor

                //Varivavel clientHandle do tipo TcpClient
                ClientHandler clientHandler = new ClientHandler(client, clientCounter);
                clientHandler.Handle();
            }
        }
    }
    class ClientHandler
    {
        //Varivaveis client e clientID
        private TcpClient client;
        private int clientID;

        //construtor
        public ClientHandler(TcpClient client, int clientID)
        {
            this.client = client;
            this.clientID = clientID;
        }

        public void Handle()
        {
            // defeicao da variavel thread e arranque da mesma 
            // threads sao unidades de execução dentro de um processo, é possivel varias instrucoes em simultaneo
            Thread thread = new Thread(threadHandler);
            thread.Start();
        }
        private void threadHandler()
        {
            //definir variaveis networkStream e protocolSI
            NetworkStream networkStream = this.client.GetStream();
            ProtocolSI protocolSI = new ProtocolSI();


           
            while (protocolSI.GetCmdType() != ProtocolSICmdType.EOT) //ProtocolSICmdType is the first byte of the communication protocol
            {
                int bytesRead = networkStream.Read(protocolSI.Buffer, 0, protocolSI.Buffer.Length);
                byte[] ack; //(acknolage) With this type should be sent a "message" with the "value" of the ACK.

                switch (protocolSI.GetCmdType())

                {
                    case ProtocolSICmdType.DATA: //To send DATA in clear text format.
                        Console.WriteLine("Client " + clientID + ": " + protocolSI.GetStringFromData());
                        ack = protocolSI.Make(ProtocolSICmdType.ACK);  //Make(): cria uma mensagem/ pacote de um tipo específico
                        networkStream.Write(ack, 0, ack.Length);
                        break;

                    case ProtocolSICmdType.EOT: //To send the signal of End of Transmission EOT.
                        Console.WriteLine("Ending Thread from client {0}", clientID);
                        ack = protocolSI.Make(ProtocolSICmdType.ACK);
                        networkStream.Write(ack, 0, ack.Length);
                        break;

                    default:
                        break;
                }
            }

            //Fecho do netWorkStream e do cliente (tcpClient)
            networkStream.Close();
            client.Close();

        }
    }
}


