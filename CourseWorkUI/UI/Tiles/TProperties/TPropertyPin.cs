using CourseWorkUI.UI.Menues;

namespace CourseWorkUI.UI.Tiles.TProperties;

public class TPropertyPin : TProperty
{
    public override string Value 
    {
        get => base.Value;
        set     // When reassigning value to pin number
        {       // adds previous pin number to AvailablePins set
            if (!String.IsNullOrEmpty(base.Value)
                && !TileFactory.AvailablePins.Contains(int.Parse(base.Value))) 
            { 
                TileFactory.AvailablePins.Add(int.Parse(base.Value));
            }
            base.Value = value;
        }
    }
    public TPropertyPin(string pin) : base(pin)
    {
    }

    public int GetNumber() 
    {
        return Int32.Parse(Value);
    }

    public bool TryGetNumber(out int pinNumber)
    {
        return Int32.TryParse(Value, out pinNumber);
    }

    public override bool IsCorrect() 
    {
        if (TryGetNumber(out int res)
            && TileFactory.AvailablePins.Contains(res)) 
        {
            TileFactory.AvailablePins.Remove(res);
            return true;
        }
        return false;
    }

    public override IView ToXaml(Grid vs, int rowCount)
    {
        var entry = new Entry
        {
            Text = (!string.IsNullOrEmpty(Value)) ? Value : "N/A",
            TextColor = ColorDictionary.TextColor,
            FontFamily = "Tomorrow-Regular.ttf",
            FontSize = 24,
            BackgroundColor = ColorDictionary.Background,
            MaxLength = 3,
        };

        vs.Add(new Label
        {
            Text = "Pin: ",
            TextColor = ColorDictionary.TextColor,
            FontFamily = "Tomorrow-Regular.ttf",
            FontSize = 24,
            BackgroundColor = ColorDictionary.Background,
            Padding = new Thickness(15,15,15,15)
        }, 0, rowCount);
        vs.Add(entry, 1, rowCount);

        return entry;
    }
}
