using CourseWorkUI.UI;
using CourseWorkUI.UI.Menues;
using System.Diagnostics;

namespace CourseWorkUI.Utilities;

/*      
 *  Example of Project's file:
 *      _@0;0,GRAPH,Graph,,0,255,8,@0;2,GRAPH,Graph,,0,255,8,_@0;2,GRAPH,Graph,,0,255,8,@0;1,BUTTON,BTN,,_@0;3,GRAPH,Graph,,0,255,8,@0;2,GRAPH,Graph,,0,255,8,@0;1,BUTTON,BTN,,@1;0,GAUGE,Pie,,0,255,@0;0,GAUGE,Pie,,0,255,
 *  
 *  _ - Specifies the new Page
 *  @ - Specifies the new Tile
 *  , - Used to seperate properties
 *  ; - Used to seperate X and Y coordinates
 *  
 *  The coordinates of position saved to file are devided by Tile size in order 
 *  to reduce length of data and make it independent of the devices' screen size.
 *  When the project is loaded back the coordinates are multiplied by this factor.
 *  
 *  The name of the project is the name of the file and **cannot be more than 6 characters**
 *  
 *  The type of Tile is determined by .GetType() and coverted to upper case with .Upper() 
 *  and compared with ETileTypes enum to determine the type back.
 *    
 */

/// <summary>
/// Coverts Tiles' properties and positions and 
/// pages of Grids to string and vice versa
/// </summary>
public static class GridEncoder
{
    /// <summary>
    /// Converts list of Grids to string
    /// </summary>
    /// <param name="grids"></param>
    /// <returns></returns>
    public static string Encode(List<TileGrid> grids)
    {
        string data = "";
        foreach (var grid in grids)
        {
            data += $"_";               // Special symbol for page
            data += grid.ToString();    // Adds data in form of string from Tiles
        }

        return data;
    }

    /// <summary>
    /// Convert string to list of Grids
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static List<TileGrid> Decode(string? data)
    {
        List<TileGrid> grids = new List<TileGrid>();

        if (String.IsNullOrEmpty(data) || data == "_")  // Project was empty
        {
            grids.Add(TileGrid.Create());   // Creates default page
            return grids;
        }

        foreach (var val in data.Split('_')[1..])       // Tokenizes csv data by pages 
        {
            TileGrid currentGrid = TileGrid.Create();   // Creates page (Grid)
            grids.Add(currentGrid);

            foreach (var subVal in val.Split('@')[1..])     // Each new Tile is marked by @
            {
                Debug.WriteLine(subVal);
                Position pos = Position.Parse(subVal.Split(',')[0]);   // Reads Position
                pos.X *= Tile.Size;
                pos.Y *= Tile.Size;

                string tileType = subVal.Split(',')[1];                // Reads Tile's type

                // Creates Tile by specified type (string) and adds it
                var currTile = TileFactory.CreateTile(pos, Tile.Size, tileType);
                currentGrid.AddTile(currTile);

                var token = subVal.Split(',')[2..];    // Tokenizes rest of page

                // Goes through Tile's properties and copies from token
                for (int i = 0; i < currTile.Properties.Count; i++)
                {
                    currTile.Properties[i].Value = token[i];
                }
            }
        }

        return grids;
    }
}
