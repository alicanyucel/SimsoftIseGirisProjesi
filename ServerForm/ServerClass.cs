using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Server
{
    private TcpListener _listener;
    private List<TcpClient> _clients = new List<TcpClient>();
    private object _lock = new object();

    public event Action<string> LogUpdated;

    public Server(int port)
    {
        _listener = new TcpListener(IPAddress.Any, port);
    }

    public void Start()
    {
        _listener.Start();
        Thread listenerThread = new Thread(AcceptClients);
        listenerThread.Start();
        LogUpdated?.Invoke("Sunucu başlatıldı...");
    }

    private void AcceptClients()
    {
        while (true)
        {
            TcpClient client = _listener.AcceptTcpClient();
            lock (_lock)
            {
                _clients.Add(client);
            }
            LogUpdated?.Invoke("Yeni bir istemci bağlandı...");
            Thread clientThread = new Thread(HandleClient);
            clientThread.Start(client);
        }
    }

    private void HandleClient(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead;

        try
        {
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                LogUpdated?.Invoke("Gelen veri: " + data);
                Broadcast(data, client);
            }
        }
        catch (Exception ex)
        {
            LogUpdated?.Invoke("İstemci bağlantısı kesildi: " + ex.Message);
        }
        finally
        {
            lock (_lock)
            {
                _clients.Remove(client);
            }
            client.Close();
        }
    }

    private void Broadcast(string data, TcpClient excludeClient)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(data);
        lock (_lock)
        {
            foreach (var client in _clients)
            {
                if (client != excludeClient)
                {
                    NetworkStream stream = client.GetStream();
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
        }
    }
}
