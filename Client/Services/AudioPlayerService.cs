using NAudio.Wave;

namespace Client.Services;

internal class AudioPlayerService : IAudioPlayerService {
    public void PlayAudio(byte[] audioBytes) {
        var waveOut = new WaveOut();
        IWaveProvider provider = new RawSourceWaveStream(new MemoryStream(audioBytes), new WaveFormat());
        waveOut.Init(provider);
        waveOut.Play();
    }
}