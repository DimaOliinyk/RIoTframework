using CourseWorkUI.Utilities;

namespace CourseWorkUI.UI;

public class Tile
{
    public double Width { get; set; }
    public double Height { get; set; }
    public Position Position { get; set; }

    public Tile(Position pos, double width, double height)
    {
        Position = pos;
        Width = width;
        Height = height;
    }
}
