using Server.Server;
using Server.Services;

namespace Server;

public static class Program {
    public static void Main() {
        StartServer();
        Console.ReadLine();
    }
    
    private static void StartServer() {
        IPacketHandlerProvider handlersProvider = new PacketHandlerProvider();
        IServer? server = new Server.Server(25565, handlersProvider);
        handlersProvider.InitHandlers(server);
        server.Start();
    }
}