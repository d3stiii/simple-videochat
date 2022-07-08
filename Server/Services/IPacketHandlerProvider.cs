using Server.PacketHandlers;
using Server.Packets;

namespace Server.Services;

public interface IPacketHandlerProvider : IService {
    void InitHandlers();
    IPacketHandler? GetHandler(ClientPackets packetId);
}