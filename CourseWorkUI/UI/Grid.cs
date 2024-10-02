#if ANDROID
using Android.Service.QuickSettings;
using Android.Systems;
using Android.Views;
#endif
using CourseWorkUI.Utilities;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Diagnostics;

namespace CourseWorkUI.UI;

/// <summary>
/// Resposible for saving all Tiles  
/// and allow easy access to perform 
/// operations on them
/// </summary>
public class TileGrid
{
    // Available size for tiles 
    public double Width { get; private set; }
    public double Height { get; private set; }

    private List<Tile> _tiles = new List<Tile>();   // List of Tiles

    public TileGrid(double width, double height)
    {
        // TODO: check values
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Checks whether space is occupied by a Tile
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool SpaceIsOccupied(Position pos) 
    {
        foreach (var tile in _tiles)
        {
            if (CheckPosition(tile, pos))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Checks whether tile can be added
    /// </summary>
    /// <param name="newTile"></param>
    /// <returns></returns>
    public ETileGrid CanAddTile(Tile newTile) 
    {
        // checks whether position is outside the grid
        if (newTile.Position.X > Width ||    
            newTile.Position.Y > Height)
        {
            return ETileGrid.OUT_OF_BOUNDS;
        }

        // checks whether size of the element is small enough to fit
        if (newTile.Position.X + newTile.Width > Width ||
            newTile.Position.Y + newTile.Height > Height) 
        {
            return ETileGrid.ELEMENT_TO_WIDE;
        }
        
        //TODO: A bug is hidden somewhere here

        // Checks whether tile can be added
        foreach (var tile in _tiles)
        {
            if (tile.Position == newTile.Position)
            {
                return ETileGrid.FALSE;
            }
        }

        return ETileGrid.TRUE;
    }

    /// <summary>
    /// Gets Tile by specified position
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Tile? GetTile(Position pos)
    {
        foreach (var tile in _tiles)
        {
            if (CheckPosition(tile, pos))
            {
                return tile;
            }
        }
        return null;
    }

    /// <summary>
    /// Adds Tile
    /// </summary>
    /// <param name="newTile"></param>
    public void AddTile(Tile newTile) => _tiles.Add(newTile);

    /// <summary>
    /// Removes Tile
    /// </summary>
    /// <param name="newTile"></param>
    public void RemoveTile(Tile newTile)
    {
        foreach (var tile in _tiles)
        {
            // if specified Tile is found it gets removed
            if (tile.Position == newTile.Position)
            {
                _tiles.Remove(tile);
                break;
            }
        }
    }

    /// <summary>
    /// Calls DrawOnCanvas for each Tile
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="dirtyRect"></param>
    public void RedrawOnCanvas(ICanvas canvas, RectF dirtyRect) 
    {
        foreach (var tile in _tiles) 
        {
            tile.DrawOnCanvas(canvas, dirtyRect);
        }
    }

    /// <summary>
    /// Draws helper UI elements to 
    /// show where Tiles can be drawn
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="dirtyRect"></param>
    /// <param name="tileSize"></param>
    public void DrawPoints(ICanvas canvas, RectF dirtyRect, double tileSize)
    {
        canvas.FillColor = ColorDictionary.TextColor;

        for (double i = -1; i < Width; i += tileSize) 
        {
            for (double j = -1; j < Height; j += tileSize)
            {
                canvas.FillCircle((float)i, (float)j, 2);
            }
        }
    }

    /// <summary>
    /// Deletes all Tiles
    /// </summary>
    public void Clear() => _tiles.Clear();

    /// <summary>
    /// Helper function to check position
    /// </summary>
    /// <param name="tile"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    private bool CheckPosition(Tile tile, Position pos)
    {
        return tile.Position == pos ||
               tile.Position.X + tile.Width - 2 >= pos.X &&
               tile.Position.X <= pos.X &&
               tile.Position.Y == pos.Y;
    }
}

/// <summary>
/// Enum of error codes 
/// </summary>
public enum ETileGrid 
{
    FALSE = 0,
    TRUE = 1,
    OUT_OF_BOUNDS = -1,
    ELEMENT_TO_WIDE = -2,
}
