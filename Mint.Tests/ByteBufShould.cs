using Mint.Common.Buffer;

namespace Mint.Tests;

[TestClass]
public class ByteBufShould
{
    [TestMethod]
    public void WriteReadByte()
    {
        ByteBuf buf = new(12);
        buf.WriteByte(0x12);

        Assert.AreEqual(0x12, buf.ReadByte());
    }

    [TestMethod]
    public void WriteReadLong()
    {
        ByteBuf buf = new(8);
        buf.WriteLong(12);

        Assert.AreEqual(12, buf.ReadLong());
    }

    [TestMethod]
    public void WriteReadInt()
    {
        ByteBuf buf = new(4);
        buf.WriteInt(33);

        Assert.AreEqual(33, buf.ReadInt());
    }

    [TestMethod]
    public void WriteReadBool()
    {
        ByteBuf buf = new(8);
        buf.WriteBool(true);
        buf.WriteBool(false);

        Assert.AreEqual(true, buf.ReadBool());
        Assert.AreEqual(false, buf.ReadBool());
    }

    [TestMethod]
    public void WriteReadUShort()
    {
        ByteBuf buf = new(2);
        buf.WriteUShort(6);

        Assert.AreEqual(6, buf.ReadUShort());
    }

    [TestMethod]
    public void WriteReadVarLong()
    {
        ByteBuf buf = new(1);
        buf.WriteVarLong(9);

        Assert.AreEqual(9, buf.ReadVarLong());
    }

    [TestMethod]
    public void WriteReadVarInt()
    {
        ByteBuf buf = new(1);
        buf.WriteVarInt(9);

        Assert.AreEqual(9, buf.ReadVarInt());
    }
}
