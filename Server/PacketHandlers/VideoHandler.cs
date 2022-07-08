using Server.Packets;
using Server.Server;
using Server.Services;

namespace Server.PacketHandlers;

public class VideoHandler : IPacketHandler {
    public ClientPackets PacketId => ClientPackets.Video;

    public void Execute(Packet packet, string authorId) {
        var bytesCount = packet.ReadInt();
        if (bytesCount > packet.UnreadLength())
            return;

        SendVideo(packet.ReadBytes(bytesCount), authorId);
    }

    private void SendVideo(byte[] bitmap, string authorId) {
        using var packet = new Packet((int)ServerPackets.Video);

        packet.Write(bitmap.Length);
        packet.Write(bitmap);

        IServer server = AllServices.Container.Single<IServer>();
        server.SendDataToAll(packet, authorId);
    }
}