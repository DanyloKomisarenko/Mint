namespace Mint.Common.Buffer;

public abstract class AbstractByteBuf : IByteBuf
{
    protected byte[] bytes = Array.Empty<byte>();
    protected int readerIndex = 0, writerIndex = 0;

    public int Capacity()
    {
        return bytes.Length;
    }

    public void Clear()
    {
        readerIndex = writerIndex = 0;
    }

    public void Flip()
    {
        readerIndex = 0;
        writerIndex = Capacity();
    }

    public abstract byte ReadByte();

    public int ReaderIndex()
    {
        return readerIndex;
    }

    public abstract void WriteByte(byte b);

    public int WriterIndex()
    {
        return writerIndex;
    }

    /*
    * <summary>
    * Writes multple bytes to the buffer and increases
    * the Writer Index accordingly.
    * </summary>
    */
    public void WriteBytes(byte[] bytes)
    {
        for (int i = 0; i < bytes.Length; i++)
        {
            WriteByte(bytes[i]);
        }
    }

    /*
     * <summary>
     * Reads multple bytes from the buffer and increases
     * the Reader Index accordingly.
     * </summary>
     */
    public byte[] ReadBytes(int len)
    {
        byte[] bytes = new byte[len];
        for (int i = 0; i < len; i++)
        {
            bytes[i] = ReadByte();
        }
        return bytes;
    }

    /*
     * <summary>
     * Returns the amount of bytes that can be read
     * from the buffer.
     * </summary>
     */
    public int ReadableBytes()
    {
        return writerIndex - readerIndex;
    }

    /*
     * <summary>
     * Returns the amount of bytes that can be wrote
     * to the buffer.
     * </summary>
     */
    public int WritableBytes()
    {
        return Capacity() - writerIndex;
    }

    /*
     * <summary>
     * Returns whether a read is possible on the 
     * buffer.
     * </summary>
     */
    public bool CanRead()
    {
        return ReadableBytes() > 0;
    }

    /*
     * <summary>
     * Returns whether a write is possible on the 
     * buffer.
     * </summary>
     */
    public bool CanWrite()
    {
        return WritableBytes() > 0;
    }

    public override string ToString()
    {
        return $"ByteBuf=[readerIndex: {readerIndex}, writerIndex:{writerIndex}, capacity: {Capacity()}, bytes: {String.Join(", ", bytes)}]";
    }
}
