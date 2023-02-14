namespace Mint.Common.Buffer;

public class ByteBuf : AbstractByteBuf
{
    private const int SEGMENT_BITS = 0x7F;
    private const int CONTINUE_BIT = 0x80;

    public ByteBuf() : this(0) { }
    public ByteBuf(int capacity) : this(new byte[capacity]) { }
    public ByteBuf(byte[] bytes) : this(bytes, 0, 0) { }

    ByteBuf(byte[] bytes, int readerIndex, int writerIndex)
    {
        this.bytes = new byte[bytes.Length];
        bytes.CopyTo(this.bytes, 0);
        this.readerIndex = readerIndex;
        this.writerIndex = writerIndex;
    }

    /*
     * <summary>
     * Reads eight bytes and converts them into a
     * 64-bit integer.
     * </summary>
     */
    public long ReadLong()
    {
        return BitConverter.ToInt64(ReadBytes(8));
    }

    /*
     * <summary>
     * Converts a 64-bit integer into eight bytes
     * and writes them to the buffer.
     * </summary>
     */
    public void WriteLong(long o)
    {
        WriteBytes(BitConverter.GetBytes(o));
    }

    /*
     * <summary>
     * Reads four bytes and converts them into a
     * 32-bit integer.
     * </summary>
     */
    public int ReadInt()
    {
        return BitConverter.ToInt32(ReadBytes(4));
    }

    /*
     * <summary>
     * Converts a 32-bit integer into four bytes
     * and writes them to the buffer.
     * </summary>
     */
    public void WriteInt(int o)
    {
        WriteBytes(BitConverter.GetBytes(o));
    }

    /*
     * <summary>
     * Reads one byte and converts it into a
     * boolean.
     * </summary>
     */
    public bool ReadBool()
    {
        return ReadBool() == 0x01;
    }

    /*
     * <summary>
     * Converts a boolean one byte
     * and writes it to the buffer.
     * </summary>
     */
    public void WriteBool(bool o)
    {
        WriteBool(o ? 0x01 : 0x00);
    }

    /*
     * <summary>
     * Reads two bytes and converts them into a
     * unsigned short.
     * </summary>
     */
    public ushort ReadUShort()
    {
        return BitConverter.ToUInt16(ReadBytes(2));
    }

    /*
     * <summary>
     * Converts a unsigned short into two bytes
     * and writes them to the buffer.
     * </summary>
     */
    public void WriteUShort(ushort o)
    {
        WriteBytes(BitConverter.GetBytes(o));
    }

    public long ReadVarLong()
    {
        long value = 0;
        int position = 0;
        byte currentByte;

        while (true)
        {
            currentByte = ReadByte();
            value |= (long)(currentByte & SEGMENT_BITS) << position;

            if ((currentByte & CONTINUE_BIT) == 0) break;

            position += 7;

            if (position >= 64) throw new Exception("VarLong is too big");
        }

        return value;
    }

    public void WriteVarLong(long _long)
    {
        while (true)
        {
            if ((_long & ~SEGMENT_BITS) == 0)
            {
                WriteByte((byte)_long);
                return;
            }

            WriteByte((byte)(_long & SEGMENT_BITS | CONTINUE_BIT));

            _long >>= 7;
        }
    }

    public int ReadVarInt()
    {
        int value = 0;
        int position = 0;
        byte currentByte;

        while (true)
        {
            currentByte = ReadByte();
            value |= (currentByte & SEGMENT_BITS) << position;

            if ((currentByte & CONTINUE_BIT) == 0) break;

            position += 7;

            if (position >= 32) throw new Exception("VarInt is too big");
        }

        return value;
    }

    public void WriteVarInt(int _int)
    {
        while (true)
        {
            if ((_int & ~SEGMENT_BITS) == 0)
            {
                WriteByte((byte)_int);
                return;
            }

            WriteByte((byte)(_int & SEGMENT_BITS | CONTINUE_BIT));

            _int >>= 7;
        }
    }

    public override byte ReadByte()
    {
        if (CanRead())
        {
            byte b = bytes[readerIndex];
            readerIndex++;
            return b;
        }
        else
        {
            throw new Exception($"Failed to read byte (readerIndex: {readerIndex}, writerIndex: {writerIndex}, capacity: {Capacity()})");
        }
    }

    public override void WriteByte(byte b)
    {
        if (CanWrite())
        {
            bytes[writerIndex] = b;
            writerIndex++;
        }
        else
        {
            throw new Exception($"Failed to write byte (readerIndex: {readerIndex}, writerIndex: {writerIndex}, capacity: {Capacity()})");
        }
    }

    public static ByteBuf WrapPacketBuffer(int size, byte[] bytes)
    {
        ByteBuf buf = new(size);
        for (int i = size - 1; i >= 0; i--)
        {
            buf.WriteByte(bytes[i]);
        }
        return buf;
    }
}
