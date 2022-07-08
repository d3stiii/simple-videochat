namespace Client.Services;

public class VideoPlayerService : IVideoPlayerService {
    private readonly PictureBox _myVideoBox;
    private readonly PictureBox _videoBox;

    public VideoPlayerService(PictureBox videoBox, PictureBox myVideoBox) {
        _videoBox = videoBox;
        _myVideoBox = myVideoBox;
    }

    public void ShowOtherCamera(Bitmap bitmap) =>
        _videoBox.Invoke(() => { _videoBox.Image = bitmap; });

    public void ShowMyCamera(Bitmap bitmap) =>
        _myVideoBox.Invoke(() => _videoBox.Image = bitmap);
}