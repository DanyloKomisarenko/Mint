namespace Mint.Common.Buffer;

/// <summary>
/// This class allows easier access and manipulation to bytes.
/// </summary>
/// This class partitions data into three sections:
/// Readable Bytes:
/// This section spans between the reader index and the writer 
/// index. It contains the bytes that can be read. When initialized 
/// it is usually zero.
/// Writable Bytes:
/// This section spans between the writer index and the capacity. It 
/// contains the space that can be written to. When initialized is 
/// also zero.
/// Discardable Bytes:
/// This section spans between the zero index and the reader index. It 
/// contains the read bytes that are now unreachable unless a flip operation
/// is executed.
/// Methods:
/// Clear:
/// Resets the reader and writer index to prepare for more write operations.
/// Flip:
/// Resets the reader and index and sets the writer index to max. This allows
/// the reading of the entire buffer even the unwritten bytes.
public interface IByteBuf
{
    public int ReaderIndex();
    public int WriterIndex();
    public int Capacity();

    /*
     * <summary>
     * Writes a single byte into the buffer and increases 
     * the Writer Index by one.
     * </summary>
     */
    public void WriteByte(byte b);

    /*
     * <summary>
     * Reads a single bytes from the buffer and increases 
     * the Reader Index by one.
     * </summary>
     */
    public byte ReadByte();

    /*
     * <summary>
     * Does not clear the contents of the buffer but
     * resets both pointers to zero.
     * </summary>
     */
    public void Clear();

    /*
     * <summary>
     * 
     * </summary>
     */
    public void Flip();
}
