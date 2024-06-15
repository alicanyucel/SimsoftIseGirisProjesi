using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class ClientClass
{
    private TcpClient _client;
    private NetworkStream _stream;
    private Thread _listenerThread;

    public event Action<int, int> PositionUpdated;

    public ClientClass(string ip, int port)
    {
        _client = new TcpClient(ip, port);
        _stream = _client.GetStream();
        _listenerThread = new Thread(ListenForServerMessages);
        _listenerThread.Start();
    }

    public void SendData(string data)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(data);
        _stream.Write(buffer, 0, buffer.Length);
    }

    private void ListenForServerMessages()
    {
        byte[] buffer = new byte[1024];
        int bytesRead;

        while ((bytesRead = _stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            string[] positions = data.Split(',');
            if (positions.Length == 2 && int.TryParse(positions[0], out int x) && int.TryParse(positions[1], out int y))
            {
                PositionUpdated?.Invoke(x, y);
            }
        }
    }
}
