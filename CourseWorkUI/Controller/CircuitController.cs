using CourseWorkUI.Model;
using CourseWorkUI.UI;
using CourseWorkUI.UI.Menues;
using CourseWorkUI.UI.Tiles;
using CourseWorkUI.Utilities;

namespace CourseWorkUI.Controller;

public class CircuitController
{
    //                (⁠ʘ⁠ᗩ⁠ʘ⁠’)
    private static Tile?[]? UsedPins;               // Array of used output pins 
    public static event Action? UpdateScreen;       // Event to update main screen when data recieved
    private static List<int> MonitoredPins = new(); // List of pins that will be saved to db
    public static string? IP        // IP of device
    {
        get => CircuitModel.IPAddress;
        set => CircuitModel.IPAddress = value;
    }
    public static double Delay      // Delay between get requests
    { 
        get => CircuitModel.TimeDelay; 
        set => CircuitModel.TimeDelay = value; 
    }

    /// <summary>
    /// Sends data to IoT device
    /// </summary>
    /// <param name="pin"></param>
    /// <param name="value"></param>
    public async static void SendData(int pin, int value) => 
        await CircuitModel.Send(CircuitInterpreter.Encode(pin, value));

    /// <summary>
    /// Starts async sending of get requests to IoT device with delay
    /// </summary>
    /// <returns></returns>
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

    // Prepares arrays of tiles where their pin number is index
    // in order to reduce time complexity
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

    /// <summary>
    /// Sends intial data to server:  
    /// Project name,
    /// Delay,
    /// and monitored pins
    /// </summary>
    /// <returns></returns>
    private static async Task SendInitialData()
    {
        if(MonitoredPins.Count != 0)
        { 
            await CircuitModel.Send(CircuitInterpreter.InitialData(FileController.GetProjectName(), Delay, MonitoredPins)); 
        }
    }

    /// <summary>
    /// When data recieved, passes value to tile and updates main screen
    /// </summary>
    /// <param name="pin"></param>
    /// <param name="val"></param>
    public static void SendDataToPin(int pin, int val)
    {
        try
        {
            if (UsedPins![pin] == null)
                return;
            ((IOutput)UsedPins[pin]!).SetValue(val);
            if (UpdateScreen != null) UpdateScreen();
        }
        catch { }   
    }

    /// <summary>
    /// Gets called to send values to pins with data 
    /// interval specified in IDLE page by user
    /// </summary>
    /// <param name="TimePinVal"></param>
    /// <returns></returns>
    public static async Task StartIDLEDataSending(List<int[]> TimePinVal) 
    {
        try
        { 
            await CircuitModel.StartAutoDataSending(TimePinVal); 
        }
        catch { }
    }
}
