namespace Client.Services;

public interface IAudioPlayerService : IService {
    void PlayAudio(byte[] audioBytes);
}