namespace Mint.Common.Buffer;

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
