using Server.Packets;
using Server.Server;

namespace Server.PacketHandlers;

public class AudioHandler : IPacketHandler {
    private readonly IServer _server;
    public ClientPackets PacketId => ClientPackets.Audio;

    public AudioHandler(IServer server) => _server = server;

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
        _server.SendDataToAll(packet, authorId);
    }
}