namespace Mint.Common.Error;

public enum Status
{
    FAILURE = -1,
    SUCCESS = 0x00,
    EXTRA_BYTES = 0x01,
    UNKNOWN_PACKET = 0x02,
    NON_MATCHING_PARAMETERS = 0x03,
    MISFORMATTED_FRAME = 0x04,
    MISFORMATTED_PACKET = 0x05,
}
