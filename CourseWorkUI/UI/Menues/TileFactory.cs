using CourseWorkUI.UI.Tiles;
using CourseWorkUI.Utilities;
using System.ComponentModel;

namespace CourseWorkUI.UI.Menues;

public static class TileFactory
{
    public static Tile CreateTile(Position pos, double tileSize, TileTypes tileType) 
    {
        return tileType switch
        {
            TileTypes.BUTTON => new Tile(pos, tileSize, tileSize),
            TileTypes.GRAPH => new Graph(pos, tileSize, tileSize),
            _ => throw new InvalidEnumArgumentException("Wrong Tile type"),
        };
    }

    public static TileTypes StringToEnum(string tyleType) 
    {
        foreach (TileTypes i in Enum.GetValues(typeof(TileTypes))) 
        {
            if (tyleType == i.ToString()) 
            {
                return i;
            }
        }

        throw new InvalidEnumArgumentException("Invalid String");
    }
}

public enum TileTypes 
{
    BUTTON = 0,
    GRAPH = 1,
}

