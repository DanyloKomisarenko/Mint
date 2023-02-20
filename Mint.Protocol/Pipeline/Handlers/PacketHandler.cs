using Mint.Common.Error;
using Mint.Protocol.Packet;

namespace Mint.Protocol.Pipeline.Handlers;

public class PacketHandler : ICurio<int, RealPacket>
{
    public int Poke(RealPacket input)
    {
        if (input.Template.id == 0)
        {
            Console.WriteLine(String.Join(", ", input.Parameters));
        }

        return (int) Status.SUCCESS;
    }
}
