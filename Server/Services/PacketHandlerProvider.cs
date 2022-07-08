using System.Reflection;
using Server.PacketHandlers;
using Server.Packets;

namespace Server.Services;

public class PacketHandlerProvider : IPacketHandlerProvider {
    private Dictionary<ClientPackets, IPacketHandler> _handlers;
    
    public void InitHandlers() {
        var handlerParentType = typeof(IPacketHandler);
        _handlers = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => handlerParentType.IsAssignableFrom(type) && handlerParentType != type)
            .Select(type => (IPacketHandler)Activator.CreateInstance(type)!)
            .ToDictionary(handler => handler.PacketId, handler => handler);

        Console.WriteLine("Packets initialized!");
    }

    public IPacketHandler? GetHandler(ClientPackets packetId) =>
        _handlers.TryGetValue(packetId, out var packetHandler) ? packetHandler : null;
}