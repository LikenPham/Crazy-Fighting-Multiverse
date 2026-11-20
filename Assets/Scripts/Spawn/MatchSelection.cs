public static class MatchSelection
{
    public const int None = -1;

    public static int Player1 = None;
    public static int Player2 = None;

    public static bool Ready => Player1 != None && Player2 != None;

    public static int Get(int playerNumber) => playerNumber == 1 ? Player1 : Player2;

    public static void Set(int playerNumber, int index)
    {
        if (playerNumber == 1) Player1 = index;
        else Player2 = index;
    }

    public static void Reset() { Player1 = None; Player2 = None; }
}