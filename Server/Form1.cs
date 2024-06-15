using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Server
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

public class Server
        {
            private TcpListener _server;
            private bool _isRunning;

            public Server(string ip, int port)
            {
                _server = new TcpListener(IPAddress.Parse(ip), port);
                _server.Start();
                _isRunning = true;
                Listen();
            }

            public void Listen()
            {
                while (_isRunning)
                {
                    try
                    {
                        TcpClient newClient = _server.AcceptTcpClient();
                        Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                        t.Start(newClient);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }

            public void HandleClient(object obj)
            {
                TcpClient client = (TcpClient)obj;
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Received: " + data);
                    stream.Write(buffer, 0, bytesRead);
                }

                client.Close();
            }
        }

    }
}
