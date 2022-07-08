namespace Server.Client;

public interface IClient {
    string Id { get; }
    void Process();
    void SendData(byte[] data);
    void Close();
}