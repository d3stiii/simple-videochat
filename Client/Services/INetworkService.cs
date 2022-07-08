using Client.Packets;

namespace Client.Services;

public interface INetworkService : IService {
    event Action OnConnected;
    event Action OnDisconnected;
    void Connect(string ip, int port);
    void SendData(Packet packet);
    void Disconnect();
}