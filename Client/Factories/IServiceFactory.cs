using Client.Services;

namespace Client.Factories;

public interface IServiceFactory {
    IPacketHandlersProvider CreateHandlerProvider();
    IDevicesProvider CreateDevicesProvider();
    IVideoPlayerService CreateVideoPlayerService(PictureBox videoBox, PictureBox myVideoBox);
    IAudioPlayerService CreateAudioPlayerService();
    INetworkService CreateNetworkService(Label infoLabel, IPacketHandlersProvider packetHandlersProvider);
    IAudioCaptureService CreateAudioCaptureService(INetworkService networkService, ComboBox micSelection);

    IVideoCaptureService CreateVideoCaptureService(INetworkService networkService,
        IVideoPlayerService videoPlayerService, ComboBox cameraSelection);
}