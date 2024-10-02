using CourseWorkUI.UI.Tiles;
using CourseWorkUI.Utilities;
using System.ComponentModel;

namespace CourseWorkUI.UI.Menues;

/// <summary>
/// Class resposible for creation of Tiles
/// </summary>
public static class TileFactory
{
    /// <summary>
    /// Function resposible for tile creation.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="tileSize"></param>
    /// <param name="tileType"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException"></exception>
    public static Tile CreateTile(Position pos, float tileSize, TileTypes tileType) 
    {
        return tileType switch
        {
            TileTypes.BUTTON    => new Tiles.Button(pos, tileSize),
            TileTypes.GRAPH     => new Graph(pos, tileSize),
            TileTypes.LED       => new Led(pos, tileSize),
            TileTypes.PIECHART  => new CircularChart(pos, tileSize),
            _ => throw new InvalidEnumArgumentException("Wrong Tile type"),
        };
    }

    
}

/// <summary>
/// Types of Tiles
/// </summary>
public enum TileTypes 
{
    BUTTON = 0,
    GRAPH = 1,
    LED = 2,
    PIECHART = 3,
}

