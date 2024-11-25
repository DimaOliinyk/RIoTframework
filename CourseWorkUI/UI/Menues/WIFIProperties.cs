using CourseWorkUI.Controller;
using CourseWorkUI.UI.Tiles.TProperties;
using CourseWorkUI.UI.Tiles.TProperties.SettingsProperties;
using CourseWorkUI.Utilities;

namespace CourseWorkUI.UI.Menues;

public static class WIFIProperties
{
    public static List<TProperty> Properties { get; set; } = new();
    private static TIPProperty _ip;
    private static TPropertyValue<double> _delay;
        
    static WIFIProperties()
    {
        Properties.Add(_ip = new TIPProperty("192.168.0.188"));
        Properties.Add(_delay = new TPropertyValue<double>("1000", "Delay"));   
    }

    /// <summary>
    /// Extra verification of settings
    /// </summary>
    /// <returns></returns>
    public static bool TransferDataAndCheck() 
    {
        if (_delay.GetNumber() < 0.0 || 
            !_ip.IsCorrect()) 
        {
            return false;
        }
        CircuitController.IP = _ip.Value;
        CircuitController.Delay = _delay.GetNumber();
        return true;
    }
}
