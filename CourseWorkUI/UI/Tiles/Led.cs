using CourseWorkUI.UI.Menues;
using CourseWorkUI.UI.Tiles.TProperties;
using CourseWorkUI.UI.Tiles.TPropertiesp;
using CourseWorkUI.Utilities;
using Font = Microsoft.Maui.Graphics.Font;

namespace CourseWorkUI.UI.Tiles;

/// <summary>
/// Light emitting diod as Tile
/// </summary>
class Led : Tile, IOutput
{
    private TProperty _name;
    private TPropertyPin _pin;
    public bool OnState { get; private set; } = false;

    public Led(Position pos) : base(pos, Tile.Size, Tile.Size)
    {
        _name = new TPropertyName("LED");
        _pin = new TPropertyPin($"{TileFactory.GetAvailablePin()}");

        Properties.Add(_name);
        Properties.Add(_pin);
    }

    protected override void DrawElementOverridable(ICanvas canvas, RectF dirtyRect)
    {
        var x = Position.X + Width / 2f;
        var y = Position.Y + Height / 2f;
        var r = Width / 2.4f;

        canvas.FillColor = Color.FromArgb("#bb86fc");
        canvas.StrokeColor = Color.FromArgb("#bb86fc");
        canvas.StrokeSize = 5f;

        if (OnState)
        {
            canvas.FillCircle(x, y, r);
            canvas.StrokeColor = Colors.White;
            canvas.StrokeSize = 10f;
            canvas.DrawArc(
                Position.X + r/2f,
                Position.Y + r/2f,
                r, r,
                90, 180,
                false, false);

        }
        else
        {
            canvas.DrawCircle(x, y, r);
            canvas.StrokeColor = Colors.Grey;
            canvas.StrokeSize = 10;
            canvas.DrawArc(
                Position.X + r / 2f,
                Position.Y + r / 2f,
                r, r,
                90, 180,
                false, false);
        }

        DrawName(canvas, dirtyRect, _name.Value);
    }

    public override int GetPin() => _pin.GetNumber();

    public void SetValue(int value) 
    {
        OnState = (value == 0) ? false : true;
    }
}
