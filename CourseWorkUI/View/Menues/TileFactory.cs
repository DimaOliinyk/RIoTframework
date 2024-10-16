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
            TileTypes.GAUGE     => new Gauge(pos, tileSize),
            TileTypes.POT     => new Pot(pos, tileSize),
            _ => throw new InvalidEnumArgumentException("Wrong Tile type"),
        };
    }
    

    /// <summary>
    /// Function resposible for tile creation.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="tileSize"></param>
    /// <param name="tileType"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException"></exception>
    public static Tile CreateTile(Position pos, float tileSize, string tileType)
    {
        //TODO:Get name of Enum. Hacky...
        return tileType switch
        {
            "BUTTON" => new Tiles.Button(pos, tileSize),
            "GRAPH"  => new Graph(pos, tileSize),
            "LED"    => new Led(pos, tileSize),
            "GAUGE"  => new Gauge(pos, tileSize),
            "POT"  => new Pot(pos, tileSize),
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

