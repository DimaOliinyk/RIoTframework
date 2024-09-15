using CourseWorkUI.Utilities;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace CourseWorkUI.UI;

public class TileGrid
{
    private double _width;
    private double _height;
    private List<Tile> _tiles = new List<Tile>();

    public TileGrid(double width, double height)
    {
        _width = width;
        _height = height;
    }

    public bool CanAddTile(Tile newTile) 
    {
        foreach (var tile in _tiles)
        {
            if (tile.Position == newTile.Position) 
            {
                return false;
            }
        }

        return true;
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

    public void Clear() =>_tiles.Clear();
}
