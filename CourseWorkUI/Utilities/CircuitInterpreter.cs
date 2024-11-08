using CourseWorkUI.Controller;
using CourseWorkUI.Model;
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

    // TODO: Not tested
    public static bool Decode(string data)
    {
        CircuitModel.Pins.Clear();
        try
        {
            if (String.IsNullOrEmpty(data)) return false;

            foreach (var pinVal in data.Split("\n"))
            {
                int pin = int.Parse(pinVal.Split("_")[0]);
                int val = int.Parse(pinVal.Split("_")[1]);
                if (val == null || pin == null)
                    return false;
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

    public static string ExtractData(string data) => data.Split("\r\n")[0];
}
