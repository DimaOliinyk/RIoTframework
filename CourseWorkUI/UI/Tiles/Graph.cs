using CourseWorkUI.UI.Menues;
using CourseWorkUI.UI.Tiles.TProperties;
using CourseWorkUI.UI.Tiles.TPropertiesp;
using CourseWorkUI.Utilities;
using CourseWorkUI.View.Tiles.TProperties;
using Font = Microsoft.Maui.Graphics.Font;

namespace CourseWorkUI.UI.Tiles;

// TODO: check for argument to be 0.

/// <summary>
/// Graph in Cartesian coordinate system
/// </summary>
public class Graph : Tile, IOutput
{
    private TProperty _name;
    private TPropertyPin _pin;
    private TPropertyValue<float> _min;
    private TPropertyValue<float> _max;
    private TPropertyValue<int> _valueCount;
    private TPropertyState _isSimple;
    private CircularArray<float> _array;

    private Random _random = new Random();

    public Graph(Position pos) : base(pos, 2 * Tile.Size, Tile.Size)
    {
        _name = new TPropertyName("Graph");
        _pin = new TPropertyPin($"{TileFactory.GetAvailablePin()}");
        _min = new TPropertyValue<float>("0", "Min");
        _max = new TPropertyValue<float>("255", "Max");
        _valueCount = new TPropertyValue<int>("8", "Count");
        _isSimple = new TPropertyState("Simplify");
        _array = new CircularArray<float>(_valueCount.GetNumber());

        Properties.Add(_name);
        Properties.Add(_pin);
        Properties.Add(_min);
        Properties.Add(_max);
        Properties.Add(_valueCount);
        Properties.Add(_isSimple);

        for(int i = 0; i < _array.Count; i++) 
        {
            _array[i] = (float)(_random.NextDouble() * (_max.GetNumber() - _min.GetNumber()) + _min.GetNumber());
        }
    }

    protected override void DrawElementOverridable(ICanvas canvas, RectF dirtyRect)
    {
        // Reset array if the count of elements has changed
        if (_array.Count != _valueCount.GetNumber()) 
        {
            _array = new CircularArray<float>(_valueCount.GetNumber());
        }

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

        float count = (Width - 110f) / _valueCount.GetNumber();
        float xStart = Position.X + 50;

        for (; min <= max; min += (max - constMin) / 2)     // Number scale
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
        float value = startPos + _array[0] / _max.GetNumber() * (-2f * diff) - 5f;
        float prevValue;

        foreach (float item in _array)
        {
            prevValue = value;
            value = startPos + (float)(item / _max.GetNumber() * (-2 * diff)) - 5;

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
                                    // TODO: When implementing comunication get rid of this
        //_array.AddValue((float)(_random.NextDouble() * (_max.GetNumber() - _min.GetNumber()) + _min.GetNumber()));
    }
    public void SetValue(int value) => _array.AddValue(value);

    public override int GetPin() => _pin.GetNumber();
}
