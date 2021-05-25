public class PlayerInfo
{
    public PlayerType Type { get; }
    public int UIIndex { get; }
    public int InputIndex { get; }
    public TDTransform ParentTransform { get; }

    public Player Instance { get; set; }
    public PlayerDisplay Display { get; set; }

    public PlayerInfo(PlayerType type, int inputIndex, int uIIndex, TDTransform transform)
    {
        Type = type;
        UIIndex = uIIndex;
        InputIndex = inputIndex;
        ParentTransform = transform;
    }
}