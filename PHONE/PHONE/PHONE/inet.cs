using System.Net.Sockets;
using RedCorona.Net;
using System.Threading;
using System.Net;
using System;
using System.Windows.Forms;
using PHONE;
using System.Text;
namespace WindowsFormsApp1
{
    public class inet
    {

        public const String IZTOKKADOIC = "iztokkadoic";
        public const String Net = ".net";
        public const String www = "www.";
        public const String text = (www + IZTOKKADOIC + Net);

        public static Form1 mf = Form1.f1;

        // Internet
        // Server

        public const int Port = (65535);
        public static SimpleServer srv = null;          // Server

        public class SimpleServer
        {
            Server server;
            public ClientInfo client;
            public void Start()
            {
                server = new Server(Port, new ClientEvent(ClientConnect));
            }

            bool ClientConnect(Server serv, ClientInfo new_client)
            {
                new_client.Delimiter = '\n'.ToString();
                new_client.OnRead += new ConnectionRead(ReadData);

                // ik add
                srv.client = new_client;
                srv.client.OnReadBytes += new ConnectionReadBytes(IKReadData);

                return true; // allow this connection
            }

            // ik add
            void IKReadData(ClientInfo ci, byte[] data, int len)
            {
                // ik add
                // check received data
                // check if ENTER OR ESC
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
            }

            void ReadData(ClientInfo ci, String text)
            {
                Console.WriteLine("Received from " + ci.ID + ": " + text);
                if (text[0] == '!')
                    server.Broadcast(Encoding.UTF8.GetBytes(text));
                else ci.Send(text);
            }
        }

        private void Server_Load(object sender, EventArgs e)
        {
            srv = new SimpleServer();
            srv.Start();

            // ik add
            // check if connected
            RANDServer.ts();                                                        // Start new thread
        }

        // Server
        // Program
        public static class RANDServer
        {
            public const int MIN_NUM = 1;
            public const int MAX_NUM = 1000;
            public static int counter = MIN_NUM;
            public static Thread t = null;
            public static void Run()
            {
                // RUN RAND
                clnt.Send("RAND\r\n\r\n");
                clnt.Send("100 numbers random generator\r\n");
                clnt.Send("This will generate 100 numbers from 1 to 1000 on \"ENTER\" key press\r\n\r\n");
                clnt.Send("Press \"ENTER\" key to generate number or \"ESC\" o exit ...\r\n\r\n");
            }

            public static void ts()
            {
                t = new Thread(new ThreadStart(tf));
                t.Start();
            }

            public static void tf()
            {
                while (clnt == null)
                { }
                Run();
                while (clnt != null)
                {
                    // Increment counter
                    if (RANDServer.counter < MAX_NUM)
                    {
                        RANDServer.counter++;
                    }
                    else
                    {
                        RANDServer.counter = MIN_NUM;
                    }
                }
            }
        }

        // Client
        public static String ConnectToHostName = (IZTOKKADOIC + Net);

        public static ClientInfo clnt = null;

        class SimpleClient
        {
            ClientInfo client;
            public void Start()
            {
                Socket sock = Sockets.CreateTCPSocket(ConnectToHostName, Port);
                client = new ClientInfo(sock, false); // Don't start receiving yet

                // ik add
                clnt = client;

                client.OnReadBytes += new ConnectionReadBytes(ReadData);
                client.BeginReceive();
            }

            void ReadData(ClientInfo ci, byte[] data, int len)
            {
                // ik add
                // check received data and display it                
                // mf.SetText(System.Text.Encoding.UTF8.GetString(data, 0, len));                // Napaka !!!
                                                                                              //                mf.listBox1.Items.Add(data.ToString()); 

                Console.WriteLine("Received " + len + " bytes: " +
                System.Text.Encoding.UTF8.GetString(data, 0, len));
            }
        }


        private void IK_Client_Connect(object sender, EventArgs e)
        {
            SimpleClient client = new SimpleClient();
            client.Start();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            clnt.Send(e.KeyChar.ToString());
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (RANDServer.t != null)
            {
                RANDServer.t.Abort();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripTextBox1_TextChanged(sender, e);
            Server_Load(sender, e);
            IK_Client_Connect(sender, e);
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            // ConnectToHostName = toolStripTextBox1.Text;
        }

        private void connectToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            toolStripTextBox1_TextChanged(sender, e);
            IK_Client_Connect(sender, e);
        }

        public String GetHostName()
        {
            return System.Net.Dns.GetHostName();
        }
    }
}