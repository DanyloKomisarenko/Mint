namespace Mint.Protocol;

/*
* <summary>
* The game state when a packet should be 
* expected.
* </summary>
*/
public enum State {
    HANDSHAKE = 0,
    STATUS = 1,
    LOGIN = 2,
    PLAY = 3
}