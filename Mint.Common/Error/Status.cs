namespace Mint.Common.Error;

public enum Status
{
    FAILURE = -1,
    SUCCESS = 0x00,
    EXTRA_BYTES = 0x01,
    UNKNOWN_PACKET = 0x02,
    NOT_MATCHING_PARAMETERS = 0x03,
}
