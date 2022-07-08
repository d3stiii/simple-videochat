using Client.Packets;
using Client.Services;

namespace Client.PacketHandlers;

public class VideoHandler : IPacketHandler {
    public ServerPackets PacketId => ServerPackets.Video;

    public void Execute(Packet packet) {
        var bytesCount = packet.ReadInt();
        var bitmap = packet.ReadBitmap(bytesCount);
        AllServices.Container.Single<IVideoPlayerService>()?.ShowOtherCamera(bitmap);
    }
}