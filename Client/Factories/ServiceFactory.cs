using Client.Services;
using Client.Services.ServiceLocator;

namespace Client.Factories;

public class ServiceFactory : IServiceFactory {
    public IPacketHandlersProvider CreateHandlerProvider() {
        IPacketHandlersProvider handlersProvider = new PacketHandlersProvider();
        handlersProvider.InitHandlers();
        return handlersProvider;
    }

    public IDevicesProvider CreateDevicesProvider() =>
        new DevicesProvider();

    public IVideoPlayerService CreateVideoPlayerService(PictureBox videoBox, PictureBox myVideoBox) {
        IVideoPlayerService videoPlayerService = new VideoPlayerService(videoBox, myVideoBox);
        AllServices.Container.RegisterSingle(videoPlayerService);
        return videoPlayerService;
    }

    public IAudioPlayerService CreateAudioPlayerService() {
        IAudioPlayerService audioPlayerService = new AudioPlayerService();
        AllServices.Container.RegisterSingle(audioPlayerService);
        return audioPlayerService;
    }

    public INetworkService CreateNetworkService(Label infoLabel, IPacketHandlersProvider packetHandlersProvider) =>
        new NetworkService(infoLabel, packetHandlersProvider);

    public IAudioCaptureService CreateAudioCaptureService(INetworkService networkService, ComboBox micSelection) =>
        new AudioCaptureService(networkService, micSelection);

    public IVideoCaptureService CreateVideoCaptureService(INetworkService networkService,
        IVideoPlayerService videoPlayerService, ComboBox cameraSelection) =>
        new VideoCaptureService(networkService, cameraSelection, videoPlayerService);
}