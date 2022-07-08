using Server.Packets;

namespace Server.PacketHandlers;

public interface IPacketHandler {
    ClientPackets PacketId { get; }
    void Execute(Packet packet, string authorId);
}