using System.Reflection;
using Client.PacketHandlers;
using Client.Packets;

namespace Client.Services;

public class PacketHandlersProvider : IPacketHandlersProvider {
    private Dictionary<ServerPackets, IPacketHandler> _handlers;

    public void InitHandlers() {
        var handlerParentType = typeof(IPacketHandler);
        _handlers = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => handlerParentType.IsAssignableFrom(type) && handlerParentType != type)
            .Select(type => (IPacketHandler)Activator.CreateInstance(type)!)
            .ToDictionary(handler => handler.PacketId, handler => handler);

        Console.WriteLine("Packets initialized!");
    }

    public IPacketHandler? GetHandler(ServerPackets packetId) =>
        _handlers.TryGetValue(packetId, out var packetHandler) ? packetHandler : null;
}