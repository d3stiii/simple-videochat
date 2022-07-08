using Client.Factories;
using Client.Services;
using Client.UI;

namespace Client;

public partial class Form1 : Form {
    private readonly IServiceFactory _serviceFactory = new ServiceFactory();
    private INetworkService _networkService;
    private IVideoCaptureService _videoCaptureService;
    private IAudioCaptureService _audioCaptureService;

    public Form1() =>
        InitializeComponent();

    private void ConnectButton_Click(object sender, EventArgs e) {
        try {
            _videoCaptureService.Init();
            _audioCaptureService.Init();
            _networkService.Connect(IpField.Text, int.Parse(PortField.Text));
            _networkService.OnConnected += ShowVideoBoxes;
            _networkService.OnDisconnected += HideVideoBoxes;
        }
        catch (Exception exception) {
            ConnectionInfoLabel.Text = exception.Message;
        }
    }

    private void Form1_Shown(object sender, EventArgs e) {
        InitializeServices();
    }

    private void Form1_FormClosed(object sender, FormClosedEventArgs e) =>
        _networkService?.Disconnect();

    private void HideVideoBoxes() {
        VideoBox.Invoke(() => VideoBox.Hide());
        MyVideoBox.Invoke(() => MyVideoBox.Hide());
    }

    private void ShowVideoBoxes() {
        VideoBox.Invoke(() => VideoBox.Show());
        MyVideoBox.Invoke(() => MyVideoBox.Show());
    }

    private void InitializeServices() {
        IPacketHandlersProvider handlersProvider = _serviceFactory.CreateHandlerProvider();
        IVideoPlayerService videoPlayerService = _serviceFactory.CreateVideoPlayerService(VideoBox, MyVideoBox);
        IAudioPlayerService audioPlayerService = _serviceFactory.CreateAudioPlayerService();
        _networkService = _serviceFactory.CreateNetworkService(ConnectionInfoLabel, handlersProvider);
        _videoCaptureService =
            _serviceFactory.CreateVideoCaptureService(_networkService, videoPlayerService, CameraSelection);
        _audioCaptureService = _serviceFactory.CreateAudioCaptureService(_networkService, MicSelection);
        InitializeDevices();
    }

    private void InitializeDevices() {
        IDevicesProvider devicesProvider = _serviceFactory.CreateDevicesProvider();
        devicesProvider.LoadDevices();
        AddDevicesToBoxes(devicesProvider);
    }

    private void AddDevicesToBoxes(IDevicesProvider devicesProvider) {
        AddCameras(devicesProvider);
        AddMics(devicesProvider);
    }

    private void AddMics(IDevicesProvider devicesProvider) {
        var mics = devicesProvider.Mics;
        foreach (var mic in mics)
            MicSelection.Items.Add(mic.ProductName);
    }

    private void AddCameras(IDevicesProvider devicesProvider) {
        List<CameraComboItem> cameras = devicesProvider.Cameras;
        for (int i = 0; i < devicesProvider.Cameras.Count; i++)
            CameraSelection.Items.Add(cameras[i]);
    }
}