using Client.PacketHandlers;
using Client.Packets;

namespace Client.Services;

public interface IPacketHandlersProvider : IService {
    void InitHandlers();
    IPacketHandler? GetHandler(ServerPackets packetId);
}