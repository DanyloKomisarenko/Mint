using Mint.Common.Error;

namespace Mint.Common.Error;

public class MintException : System.Exception
{
    public MintException(string msg, System.Exception parent, Status status) : base($"{msg} [Status: '{status}']", parent)
    {

    }
}
