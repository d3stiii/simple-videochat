using Client.Packets;
using Client.Services;

namespace Client.PacketHandlers;

public class AudioHandler : IPacketHandler {
    public ServerPackets PacketId => ServerPackets.Audio;

    public void Execute(Packet packet) {
        var bytesCount = packet.ReadInt();
        var audioBytes = packet.ReadBytes(bytesCount);
        AllServices.Container.Single<IAudioPlayerService>()?.PlayAudio(audioBytes);
    }
}