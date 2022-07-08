using System.Net.Sockets;
using Client.PacketHandlers;
using Client.Packets;

namespace Client.Services;

public class NetworkService : INetworkService {
    public event Action OnConnected;
    public event Action OnDisconnected;
    
    private const int BufferSize = 40980;
    private readonly Label _connectionInfo;
    private readonly IPacketHandlersProvider _packetHandlersProvider;
    private TcpClient _client;
    private Packet _packet;
    private byte[] _receiveBuffer;
    private NetworkStream _stream;

    public NetworkService(Label connectionInfo, IPacketHandlersProvider packetHandlersProvider) {
        _connectionInfo = connectionInfo;
        _packetHandlersProvider = packetHandlersProvider;
    }

    public void Connect(string ip, int port) {
        _packet = new Packet();
        _client = new TcpClient();
        _client.ReceiveBufferSize = BufferSize;
        _client.SendBufferSize = BufferSize;
        _receiveBuffer = new byte[BufferSize];
        _client.BeginConnect(ip, port, ConnectCallback, null);
    }

    public void SendData(Packet packet) {
        try {
            var data = packet.ToArray();
            if (_client is { Connected: true } && data.Length > 0 && _stream.CanWrite)
                _stream.BeginWrite(data, 0, data.Length, null, null);
        }
        catch (Exception e) {
            Disconnect();
        }
    }

    public void Disconnect() {
        OnDisconnected?.Invoke();
        _client?.Close();
        _stream?.Close();
        _client = null;
        _stream = null;
        _receiveBuffer = null;
    }

    private void ConnectCallback(IAsyncResult result) {
        if (!_client.Client.Connected) {
            _connectionInfo.Invoke(() => _connectionInfo.Text = "Cant connect to server. Try again.");
            Disconnect();
            return;
        }

        OnConnected?.Invoke();

        _stream = _client.GetStream();
        _stream.BeginRead(_receiveBuffer, 0, BufferSize, DataReceivedCallback, null);
    }

    private void DataReceivedCallback(IAsyncResult result) {
        try {
            if (_stream is { CanRead: true }) {
                var bytesLength = _stream.EndRead(result);
                if (bytesLength <= 0) {
                    Disconnect();
                    return;
                }

                var data = new byte[bytesLength];
                Array.Copy(_receiveBuffer, data, bytesLength);
                _packet.Reset(HandleData(data));
                _stream.BeginRead(_receiveBuffer, 0, BufferSize, DataReceivedCallback, null);
            }
        }
        catch {
            Disconnect();
        }
    }

    private bool HandleData(byte[] data) {
        try {
            var bytesLenght = 0;

            _packet.SetBytes(data);

            if (_packet.UnreadLength() >= 4) {
                bytesLenght = _packet.UnreadLength();
                if (bytesLenght <= 0) return true;
            }

            while (bytesLenght > 0 && bytesLenght <= _packet.UnreadLength()) {
                var packetBytes = _packet.ReadBytes(bytesLenght);

                using Packet packet = new(packetBytes);

                try {
                    int packetId = packet.ReadInt();
                    IPacketHandler? handler = _packetHandlersProvider.GetHandler((ServerPackets)packetId);
                    handler?.Execute(packet);
                }
                catch (Exception e) {
                    Console.WriteLine($"Unhandled packet. Error: {e.Message}");
                    return true;
                }

                bytesLenght = 0;

                if (_packet.UnreadLength() >= 4) {
                    bytesLenght = _packet.ReadInt();
                    if (bytesLenght <= 0) return true;
                }
            }

            return bytesLenght <= 1;
        }
        catch (Exception e) {
            Console.WriteLine(e.Message);
        }

        return true;
    }
}