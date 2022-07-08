using Client.Services;
using Client.UI;

namespace Client;

public partial class Form1 : Form {
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
        RegisterServices();
        AddDevicesToBoxes();
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

    private void RegisterServices() {
        var services = AllServices.Container;
        RegisterHandlersProvider(services);
        RegisterDevicesProvider(services);
        RegisterNetworkService(services);
        services.RegisterSingle<IVideoPlayerService>(new VideoPlayerService(VideoBox, MyVideoBox));
        services.RegisterSingle<IAudioPlayerService>(new AudioPlayerService());
        RegisterVideoCaptureService(services);
        RegisterAudioCaptureService(services);
    }

    private void RegisterAudioCaptureService(AllServices services) {
        _audioCaptureService = new AudioCaptureService(_networkService, MicSelection);
        services.RegisterSingle<IAudioCaptureService>(_audioCaptureService);
    }

    private void RegisterVideoCaptureService(AllServices services) {
        _videoCaptureService = new VideoCaptureService(services.Single<INetworkService>(),
            CameraSelection, services.Single<IVideoPlayerService>());
        services.RegisterSingle<IVideoCaptureService>(_videoCaptureService);
    }

    private void RegisterNetworkService(AllServices services) {
        _networkService = new NetworkService(ConnectionInfoLabel,
            services.Single<IPacketHandlersProvider>());
        services.RegisterSingle<INetworkService>(_networkService);
    }

    private void RegisterDevicesProvider(AllServices services) {
        IDevicesProvider devicesProvider = new DevicesProvider();
        services.RegisterSingle(devicesProvider);
        devicesProvider.LoadDevices();
    }

    private void RegisterHandlersProvider(AllServices services) {
        IPacketHandlersProvider handlersProvider = new PacketHandlersProvider();
        handlersProvider.InitHandlers();
        services.RegisterSingle(handlersProvider);
    }

    private void AddDevicesToBoxes() {
        IDevicesProvider devicesProvider = AllServices.Container.Single<IDevicesProvider>();
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