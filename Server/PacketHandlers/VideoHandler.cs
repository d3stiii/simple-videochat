using Server.Packets;
using Server.Server;

namespace Server.PacketHandlers;

public class VideoHandler : IPacketHandler {
    private readonly IServer _server;
    public ClientPackets PacketId => ClientPackets.Video;

    public VideoHandler(IServer server) => _server = server;

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
        
        _server.SendDataToAll(packet, authorId);
    }
}