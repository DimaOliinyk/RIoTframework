using CourseWorkUI.Model;
using CourseWorkUI.UI;
using CourseWorkUI.UI.Menues;
using CourseWorkUI.UI.Tiles;
using CourseWorkUI.Utilities;
using System.Net.NetworkInformation;

namespace CourseWorkUI.Controller;

public class CircuitController
{
    private static Tile?[] UsedPins;
    public static string? IP
    {
        get => CircuitModel.IPAddress;
        set => CircuitModel.IPAddress = value;
    }
    public static double Delay 
    { 
        get => CircuitModel.TimeDelay; 
        set => CircuitModel.TimeDelay = value; 
    }

    public static void SendData(int pin, int value)
    {
        CircuitModel.Send(CircuitInterpreter.Encode(pin, value));
    }

    public static async Task StartDataChecking()
    {
        string res = await CircuitModel.StartDataChecking();
        try
        {
            res = CircuitInterpreter.ExtractData(res);
        }
        catch 
        {
            return;
        }
        CircuitInterpreter.Decode(res);
    }

    public static void PrepareData(List<TileGrid> grids) 
    {
        UsedPins = new Tile[TileFactory.MaxNumberOfPins];
        foreach (TileGrid grid in MainPage.tileGrids)
        {
            foreach (Tile tile in grid.Tiles)
            {
                if (tile is IOutput)
                {
                    UsedPins[tile.GetPin()] = tile; 
                }
            }
        }
    }

    public static void SendDataToPin(int pin, int val)
    {
        try
        {
            if (UsedPins[pin] == null)
                return;
            ((IOutput)UsedPins[pin]!).SetValue(val);
        }
        catch 
        {
        }   
    }
}
