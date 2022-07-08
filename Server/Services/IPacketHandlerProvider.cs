using Server.PacketHandlers;
using Server.Packets;
using Server.Server;

namespace Server.Services;

public interface IPacketHandlerProvider : IService {
    void InitHandlers(IServer server);
    IPacketHandler? GetHandler(ClientPackets packetId);
}