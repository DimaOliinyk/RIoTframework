#if ANDROID
using Android.Service.QuickSettings;
using Android.Systems;
using Android.Views;
#endif
using CourseWorkUI.Utilities;

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

    public List<Tile> Tiles { get; private set; }  = new List<Tile>();   // List of Tiles

    public TileGrid(double width, double height)
    {
        // TODO: check values
        Width = width;
        Height = height;
    }

    public static TileGrid Create() 
    {
        return
#if WINDOWS
            new TileGrid(App.WindowWidth - 50, //360
                 Math.Round(
                     App.WindowHeight /
                     (App.WindowWidth)) *
                     App.WindowWidth - 60f);
#else
            new TileGrid(DeviceDisplay.Current.MainDisplayInfo.Width / 3, //360
                 Math.Round(
                    DeviceDisplay.Current.MainDisplayInfo.Height /
                    (DeviceDisplay.Current.MainDisplayInfo.Width)) *
                    DeviceDisplay.Current.MainDisplayInfo.Width / 3 - 60);   //780
#endif
    }

    /// <summary>
    /// Checks whether space is occupied by a Tile
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool SpaceIsOccupied(Position pos) 
    {
        foreach (var tile in Tiles)
        {
            if (IsOccupied(tile, pos))
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

        // Checks whether tile can be added
        foreach (var tile in Tiles)
        {
            if (tile.Position == newTile.Position)
            {
                return ETileGrid.FALSE;
            }
            
            // Idk how but this works
            if (newTile.Position.X + newTile.Width > tile.Position.X &&
                newTile.Position.X < tile.Position.X &&
                newTile.Position.Y == tile.Position.Y)
            {
                return ETileGrid.ELEMENT_TO_WIDE;
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
        foreach (var tile in Tiles)
        {
            if (IsOccupied(tile, pos))
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
    public void AddTile(Tile newTile) => Tiles.Add(newTile);

    /// <summary>
    /// Removes Tile
    /// </summary>
    /// <param name="newTile"></param>
    public void RemoveTile(Tile newTile)
    {
        foreach (var tile in Tiles)
        {
            // if specified Tile is found it gets removed
            if (tile.Position == newTile.Position)
            {
                Tiles.Remove(tile);
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
        foreach (var tile in Tiles) 
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

    public override string ToString() 
    {
        string res = "";
        foreach(var tile in Tiles) 
        {
            res += 
                $"@{tile.Position.X / tile.Height};{tile.Position.Y / tile.Height},{tile.GetType().ToString().Substring(22).ToUpperInvariant()},";
            foreach (var prop in tile.Properties) 
            {
                res += prop.Value.ToString()+",";
            }
        }
        return res;
    }

    /// <summary>
    /// Deletes all Tiles
    /// </summary>
    public void Clear() => Tiles.Clear();

    /// <summary>
    /// Helper function to check whether specified position or position around is occupied
    /// </summary>
    /// <param name="tile"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    private bool IsOccupied(Tile tile, Position pos)
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
