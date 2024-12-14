using CourseWorkUI.UI.Menues;
using CourseWorkUI.UI.Tiles.TProperties;
using CourseWorkUI.UI.Tiles.TPropertiesp;
using CourseWorkUI.Utilities;
using CourseWorkUI.View.Tiles.TProperties;
using Font = Microsoft.Maui.Graphics.Font;

namespace CourseWorkUI.UI.Tiles;

/// <summary>
/// Graph in Cartesian coordinate system
/// </summary>
public class Graph : Tile, IOutput, IDBSaveable, IExtraVerifiable
{
    private TProperty _name;
    private TPropertyPin _pin;
    private TPropertyValue<float> _min;
    private TPropertyValue<float> _max;
    private TPropertyValue<int> _valueCount;
    private TPropertyState _isSimple;
    private TPropertyLogical _logical;
    private TPropertyState _monitoredToDB;
    private CircularArray<float> _array;

    private Random _random = new Random();

    public Graph(Position pos) : base(pos, 2 * Tile.Size, Tile.Size)
    {
        Properties.Add(_name = new TPropertyName("Graph"));
        Properties.Add(_pin = new TPropertyPin($"{TileFactory.GetAvailablePin()}"));
        Properties.Add(_min = new TPropertyValue<float>("0", "Min"));
        Properties.Add(_max = new TPropertyValue<float>("255", "Max"));
        Properties.Add(_valueCount = new TPropertyValue<int>("8", "Count"));
        Properties.Add(_isSimple = new TPropertyState("Simplify"));
        Properties.Add(_logical = new TPropertyLogical());
        Properties.Add(_monitoredToDB = new TPropertyState("Save to DB"));
        
        _array = new CircularArray<float>(_valueCount.GetNumber());

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

        // Draw scale
        for (startPos += diff * (max - constMin != 0 ? 2 : 1);
             min <= max; 
             min += (max - constMin) / 2)     
        {
            canvas.DrawString(
                $"{min:0.00}",
                xStart,
                startPos,
                HorizontalAlignment.Right);
            startPos -= diff;

            if (max - constMin == 0)
                break; 
        }
        startPos += 4 * diff;

        PathF path = new PathF();
#if WINDOWS
        path.Move(xStart += 50f, startPos -= diff);
#else
        path.Move(xStart += 20f, startPos -= diff);
#endif
        path.LineTo(xStart, startPos);
        float value = startPos + _array[0] / (max - constMin) * (-2f * diff) - 5f;
        float prevValue;

        foreach (float item in _array)
        {
            prevValue = value;
            value = startPos + (float)(item / (max - constMin) * (-2 * diff)) - 5;

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
                EndColor = ColorDictionary.TileBackground,
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1),
            };
            gradient.AddOffset(0.1f, ColorDictionary.TransparentPrimary);
            gradient.AddOffset(0.95f, Colors.Transparent);


            // TODO:FIXMELATER: SetFillPaint effects other UI elements if not reset
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
    public override int GetPin() => _pin.GetNumber();

    public void SetValue(int value) 
    {
        _array.AddValue(
            CircuitInterpreter.ConvertIntToFloat(
                value, 
                _min.GetNumber(), 
                _max.GetNumber()));
        if (_logical.ConditionIsTrue(value))
        {
            NotificationSender.Notify($"Value of pin {GetPin()} is {value}");
        }
    }

    public bool SaveToDB => _monitoredToDB.Value;

    public (bool, string) ExtraVerify() =>
        (_min.GetNumber() < _max.GetNumber(), "Min value must be less than Max value");
}
