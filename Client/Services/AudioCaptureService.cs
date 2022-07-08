using Client.Packets;
using NAudio.Wave;

namespace Client.Services;

public class AudioCaptureService : IAudioCaptureService {
    private readonly WaveIn _audioSource;
    private readonly INetworkService _networkService;
    private readonly ComboBox _micSelection;

    public AudioCaptureService(INetworkService networkService, ComboBox micSelection) {
        _networkService = networkService;
        _micSelection = micSelection;
        _audioSource = new WaveIn();
    }

    public void Init() {
        if (_micSelection.SelectedItem == null) return;
        _audioSource.WaveFormat = new WaveFormat(44100, WaveIn.GetCapabilities(_micSelection.SelectedIndex).Channels);
        _audioSource.DeviceNumber = _micSelection.SelectedIndex;
        _audioSource.DataAvailable += OnAudioCaptured;
        _networkService.OnConnected += Record;
        _networkService.OnDisconnected += StopRecord;
    }

    private void StopRecord() =>
        _audioSource.StopRecording();

    private void Record() {
        try {
            _audioSource.StartRecording();
        }
        catch { }
    }

    private void OnAudioCaptured(object? sender, WaveInEventArgs e) =>
        SendAudio(e);

    private void SendAudio(WaveInEventArgs waveInEventArgs) {
        using var packet = new Packet((int)ClientPackets.Audio);
        var audioData = waveInEventArgs.Buffer;
        packet.Write(audioData.Length);
        packet.Write(audioData);

        _networkService.SendData(packet);
    }
}

public interface IAudioCaptureService : IService {
    void Init();
}