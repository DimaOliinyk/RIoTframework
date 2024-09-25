#if ANDROID
using Android.Views;
#endif
using CourseWorkUI.Utilities;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Diagnostics;

namespace CourseWorkUI.UI;

public class TileGrid
{
    private double _width;
    private double _height;

    public double Width { get => _width; }
    public double Height { get => _height; }

    private List<Tile> _tiles = new List<Tile>();

    public TileGrid(double width, double height)
    {
        _width = width;
        _height = height;
    }

    public bool SpaceIsOccupied(Position pos) 
    {
        foreach (var tile in _tiles)
        {
            if (tile.Position == pos || 
                tile.Position.X + tile.Width - 2 == pos.X)
            {
                return true;
            }
        }
        return false;
    }

    public ETileGrid CanAddTile(Tile newTile) 
    {
        if (newTile.Position.X > _width ||
            newTile.Position.Y > _height)
        {
            return ETileGrid.OUT_OF_BOUNDS;
        }
        //Debug.Write($"({newTile.Position.X};{newTile.Position.Y}) in range ({_width};{_height})");

        if (newTile.Position.X + newTile.Width > _width ||
            newTile.Position.Y + newTile.Height > _height) 
        {
            return ETileGrid.ELEMENT_TO_WIDE;
        }

        foreach (var tile in _tiles)
        {
            if (tile.Position == newTile.Position)
            {
                return ETileGrid.FALSE;
            }
        }

        return ETileGrid.TRUE;
    }

    public Tile GetTile(Position pos)
    {
        foreach (var tile in _tiles)
        {
            if (tile.Position == pos)
            {
                return tile;
            }
        }
        return null!;
    }

    public void AddTile(Tile newTile) => _tiles.Add(newTile);

    public void RemoveTile(Tile newTile)
    {
        foreach (var tile in _tiles)
        {
            if (tile.Position == newTile.Position)
            {
                _tiles.Remove(tile);
                break;
            }
        }
    }

    public void RedrawOnCanvas(ICanvas canvas, RectF dirtyRect) 
    {
        foreach (var tile in _tiles) 
        {
            tile.DrawOnCanvas(canvas, dirtyRect);
        }
    }

    public void DrawPoints(ICanvas canvas, RectF dirtyRect, double tileSize)
    {
        canvas.FillColor = Colors.Grey;

        for (double i = -1; i < _width; i += tileSize) 
        {
            for (double j = -1; j < _height; j += tileSize)
            {
                canvas.FillCircle((float)i, (float)j, 2);
            }
        }
    }

    public void Clear() => _tiles.Clear();
}

public enum ETileGrid 
{
    FALSE = 0,
    TRUE = 1,
    OUT_OF_BOUNDS = -1,
    ELEMENT_TO_WIDE = -2,
}
