
public class SpawnVariant
{
    public bool Left { get; private set; }
    public bool Middle { get; private set; }
    public bool Right { get; private set; }

    public SpawnVariant(bool left, bool mid, bool right)
    {
        Left = left;
        Middle = mid;
        Right = right;
    }
}

