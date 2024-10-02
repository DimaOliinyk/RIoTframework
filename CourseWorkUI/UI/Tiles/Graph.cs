using CourseWorkUI.UI.Tiles.TProperties;
using CourseWorkUI.UI.Tiles.TPropertiesp;
using CourseWorkUI.Utilities;
using Font = Microsoft.Maui.Graphics.Font;

namespace CourseWorkUI.UI.Tiles;

// TODO: check for argument to be 0.
// TODO: Add circular array to save values

/// <summary>
/// Graph in Cartesian coordinate system
/// </summary>
public class Graph : Tile
{
    private TProperty _name;
    private TPropertyValue _min;
    private TPropertyValue _max;
    private TPropertyValue _valueCount;

    private Random _random = new Random();

    public Graph(Position pos, float size) : base(pos, 2 * size, size)
    {
        _name = new TPropertyName("Graph");
        _min = new TPropertyValue("0", "Min");
        _max = new TPropertyValue("255", "Max");
        _valueCount = new TPropertyValue("8", "Count");

        Properties.Add(_name);
        Properties.Add(new TPropertyPin(""));
        Properties.Add(_min);
        Properties.Add(_max);
        Properties.Add(_valueCount);
    }

    protected override void DrawElementOverridable(ICanvas canvas, RectF dirtyRect)
    {
        float diff = Height / 3f;
        float startPos = Position.Y + 45f;

        canvas.Font = new Font("Tomorrow-Regular.ttf");
        canvas.FontSize = Height / 18f;
        canvas.FontColor = Colors.Grey;

        var min = _min.GetNumber();
        var max = _max.GetNumber();
        var constMin = min;

        float count = (float)((Width - 110f) / _valueCount.GetNumber());
        float xStart = Position.X + 50;

        for (; min <= max; min += (max - constMin) / 2)
        {
            canvas.DrawString(
                $"{(max - min):0.00}",
                xStart,
                startPos,
                HorizontalAlignment.Right);
            startPos += diff;
        }

        canvas.FillColor = Color.FromArgb("#bb86fc");
        PathF path = new PathF();

#if WINDOWS
        path.Move(xStart += 50f, startPos -= diff);
#else
        path.Move(xStart += 20f, startPos -= diff);
#endif
        path.LineTo(xStart, startPos);
        for (int i = 0; i < (int)_valueCount.GetNumber(); i++)
        {
            path.LineTo(xStart, startPos + (float)(_random.NextDouble() * (-2*diff)) - 5);
            xStart += count;
        }
        path.LineTo(xStart -= count, startPos);
        path.Close();
        canvas.FillPath(path);

        DrawName(canvas, dirtyRect, _name.Value);
    }
}
