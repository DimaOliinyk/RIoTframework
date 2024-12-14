using CourseWorkUI.UI.Menues;
using CourseWorkUI.UI.Tiles.TProperties;
using CourseWorkUI.UI.Tiles.TPropertiesp;
using CourseWorkUI.Utilities;
using CourseWorkUI.View.Tiles.TProperties;
using System.Diagnostics;
using Font = Microsoft.Maui.Graphics.Font;

namespace CourseWorkUI.UI.Tiles;

/// <summary>
/// Gauge Tile
/// </summary>
public class Gauge : Tile, IOutput, IDBSaveable, IExtraVerifiable
{
    private TProperty _name;        // Name property
    private TPropertyPin _pin;
    private TPropertyValue<float> _min;    // Min value property
    private TPropertyValue<float> _max;    // Max value property
    private double _value;          // Value that will be passed to Chart
    private TPropertyLogical _logical;
    private TPropertyState _monitoredToDB;

    public Gauge(Position pos) : base(pos, Tile.Size, Tile.Size)
    {
        Properties.Add(_name = new TPropertyName("Pie"));
        Properties.Add(_pin = new TPropertyPin($"{TileFactory.GetAvailablePin()}"));
        Properties.Add(_min = new TPropertyValue<float>("0", "Min"));
        Properties.Add(_max = new TPropertyValue<float>("255", "Max"));
        Properties.Add(_logical = new TPropertyLogical());
        Properties.Add(_monitoredToDB = new TPropertyState("Save to DB"));

        Random rand = new Random();
        _value = rand.NextDouble()*(_max.GetNumber() - _min.GetNumber()) + _min.GetNumber();
    }

    protected override void DrawElementOverridable(ICanvas canvas, RectF dirtyRect)
    {
        float r = Width / 1.4f;
        float x = Position.X + r / 5f - 5f;
        float y = Position.Y + r / 5f + 10f;

        canvas.StrokeSize = 20f;
        canvas.FontColor = Color.FromArgb("#bb86fc");

        canvas.Font = new Font("Tomorrow-Regular.ttf");
        canvas.FontSize = Width / 7f;

        canvas.StrokeColor = Color.FromArgb("#abbb86fc");
        canvas.DrawArc(
            x, y,
            r, r,
            -400, 220,
            false, false);

        canvas.StrokeColor = Color.FromArgb("#bb86fc");
        canvas.DrawArc(
            x, y,
            r, r,
            -400f + 180f * (1f - (float)_value / (float)(_max.GetNumber() - _min.GetNumber())), 220,
            false, false);

        canvas.DrawString(
                $"{_value:0.00}",
                Position.X + Width / 2f - 2f,
                Position.Y + Width / 2f + 10f,
                HorizontalAlignment.Center);

        DrawName(canvas, dirtyRect, _name.Value);
    }

    public override int GetPin() => _pin.GetNumber();

    public void SetValue(int value)
    {
        _value = CircuitInterpreter.ConvertIntToFloat(value, _min.GetNumber() + 50f, _max.GetNumber() - 50f);
        if (_logical.ConditionIsTrue(value))
        {
            NotificationSender.Notify($"Value of pin {_pin.GetNumber()} is {_value}");
        }
    }

    public bool SaveToDB => _monitoredToDB.Value;

    public (bool, string) ExtraVerify() =>
        (_min.GetNumber() < _max.GetNumber(), "Min value must be less than Max value");
}
