using CourseWorkUI.UI.Tiles.TProperties;
using CourseWorkUI.UI.Tiles.TPropertiesp;
using CourseWorkUI.Utilities;
using Font = Microsoft.Maui.Graphics.Font;

namespace CourseWorkUI.UI.Tiles;

/// <summary>
/// Light emitting diod as Tile
/// </summary>
class Led : Tile
{
    private TProperty _name;

    public bool OnState { get; private set; } = false;

    public Led(Position pos, float size) : base(pos, size, size)
    {
        _name = new TPropertyName("LED");

        Properties.Add(_name);
        Properties.Add(new TPropertyPin(""));
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

    public void ChangeState() => OnState = !OnState;    // Change led state
}
