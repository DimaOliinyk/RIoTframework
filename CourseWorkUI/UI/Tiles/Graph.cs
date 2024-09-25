using CourseWorkUI.UI.Tiles.TProperties;
using CourseWorkUI.UI.Tiles.TPropertiesp;
using CourseWorkUI.Utilities;

namespace CourseWorkUI.UI.Tiles;

public class Graph : Tile
{
    private TProperty _name;

    public Graph(Position pos, double size, double tileSize) : base(pos, 2*size, size)
    {
        _name = new TPropertyName("Graph");

        Properties.Add(_name);
        Properties.Add(new TPropertyPin(""));
    }

    protected override void DrawElementOverridable(ICanvas canvas, RectF dirtyRect) 
    {
        DrawName(canvas, dirtyRect, _name.Value);
    }
}
