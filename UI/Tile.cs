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
    
    public void DrawOnCanvas(ICanvas canvas, RectF dirtyRect) 
    {
        canvas.FillColor = Colors.DarkBlue;
        canvas.StrokeSize = 1;
        canvas.FillRoundedRectangle(
            (float)Position.X,
            (float)Position.Y,
            (float)Width - 2,
            (float)Height- 2, 
            10);
    }
}
