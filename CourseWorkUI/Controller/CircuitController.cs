using CourseWorkUI.Model;
using CourseWorkUI.UI;
using CourseWorkUI.UI.Menues;
using CourseWorkUI.UI.Tiles;
using CourseWorkUI.Utilities;

namespace CourseWorkUI.Controller;

public class CircuitController
{
    //                (⁠ʘ⁠ᗩ⁠ʘ⁠’)
    private static Tile?[]? UsedPins;
    public static event Action? UpdateScreen;
    private static List<int> MonitoredPins = new();
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

    public async static void SendData(int pin, int value) => 
        await CircuitModel.Send(CircuitInterpreter.Encode(pin, value));

    public static async Task StartDataChecking()
    {
        string? res = await CircuitModel.StartDataChecking();
        try
        {
            res = CircuitInterpreter.ExtractData(res!);
        }
        catch 
        {
            return;
        }
        CircuitInterpreter.Decode(res);
    }

    public static async Task PrepareData(List<TileGrid> grids) 
    {
        MonitoredPins.Clear();
        UsedPins = new Tile[TileFactory.MaxNumberOfPins];
        foreach (TileGrid grid in MainPage.tileGrids)
        {
            foreach (Tile tile in grid.Tiles)
            {
                if (tile is IOutput)
                {
                    UsedPins[tile.GetPin()] = tile; 
                }
                if (tile is IDBSaveable && ((IDBSaveable)tile).SaveToDB)
                {
                    MonitoredPins.Add(tile.GetPin());
                }
            }
        }
        await SendInitialData();
    }

    private static async Task SendInitialData()
    {
        if(MonitoredPins.Count != 0)
        { 
            await CircuitModel.Send(CircuitInterpreter.InitialData(FileController.GetProjectName(), Delay, MonitoredPins)); 
        }
    }

    public static void SendDataToPin(int pin, int val)
    {
        try
        {
            if (UsedPins![pin] == null)
                return;
            ((IOutput)UsedPins[pin]!).SetValue(val);
            if (UpdateScreen != null) UpdateScreen();
        }
        catch 
        {
        }   
    }

    public static async Task StartIDLEDataSending(List<int[]> TimePinVal) 
    {
        try
        { 
            await CircuitModel.StartAutoDataSending(TimePinVal); 
        }
        catch { }
    }
}
