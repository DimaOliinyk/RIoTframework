using CourseWorkUI.UI.Tiles.TProperties;
using CourseWorkUI.Utilities;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Graphics.Text;
using Font = Microsoft.Maui.Graphics.Font;  // alias for Microsoft.Maui.Graphics.Font

namespace CourseWorkUI.UI;

/// <summary>
/// Represents a UI element dynamically added by user
/// </summary>
public abstract class Tile
{
    public List<TProperty> Properties;      // List of tile Properties
    public float Width { get; set; }       // Width of the UI Element
    public float Height { get; set; }      // Height of the UI Element
    public Position Position { get; set; }  // Position on the canvas

    public Tile(Position pos, float width, float height)
    {
        Position = pos;
        Width = width;
        Height = height;
        Properties = new List<TProperty>();
    }
    
    /// <summary>
    /// Draws Element onto the passed canvas
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="dirtyRect"></param>
    public void DrawOnCanvas(ICanvas canvas, RectF dirtyRect) 
    {
        canvas.FillColor = ColorDictionary.TileBackground;
        canvas.StrokeSize = 1;
        canvas.FillRoundedRectangle(
            Position.X,
            Position.Y,
            Width - 2f,
            Height- 2f, 
            10);

        DrawElementOverridable(canvas, dirtyRect);
    }

    /// <summary>
    /// Function that gives client an ability 
    /// to write drawing algorithm 
    /// </summary>
    protected virtual void DrawElementOverridable(ICanvas canvas, RectF dirtyRect) 
    {    
    }

    /// <summary>
    /// Function which will draw the name 
    /// of the element on top of it
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="dirtyRect"></param>
    /// <param name="name"> Name of the Tile </param>
    protected void DrawName(ICanvas canvas, RectF dirtyRect, string name) 
    {
        // TODO:FIXMELATER: Apparently doesn't set the 
        // font type on windows and no solutions online 
        // were provided... oh well I tried...
        canvas.Font = new Font("Tomorrow-Regular.ttf");
        canvas.FontSize = Height / 12f;
        canvas.FontColor = ColorDictionary.TextColor;

        canvas.DrawString(
            name,
            Position.X + 7f,
            Position.Y + 20f,
            HorizontalAlignment.Left);
    }

    /// <summary>
    /// Gets called when the Tile is 
    /// determind to have been clicked
    /// </summary>
    public virtual void Clicked() { }
}
