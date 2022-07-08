using Server.Packets;
using Server.Server;
using Server.Services;

namespace Server.PacketHandlers;

public class AudioHandler : IPacketHandler {
    public ClientPackets PacketId => ClientPackets.Audio;

    public void Execute(Packet packet, string authorId) {
        var bytesCount = packet.ReadInt();
        if (bytesCount > packet.UnreadLength())
            return;
        
        byte[] audioBytes = packet.ReadBytes(bytesCount);
        SendAudio(audioBytes, authorId);
    }

    private void SendAudio(byte[] audioBytes, string authorId) {
        using var packet = new Packet((int)ServerPackets.Audio);
        packet.Write(audioBytes.Length);
        packet.Write(audioBytes);
        IServer server = AllServices.Container.Single<IServer>();
        server.SendDataToAll(packet, authorId);
    }
}