using System.Net;
using System.Net.Sockets;
using Server.Client;
using Server.Packets;
using Server.Services;

namespace Server.Server;

public class Server : IServer {
    private readonly List<IClient> _clients;
    private readonly int _port;
    private readonly IPacketHandlerProvider _handlersProvider;
    private TcpListener _listener;

    public Server(int port, IPacketHandlerProvider handlersProvider) {
        _port = port;
        _handlersProvider = handlersProvider;
        _clients = new List<IClient>();
    }

    public void Start() {
        StartListener();
        Console.WriteLine("Server started.");
    }

    public void RemoveClient(string id) {
        var client = _clients.FirstOrDefault(client => client.Id == id);
        if (client != null && _clients.Contains(client)) _clients.Remove(client);
    }

    public void AddClient(IClient client) {
        _clients.Add(client);
    }

    public void Stop() {
        _listener.Stop();
        foreach (var client in _clients) client.Close();
        Environment.Exit(0);
    }

    public void SendDataToAll(Packet packet, string authorId) {
        for (var i = 0; i < _clients.Count; i++)
            if (_clients[i].Id != authorId)
                _clients[i].SendData(packet.ToArray());
    }

    private void StartListener() {
        try {
            _listener = new TcpListener(IPAddress.Any, _port);
            _listener.Start();
            _listener.BeginAcceptTcpClient(OnConnecting, null);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            Stop();
        }
    }

    private void OnConnecting(IAsyncResult result) {
        var tcpClient = _listener.EndAcceptTcpClient(result);
        _listener.BeginAcceptTcpClient(OnConnecting, null);
        Console.WriteLine($"Trying to connect {tcpClient.Client.RemoteEndPoint}");

        IClient client = new Client.Client(tcpClient, this, _handlersProvider);
        var clientThread = new Thread(client.Process);
        clientThread.Start();
    }
}