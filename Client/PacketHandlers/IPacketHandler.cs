using Client.Packets;

namespace Client.PacketHandlers;

public interface IPacketHandler {
    ServerPackets PacketId { get; }
    void Execute(Packet packet);
}