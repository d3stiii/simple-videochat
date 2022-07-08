using System.Drawing.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;
using Client.Packets;
using Client.UI;

namespace Client.Services;

public class VideoCaptureService : IVideoCaptureService {
    private readonly INetworkService _networkService;
    private readonly ComboBox _cameraSelection;
    private readonly IVideoPlayerService _videoPlayerService;
    private VideoCaptureDevice _videoSource;

    public VideoCaptureService(INetworkService networkService, ComboBox cameraSelection,
        IVideoPlayerService videoPlayerService) {
        _networkService = networkService;
        _cameraSelection = cameraSelection;
        _videoPlayerService = videoPlayerService;
    }

    public void Init() {
        if (_cameraSelection.SelectedItem == null) return;
        _videoSource =
            new VideoCaptureDevice(((CameraComboItem)_cameraSelection.SelectedItem).CameraInfo.MonikerString);
        _networkService.OnDisconnected += Stop;
        _networkService.OnConnected += Start;
    }

    private void Start() {
        _videoSource.NewFrame += OnNewFrame;
        _videoSource.Start();
    }

    private void Stop() {
        _videoSource.SignalToStop();
    }

    private void OnNewFrame(object sender, NewFrameEventArgs eventArgs) {
        var bitmapForSend = new Bitmap(eventArgs.Frame);
        var bitmapForDisplay = new Bitmap(eventArgs.Frame, 165, 120);
        _videoPlayerService.ShowMyCamera(bitmapForDisplay);
        SendVideo(bitmapForSend);
    }

    private void SendVideo(Bitmap bitmap) {
        using var packet = new Packet((int)ClientPackets.Video);
        using var memoryStream = new MemoryStream();

        bitmap.Save(memoryStream, ImageFormat.Jpeg);
        var bytes = memoryStream.ToArray();
        packet.Write(bytes.Length);
        packet.Write(bytes);

        _networkService.SendData(packet);
    }
}