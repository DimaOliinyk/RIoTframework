using CourseWorkUI.UI.Tiles.TProperties;
using CourseWorkUI.UI.Tiles.TPropertiesp;
using CourseWorkUI.Utilities;
using CourseWorkUI.View.Tiles.TProperties;
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
    private TPropertyState _isSimple;

    private Random _random = new Random();

    public Graph(Position pos, float size) : base(pos, 2 * size, size)
    {
        _name = new TPropertyName("Graph");
        _min = new TPropertyValue("0", "Min");
        _max = new TPropertyValue("255", "Max");
        _valueCount = new TPropertyValue("8", "Count");
        _isSimple = new TPropertyState("Simplify");

        Properties.Add(_name);
        Properties.Add(new TPropertyPin(""));
        Properties.Add(_min);
        Properties.Add(_max);
        Properties.Add(_valueCount);
        Properties.Add(_isSimple);
    }

    protected override void DrawElementOverridable(ICanvas canvas, RectF dirtyRect)
    {
        canvas.SaveState();     // Saving state is required to reset gradient fill or else bugs will appear

        float diff = Height / 3f;
        float startPos = Position.Y + 45f;

        canvas.Font = new Font("Tomorrow-Regular.ttf");
        canvas.FontSize = Height / 18f;
        canvas.FontColor = Colors.Grey;
        canvas.StrokeColor = Color.FromArgb("#bb86fc");
        canvas.StrokeSize = 2f;

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

        PathF path = new PathF();
#if WINDOWS
        path.Move(xStart += 50f, startPos -= diff);
#else
        path.Move(xStart += 20f, startPos -= diff);
#endif
        path.LineTo(xStart, startPos);
        var value = startPos + (float)(_random.NextDouble() * (-2 * diff)) - 5;
        float prevValue;

        for (int i = 0; i < (int)_valueCount.GetNumber(); i++)
        {
            prevValue = value;
            value = startPos + (float)(_random.NextDouble() * (-2 * diff)) - 5;

            if(_isSimple.Value == false)
                canvas.DrawLine(xStart, prevValue, xStart+count, value);
            path.LineTo(xStart, prevValue);
            xStart += count;            
        }
        path.LineTo(xStart, value);
        path.LineTo(xStart, startPos);
        path.Close();

        if (_isSimple.Value == false)
        {
            var gradient = new LinearGradientPaint
            {
                StartColor = ColorDictionary.TransparentPrimary,
                EndColor = ColorDictionary.TileBackground,  //Using Colors.Transparent causes to not display Tile frame
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1),
            };
            gradient.AddOffset(0.1f, ColorDictionary.TransparentPrimary);
            gradient.AddOffset(0.95f, Colors.Transparent);


            // TODO:FIXMELATER: SetFillPaint effects other ui if not reset
            canvas.SetFillPaint(gradient, new RectF(Position.X, Position.Y, Width, Height));
        }
        else 
        {
            canvas.FillColor = ColorDictionary.Primary;
        }
        canvas.FillPath(path);
        DrawName(canvas, dirtyRect, _name.Value);
        
        canvas.RestoreState();      // reseting state to clear gradient fill
    }
}
