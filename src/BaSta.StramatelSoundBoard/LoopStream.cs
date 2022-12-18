using NAudio.Wave;

namespace BaSta.StramatelSoundBoard;

internal class LoopStream : WaveStream
{
    readonly WaveStream _sourceStream;

    /// <summary>
    /// Creates a instance of the <see cref="LoopStream"/> class.
    /// </summary>
    /// <param name="sourceStream">The stream to read from. Note: the Read method of this stream should return 0 when it reaches the end or else we will not loop to the start again.</param>
    public LoopStream(WaveStream sourceStream)
    {
        _sourceStream = sourceStream;
        EnableLooping = true;
    }

    /// <summary>
    /// Use this to turn looping on or off.
    /// </summary>
    public bool EnableLooping { get; set; }

    /// <inheritdoc />
    public override WaveFormat WaveFormat => _sourceStream.WaveFormat;

    /// <inheritdoc />
    public override long Length => _sourceStream.Length;

    /// <inheritdoc />
    public override long Position
    {
        get => _sourceStream.Position;
        set => _sourceStream.Position = value;
    }

    /// <inheritdoc />
    public override int Read(byte[] buffer, int offset, int count)
    {
        int totalBytesRead = 0;

        while (totalBytesRead < count)
        {
            int bytesRead = _sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
            if (bytesRead == 0)
            {
                if (_sourceStream.Position == 0 || !EnableLooping)
                {
                    // something wrong with the source stream
                    break;
                }
                // loop
                _sourceStream.Position = 0;
            }
            totalBytesRead += bytesRead;
        }
        return totalBytesRead;
    }
}