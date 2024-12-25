using CourseWorkUI.Controller;
using CourseWorkUI.Model;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace CourseWorkUI.Utilities;

/*
 *  - Outcoming data is converted to JSON format
 *  - Incoming data is of CSV format
 *
 *  When Run button is pressed initial data gets 
 *  gathred converted and sent to IoT device. Also
 *  async call to send GET request and delay is called.
 */

public static class CircuitInterpreter
{
    /// <summary>
    /// Encodes Pin Val data to Json and then to HTTP Content
    /// </summary>
    /// <param name="pin"></param>
    /// <param name="val"></param>
    /// <returns></returns>
    public static StringContent Encode(int pin, int val)
    {
        string json = JsonSerializer.Serialize(new PinModel(pin, val));
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        return content;
    }

    /// <summary>
    /// Decodes data recieved from IoT device.
    /// Data format is CSV
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Removes Header of incoming responce
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string ExtractData(string data) => (!String.IsNullOrEmpty(data) ) ? 
                                                            data.Split("\r\n")[0] : "";
    
    /// <summary>
    /// Encodes list of monitored pins into json format
    /// </summary>
    /// <param name="monitoredPins"></param>
    /// <returns></returns>
    public static StringContent MonitoredPins(List<int> monitoredPins)
    {
        string json = JsonSerializer.Serialize(monitoredPins.ToArray());
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        return content;
    }

    /// <summary>
    /// Converts int value to respective 
    /// float value with specified resolution
    /// </summary>
    /// <param name="val"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="resolution"></param>
    /// <returns></returns>
    public static float ConvertIntToFloat(int val, float min, float max, int resolution = 255) =>
        Math.Clamp(val/(float)resolution * (max - min) + min, min, max);

    /// <summary>
    /// Converts float value to respective 
    /// int value with specified resolution
    /// </summary>
    /// <param name="val"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="resolution"></param>
    /// <returns></returns>
    public static int ConvertFloatToInt(float val, float min, float max, int resolution = 255) =>
        (int)Math.Clamp(((val - min) / (max - min) * resolution), min, max);

    /// <summary>
    /// Encodes initial data into json format string
    /// </summary>
    /// <param name="dbName"></param>
    /// <param name="delay"></param>
    /// <param name="monitoredPins"></param>
    /// <returns></returns>
    public static StringContent InitialData(string dbName, double delay, List<int> monitoredPins)
    {
        string json = JsonSerializer.Serialize(monitoredPins.ToArray()) + JsonSerializer.Serialize(delay) + JsonSerializer.Serialize(dbName);
        Debug.WriteLine(json);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        return content;
    }
}