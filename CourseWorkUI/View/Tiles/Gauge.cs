using CourseWorkUI.UI.Tiles.TProperties;
using CourseWorkUI.UI.Tiles.TPropertiesp;
using CourseWorkUI.Utilities;
using System.Diagnostics;
using Font = Microsoft.Maui.Graphics.Font;

namespace CourseWorkUI.UI.Tiles;

// TODO: check for argument to be 0.

/// <summary>
/// Gauge Tile
/// </summary>
public class Gauge : Tile
{
    private TProperty _name;        // Name property
    private TPropertyValue _min;    // Min value property
    private TPropertyValue _max;    // Max value property
    private double _value;          // Value that will be passed to Chart

    public Gauge(Position pos, float size) : base(pos, size, size)
    {
        _name = new TPropertyName("Pie");
        _min = new TPropertyValue("0", "Min");
        _max = new TPropertyValue("255", "Max");
        
        Properties.Add(_name);
        Properties.Add(new TPropertyPin(""));
        Properties.Add(_min);
        Properties.Add(_max);

        // for debugging
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
            -400 + 180 * (float)((1 - _value / _max.GetNumber())), 220,
            false, false);

        canvas.DrawString(
                $"{_value:0.00}",
                Position.X + Width / 2f - 2f,
                Position.Y + Width / 2f + 10f,
                HorizontalAlignment.Center);

        DrawName(canvas, dirtyRect, _name.Value);
    }
}
