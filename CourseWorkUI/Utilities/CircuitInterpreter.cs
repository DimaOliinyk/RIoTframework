using CourseWorkUI.Controller;
using CourseWorkUI.Model;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace CourseWorkUI.Utilities;

public class CircuitInterpreter
{
    public static StringContent Encode(int pin, int val)
    {
        string json = JsonSerializer.Serialize(new PinModel(pin, val));
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        return content;
    }

    public static bool Decode(string data)
    {
        CircuitModel.Pins.Clear();
        
        try
        {
            if (String.IsNullOrEmpty(data)) return false;

            var PinValArr = data.Split(",");

            for (int i = 0; i < PinValArr.Length; i+=2) 
            {
                int pin = int.Parse(PinValArr[i]);
                int val = int.Parse(PinValArr[i+1]);
                
                CircuitModel.Pins.Add(new PinModel(pin, val));
                CircuitController.SendDataToPin(pin, val);
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static string ExtractData(string data) => (!String.IsNullOrEmpty(data) ) ? 
                                                            data.Split("\r\n")[0] : "";

    public static StringContent MonitoredPins(List<int> monitoredPins)
    {
        string json = JsonSerializer.Serialize(monitoredPins.ToArray());
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        return content;
    }

    public static float ConvertIntToFloat(int val, float min, float max, int resolution = 255) =>
        Math.Clamp(val/(float)resolution * (max - min) + min, min, max);

    public static int ConvertFloatToInt(float val, float min, float max, int resolution = 255) =>
        (int)Math.Clamp(((val - min) / (max - min) * resolution), min, max);

    public static StringContent InitialData(string dbName, double delay, List<int> monitoredPins)
    {
        string json = JsonSerializer.Serialize(monitoredPins.ToArray()) + JsonSerializer.Serialize(delay) + JsonSerializer.Serialize(dbName);
        Debug.WriteLine(json);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        return content;
    }
}