using CourseWorkUI.UI.Tiles.TProperties;
using CourseWorkUI.Utilities;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Graphics.Text;
using Font = Microsoft.Maui.Graphics.Font;

namespace CourseWorkUI.UI;

public class Tile
{
    //TODO:Remake list of properties into Dictionary
    public List<TProperty> Properties;      // List of tile Properties
    public double Width { get; set; }       // Width of the UI Element
    public double Height { get; set; }      // Height of the UI Element
    public Position Position { get; set; }  // Position on the canvas

    public Tile(Position pos, double width, double height)
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
        canvas.FillColor = Color.FromArgb("#202226");
        canvas.StrokeSize = 1;
        canvas.FillRoundedRectangle(
            (float)Position.X,
            (float)Position.Y,
            (float)Width - 2,
            (float)Height- 2, 
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
    /// <param name="name"></param>
    protected void DrawName(ICanvas canvas, RectF dirtyRect, string name) 
    {
        // TODO:FIXMELATER: Apparently doesn't set the 
        // font type on windows and no solutions online 
        // were provided... oh well I tried...
        canvas.Font = new Font("Tomorrow-Regular.ttf");
        canvas.FontSize = (float)(Width / 24);
        canvas.FontColor = Colors.Grey;

        canvas.DrawString(
            name,
            (float)Position.X + 7,
            (float)Position.Y + 20,
            HorizontalAlignment.Left);
    }
}
