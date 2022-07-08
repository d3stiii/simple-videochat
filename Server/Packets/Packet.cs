using System.Drawing;
using System.Text;

namespace Server.Packets;

public class Packet : IDisposable {
    private List<byte> _buffer;

    private bool _disposed;
    private byte[] _readableBuffer;
    private int _readPos;

    public Packet() {
        _buffer = new List<byte>();
        _readPos = 0;
    }

    public Packet(int id) {
        _buffer = new List<byte>();
        _readPos = 0;

        Write(id);
    }

    public Packet(byte[] data) {
        _buffer = new List<byte>();
        _readPos = 0;

        SetBytes(data);
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
        if (!_disposed) {
            if (disposing) {
                _buffer = null;
                _readableBuffer = null;
                _readPos = 0;
            }

            _disposed = true;
        }
    }

    public void SetBytes(byte[] data) {
        Write(data);
        _readableBuffer = _buffer.ToArray();
    }

    public void WriteLength() {
        _buffer.InsertRange(0,
            BitConverter.GetBytes(_buffer.Count));
    }

    public void InsertInt(int value) {
        _buffer.InsertRange(0, BitConverter.GetBytes(value));
    }

    public byte[] ToArray() {
        _readableBuffer = _buffer.ToArray();
        return _readableBuffer;
    }

    public int Length() {
        return _buffer.Count;
    }

    public int UnreadLength() {
        return Length() - _readPos;
    }

    public void Reset(bool shouldReset = true) {
        if (shouldReset) {
            _buffer.Clear();
            _readableBuffer = null;
            _readPos = 0;
        }
        else {
            _readPos -= 4;
        }
    }

    public void Write(byte value) {
        _buffer.Add(value);
    }

    public void Write(byte[] value) {
        _buffer.AddRange(value);
    }

    public void Write(short value) {
        _buffer.AddRange(BitConverter.GetBytes(value));
    }

    public void Write(int value) {
        _buffer.AddRange(BitConverter.GetBytes(value));
    }

    public void Write(long value) {
        _buffer.AddRange(BitConverter.GetBytes(value));
    }

    public void Write(float value) {
        _buffer.AddRange(BitConverter.GetBytes(value));
    }

    public void Write(bool value) {
        _buffer.AddRange(BitConverter.GetBytes(value));
    }

    public void Write(string? value) {
        Write(value.Length);
        _buffer.AddRange(Encoding.GetEncoding(1251).GetBytes(value));
    }

    public byte ReadByte(bool moveReadPos = true) {
        if (_buffer.Count > _readPos) {
            var value = _readableBuffer[_readPos];
            if (moveReadPos) _readPos += 1;

            return value;
        }

        throw new Exception("Could not read value of type 'byte'!");
    }

    public byte[] ReadBytes(int length, bool moveReadPos = true) {
        if (_buffer.Count > _readPos) {
            var value =
                _buffer.GetRange(_readPos, length)
                    .ToArray();
            if (moveReadPos) _readPos += length;

            return value;
        }

        throw new Exception("Could not read value of type 'byte[]'!");
    }

    public short ReadShort(bool moveReadPos = true) {
        if (_buffer.Count > _readPos) {
            var value = BitConverter.ToInt16(_readableBuffer, _readPos);
            if (moveReadPos) _readPos += 2;

            return value;
        }

        throw new Exception("Could not read value of type 'short'!");
    }

    public int ReadInt(bool moveReadPos = true) {
        if (_buffer.Count > _readPos) {
            var value = BitConverter.ToInt32(_readableBuffer, _readPos);
            if (moveReadPos) _readPos += 4;

            return value;
        }

        throw new Exception("Could not read value of type 'int'!");
    }

    public long ReadLong(bool moveReadPos = true) {
        if (_buffer.Count > _readPos) {
            var value = BitConverter.ToInt64(_readableBuffer, _readPos);
            if (moveReadPos) _readPos += 8;

            return value;
        }

        throw new Exception("Could not read value of type 'long'!");
    }

    public float ReadFloat(bool moveReadPos = true) {
        if (_buffer.Count > _readPos) {
            var value = BitConverter.ToSingle(_readableBuffer, _readPos);
            if (moveReadPos) _readPos += 4;

            return value;
        }

        throw new Exception("Could not read value of type 'float'!");
    }

    public bool ReadBool(bool moveReadPos = true) {
        if (_buffer.Count > _readPos) {
            var value = BitConverter.ToBoolean(_readableBuffer, _readPos);
            if (moveReadPos) _readPos += 1;

            return value;
        }

        throw new Exception("Could not read value of type 'bool'!");
    }

    public string? ReadString(bool moveReadPos = true) {
        try {
            var length = ReadInt();
            var value =
                Encoding.GetEncoding(1251)
                    .GetString(_readableBuffer, _readPos, length);
            if (moveReadPos && value.Length > 0) _readPos += length;

            return value;
        }
        catch {
            throw new Exception("Could not read value of type 'string'!");
        }
    }

    public Bitmap ReadBitmap(int bytesCount, bool moveReadPos = true) {
        var bytes = ReadBytes(bytesCount, moveReadPos);
        using (var memoryStream = new MemoryStream(bytes)) {
            return new Bitmap(memoryStream);
        }
    }
}