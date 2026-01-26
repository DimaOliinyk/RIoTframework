namespace CourseWorkUI.Utilities;

public class Position
{
    public double X { get; set; }
    public double Y { get; set; }

    public Position Round(double tileSize) { 
        X = _RoundHelper(X, tileSize);
        Y = _RoundHelper(Y, tileSize);
        return this;
    }

    public Position(double x, double y)
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

    private double _RoundHelper(double val, double tileSize) => val - val % tileSize;

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
