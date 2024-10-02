namespace CourseWorkUI.Utilities;

public class Position
{
    // X and Y positions
    public float X { get; set; }
    public float Y { get; set; }

    /// <summary>
    /// Rounds the position 
    /// </summary>
    /// <param name="tileSize"></param>
    /// <returns></returns>
    public Position Round(float tileSize) 
    {
        X = _RoundHelper(X, tileSize);
        Y = _RoundHelper(Y, tileSize);
        return this;
    }

    public Position(float x, float y)
    {
        X = x;
        Y = y;
    }

    public static bool operator ==(Position a, Position b)
    {
        return (a.X == b.X && a.Y == b.Y);
    }

    public static bool operator !=(Position a, Position b)
    {
        return !(a == b);
    }

    /// <summary>
    /// Helper rounding functions
    /// </summary>
    /// <param name="val"></param>
    /// <param name="tileSize"></param>
    /// <returns></returns>
    private float _RoundHelper(float val, float tileSize) => val - val % tileSize;

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (ReferenceEquals(obj, null))
        {
            return false;
        }

        if (!(obj is Position))
        {
            return false;
        }
        else
        {
            return (this == (Position)obj);
        }
    }
}
