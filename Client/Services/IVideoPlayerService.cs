namespace Client.Services;

public interface IVideoPlayerService : IService {
    void ShowOtherCamera(Bitmap bitmap);
    void ShowMyCamera(Bitmap bitmap);
}