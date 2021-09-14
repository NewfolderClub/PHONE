/*
 * If this works phone company works
 */
using PHONE;
using System.Net;
using RedCorona.Net;
using System.Net.Sockets;
using System;
using System.Text;
namespace WindowsFormsApp1
{
    public static class Centrala
    {
        // public const int Port65534ReqALLPHONENUMBERS = 1;
        public const ushort Port65534 = 65534;
        public static inet.SimpleServer srv = new inet.SimpleServer();          // Server
        public static void ExchangePhoneNumbersWithAllP2PNetwork()
        {
            for (int i = 0; i < Form1.MAX; i++)
            {
                if (Form1.PhoneNumbers != null)
                {
                    if (Form1.PhoneNumbers[i] != null)
                    {
                        if (Form1.PhoneNumbers[i].ip != null)
                        {
                            // Conect to server
                            Client.ClientConnect(Form1.PhoneNumbers[i].ip);
                        }
                    }
                }
            }
        }

        public static class Server
        {
            public static RedCorona.Net.Server server;
            public static void ServerPort65534()
            {
                server = new RedCorona.Net.Server(Port65534, new ClientEvent(ClientConnect));
            }

            static bool ClientConnect(RedCorona.Net.Server serv, ClientInfo new_client)
            {
                new_client.Delimiter = '\n'.ToString();
                new_client.OnRead += new ConnectionRead(ReadData);

                // ik add
                srv.client = new_client;
                srv.client.OnReadBytes += new ConnectionReadBytes(IKReadData);

                return true; // allow this connection
            }

            static void ReadData(ClientInfo ci, String text)
            {
                Console.WriteLine("Received from " + ci.ID + ": " + text);
                if (text[0] == '!')
                    server.Broadcast(Encoding.UTF8.GetBytes(text));
                else ci.Send(text);
            }

            // ik add
            static void IKReadData(ClientInfo ci, byte[] data, int len)
            {
                // ik add
                // check received data
                // check if ENTER OR ESC
                /*
                if (data[0] == (Byte)Keys.Enter)
                {
                    // Send Generated number from SERVER
                    clnt.Send(("Generated number: " + RANDServer.counter.ToString() + "\n"));
                }
                else if (data[0] == (Byte)Keys.Escape)
                {
                    // EXIT the program
                    mf.Close();
                }
                */
            }
        }

        public static class Client
        {
            static ClientInfo client;
            public static void ClientConnect(IPAddress ip)
            {
                Socket sock = Sockets.CreateTCPSocket(ip.ToString(), Port65534);
                client = new ClientInfo(sock, true); // Start receiving yet
                client.OnReadBytes += new ConnectionReadBytes(ClientReadData);
                client.BeginReceive();
            }

            static void ClientReadData(ClientInfo ci, byte[] data, int len)
            {
                // Console.WriteLine("Received " + len + " bytes: " +
                System.Text.Encoding.UTF8.GetString(data, 0, len);
            }
        }
    }
}