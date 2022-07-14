using System.Net.Sockets;
using Server.PacketHandlers;
using Server.Packets;
using Server.Server;
using Server.Services;

namespace Server.Client;

public class Client : IClient {
    public string Id { get; }

    private const int BufferSize = 40980;
    private readonly IServer _server;
    private readonly IPacketHandlerProvider _packetHandlerProvider;
    private byte[]? _receiveBuffer;
    private TcpClient? _client;
    private NetworkStream? _stream;
    private Packet? _packet;

    public Client(TcpClient? client, IServer server, IPacketHandlerProvider packetHandlerProvider) {
        Id = Guid.NewGuid().ToString();
        _client = client;
        _server = server;
        _packetHandlerProvider = packetHandlerProvider;
        _stream = _client.GetStream();
        _packet = new Packet();
        _receiveBuffer = new byte[BufferSize];
        _server.AddClient(this);
    }

    public void Process() {
        Console.WriteLine($"{_client?.Client.RemoteEndPoint} as {Id} connected.");
        _client.ReceiveBufferSize = BufferSize;
        _client.SendBufferSize = BufferSize;
        _stream?.BeginRead(_receiveBuffer, 0, BufferSize, OnDataReceived, null);
    }

    public void Close() {
        Console.WriteLine($"{_client?.Client?.RemoteEndPoint} with id {Id} disconnected.");
        _client?.Close();
        _stream?.Close();
        _packet = null;
        _receiveBuffer = null;
        _client = null;
        _stream = null;
        _server.RemoveClient(Id);
    }

    public void SendData(byte[] data) {
        try {
            if (_client is { Connected: true } && data.Length > 0 && _stream.CanWrite)
                _stream.BeginWrite(data, 0, data.Length, null, null);
        }
        catch (Exception e) {
            Console.WriteLine(e.Message);
            Close();
        }
    }

    private void OnDataReceived(IAsyncResult result) {
        try {
            if (_stream is { CanRead: true }) {
                var bytesLength = _stream.EndRead(result);
                if (bytesLength <= 0) {
                    Console.WriteLine($"{Id} sent <= 0 bytes");
                    Close();
                    return;
                }

                var data = new byte[bytesLength];
                Array.Copy(_receiveBuffer, data, bytesLength);

                _packet.Reset(HandleData(data));

                _stream.BeginRead(_receiveBuffer, 0, BufferSize, OnDataReceived, null);
            }
        }
        catch (Exception e) {
            Console.WriteLine(e.Message);
            Close();
        }
    }

    private bool HandleData(byte[] data) {
        try {
            var packetLength = 0;
            _packet.SetBytes(data);

            if (_packet.UnreadLength() >= 4) {
                packetLength = _packet.UnreadLength();
                if (packetLength <= 0) return true;
            }

            while (packetLength > 0 && packetLength <= _packet.UnreadLength()) {
                var packetBytes = _packet.ReadBytes(packetLength);

                using Packet packet = new(packetBytes);
                int packetId = 0;
                try {
                    packetId = packet.ReadInt();
                    IPacketHandler handler = _packetHandlerProvider.GetHandler((ClientPackets)packetId)!;
                    handler?.Execute(packet, Id);
                }
                catch (Exception e) {
                    Console.WriteLine($"Unhandled packet. Packet id: {packetId}. Error: {e}");
                }

                packetLength = 0;

                if (_packet.UnreadLength() >= 4) {
                    packetLength = _packet.ReadInt();
                    if (packetLength <= 0) return true;
                }
            }

            return packetLength <= 1;
        }
        catch (Exception e) {
            Console.WriteLine(e.Message);
        }

        return true;
    }
}