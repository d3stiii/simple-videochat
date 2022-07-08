using Client.UI;
using NAudio.Wave;

namespace Client.Services;

public interface IDevicesProvider : IService {
    List<CameraComboItem> Cameras { get; }
    List<WaveInCapabilities> Mics { get; }
    void LoadDevices();
}