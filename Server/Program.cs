using Server.Server;
using Server.Services;

namespace Server;

public static class Program {
    public static void Main() {
        AllServices services = AllServices.Container;
        RegisterServices(services);
        StartServer(services);

        Console.ReadLine();
    }

    private static void StartServer(AllServices services) {
        IServer? server = AllServices.Container.Single<IServer>();
        try {
            server.Start();
        }
        catch (Exception e) {
            server?.Stop();
            Console.WriteLine(e);
        }
    }

    private static void RegisterServices(AllServices services) {
        RegisterHandlersProvider(services);
        services.RegisterSingle<IServer>(new Server.Server(25565, AllServices.Container.Single<IPacketHandlerProvider>()));
    }

    private static void RegisterHandlersProvider(AllServices services) {
        IPacketHandlerProvider handlerProvider = new PacketHandlerProvider();
        handlerProvider.InitHandlers();
        services.RegisterSingle<IPacketHandlerProvider>(handlerProvider);
    }
}