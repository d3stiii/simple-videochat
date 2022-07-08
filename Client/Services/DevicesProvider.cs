using AForge.Video.DirectShow;
using Client.UI;
using NAudio.Wave;

namespace Client.Services;

public class DevicesProvider : IDevicesProvider {
    public List<CameraComboItem> Cameras { get; } = new List<CameraComboItem>();
    public List<WaveInCapabilities> Mics { get; } = new List<WaveInCapabilities>();

    public void LoadDevices() {
        LoadMics();
        LoadCameras();
    }

    private void LoadMics() {
        for (var deviceId = 0; deviceId < WaveIn.DeviceCount; deviceId++) {
            var deviceInfo = WaveIn.GetCapabilities(deviceId);
            Mics.Add(deviceInfo);
        }
    }

    private void LoadCameras() {
        var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        for (var i = 0; i < videoDevices.Count; i++)
            Cameras.Add(new CameraComboItem { CameraInfo = videoDevices[i] });
    }
}