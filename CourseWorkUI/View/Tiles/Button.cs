using CourseWorkUI.UI.Tiles.TProperties;
using CourseWorkUI.UI.Tiles.TPropertiesp;
using CourseWorkUI.Utilities;
using Font = Microsoft.Maui.Graphics.Font;

namespace CourseWorkUI.UI.Tiles;

/// <summary>
/// Class representing UI element Button 
/// </summary>
class Button : Tile
{
    private TPropertyName _name;    // Name of the Tile to be displayed
    public bool ButtonIsOn { get; private set; } = false;    // Current state of the button 

    public Button(Position pos, float size) : base(pos, size, size)
    {
        _name = new TPropertyName("BTN");
        Properties.Add(_name);
        Properties.Add(new TPropertyPin(""));
    }

    protected override void DrawElementOverridable(ICanvas canvas, RectF dirtyRect)
    {
        float x = Position.X + Width / 2f;
        float y = Position.Y + Height / 2f;

        canvas.Font = new Font("Tomorrow-Regular.ttf");
        canvas.FontSize = Width / 5f;
        

        if (ButtonIsOn && AppState.IsRunning)
        {
            canvas.FillColor = Color.FromArgb("#bb86fc");
            canvas.FillCircle(x, y, Width / 2.4f);
            canvas.FontColor = Colors.Black;

            canvas.DrawString(
                "ON",
                x,
                y+10f,
                HorizontalAlignment.Center);
        }
        else
        {
            canvas.StrokeColor = Color.FromArgb("#bb86fc");
            canvas.StrokeSize = 5f;
            canvas.DrawCircle(x, y, Width / 2.4f);
            canvas.FontColor = Color.FromArgb("#bb86fc");

            canvas.DrawString(
                "OFF",
                x,
                y+10,
                HorizontalAlignment.Center);
        }

        DrawName(canvas, dirtyRect, _name.Value);
    }

    public override void Clicked() => ButtonIsOn = !ButtonIsOn;  // change current button state
    public override void Clicked(Position pos) => Clicked();
}
