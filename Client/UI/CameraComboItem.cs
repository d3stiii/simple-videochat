using AForge.Video.DirectShow;

namespace Client.UI;

public class CameraComboItem {
    public FilterInfo CameraInfo { get; init; }

    public override string ToString() {
        return CameraInfo.Name;
    }
}