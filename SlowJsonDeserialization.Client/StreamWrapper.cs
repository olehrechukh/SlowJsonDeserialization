namespace SlowJsonDeserialization.Client;

internal class StreamWrapper : Stream
{
    private readonly Stream stream;
    public int BytesRead;

    public StreamWrapper(Stream stream)
    {
        this.stream = stream;
    }

    public override bool CanRead => stream.CanRead;
    public override bool CanSeek => stream.CanSeek;
    public override bool CanWrite => stream.CanWrite;
    public override long Length => stream.Length;

    public override long Position
    {
        get => stream.Position;
        set => stream.Position = value;
    }

    public override void Flush()
    {
        stream.Flush();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return stream.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        stream.SetLength(value);
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return stream.Read(buffer, offset, count);
    }

    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        return stream.ReadAsync(buffer, offset, count, cancellationToken);
    }

    public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        var num = await stream.ReadAsync(buffer, cancellationToken);

        BytesRead += num;
        return num;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        stream.Write(buffer, offset, count);
    }

    protected override void Dispose(bool disposing)
    {
        if (!disposing)
        {
            stream.Dispose();
        }
    }

    public override ValueTask DisposeAsync()
    {
        return stream.DisposeAsync();
    }
}