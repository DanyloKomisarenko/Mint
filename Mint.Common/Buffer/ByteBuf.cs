using System.Net.Sockets;
using System.Text;

namespace Mint.Common.Buffer;

public class ByteBuf : AbstractByteBuf
{
    private const int SEGMENT_BITS = 0x7F;
    private const int CONTINUE_BIT = 0x80;
    private const int MAXIMUM_STRING_LENGTH = 32767;

    public ByteBuf(ByteBuf input)
    {
        this.bytes = new byte[input.Capacity()];
        input.Flip();
        for (int i = 0; i < input.Capacity(); i++) WriteByte(input.ReadByte());
    }

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
        return ReadByte() == 0x01;
    }

    /*
     * <summary>
     * Converts a boolean one byte
     * and writes it to the buffer.
     * </summary>
     */
    public void WriteBool(bool o)
    {
        WriteByte((byte) (o ? 0x01 : 0x00));
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

    public void WriteVarLong(long @long)
    {
        while (true)
        {
            if ((@long & ~SEGMENT_BITS) == 0)
            {
                WriteByte((byte)@long);
                return;
            }

            WriteByte((byte)(@long & SEGMENT_BITS | CONTINUE_BIT));

            @long >>= 7;
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

    public void WriteVarInt(int @int)
    {
        while (true)
        {
            if ((@int & ~SEGMENT_BITS) == 0)
            {
                WriteByte((byte)@int);
                return;
            }

            WriteByte((byte)(@int & SEGMENT_BITS | CONTINUE_BIT));

            @int >>= 7;
        }
    }

    public void WriteString(string @string)
    {
        WriteString(MAXIMUM_STRING_LENGTH, @string);
    }

    public string ReadString()
    {
        return ReadString(MAXIMUM_STRING_LENGTH);
    }

    public void WriteString(int max, string @string)
    {
        byte[] bytes = new UTF8Encoding().GetBytes(@string);
        if (bytes.Length > max)
        {
            throw new Exception($"String too big (Is: '{bytes.Length}', Max: '{max}')");
        }
        else
        {
            WriteVarInt(bytes.Length);
            WriteBytes(bytes);
        }
    }

    public string ReadString(int max)
    {
        int length = ReadVarInt();
        if (length > max * 4)
        {
            throw new InvalidOperationException($"The recieved encoded string is longer than maximum allowed ({length} > {max * 4})");
        }
        else if (length < 0)
        {
            throw new InvalidOperationException($"The recieved encoded string length is less than zero");
        }
        else
        {
            string s = new UTF8Encoding().GetString(ReadBytes(length), 0, length);
            if (s.Length > MAXIMUM_STRING_LENGTH)
            {
                throw new InvalidOperationException($"The received string length is longer than maximum allowed ({length} > {MAXIMUM_STRING_LENGTH})");
            }
            else
            {
                return s;
            }
        }
    }

    public byte[] ReadAllReadable()
    {
        return ReadBytes(ReadableBytes());
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
            throw new IndexOutOfRangeException($"Failed to read byte (readerIndex: {readerIndex}, writerIndex: {writerIndex}, capacity: {Capacity()})");
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
            throw new IndexOutOfRangeException($"Failed to write byte (readerIndex: {readerIndex}, writerIndex: {writerIndex}, capacity: {Capacity()})");
        }
    }

    public ByteBuf Copy()
    {
        return new ByteBuf(bytes);
    }
}
