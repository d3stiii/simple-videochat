using Server.Client;
using Server.Packets;
using Server.Services;

namespace Server.Server;

public interface IServer : IService {
    void Start();
    void RemoveClient(string id);
    void AddClient(IClient client);
    void Stop();
    void SendDataToAll(Packet packet, string authorId);
}