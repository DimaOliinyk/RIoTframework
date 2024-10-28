using CourseWorkUI.UI.Tiles;
using CourseWorkUI.Utilities;
using CourseWorkUI.View.Tiles;
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
    /// <param name="tileType"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException"></exception>
    public static Tile CreateTile(Position pos, TileTypes tileType) 
    {
        return tileType switch
        {
            TileTypes.BUTTON    => new Tiles.Button(pos),
            TileTypes.GRAPH     => new Graph(pos),
            TileTypes.LED       => new Led(pos),
            TileTypes.GAUGE     => new Gauge(pos),
            TileTypes.POT     => new Pot(pos),
            _ => throw new InvalidEnumArgumentException("Wrong Tile type"),
        };
    }
    

    /// <summary>
    /// Function resposible for tile creation.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="tileType"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException"></exception>
    public static Tile CreateTile(Position pos, string tileType)
    {
        return tileType switch
        {
            nameof(TileTypes.BUTTON) => new Tiles.Button(pos),
            nameof(TileTypes.GRAPH)  => new Graph(pos),
            nameof(TileTypes.LED)    => new Led(pos),
            nameof(TileTypes.GAUGE)  => new Gauge(pos),
            nameof(TileTypes.POT)    => new Pot(pos),
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
    GRAPH,
    LED,
    GAUGE,
    POT,
}

