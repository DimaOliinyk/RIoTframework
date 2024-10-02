namespace CourseWorkUI.UI.Tiles.TProperties;

// TODO: Make function generic
public class TPropertyValue : TProperty
{
    private string _name;

    public TPropertyValue(string numValue, string name) : base(numValue)
    {
        _name = name;
    }

    public bool GetNumber(out double numValue) 
    {
        if (Double.TryParse(Value, out double result)) 
        {
            numValue = result;
            return true;
        }
        numValue = default;
        return false;
    }

    public double GetNumber() 
    {
        return Double.Parse(Value);
    }

    public override bool IsCorrect()
    {
        return GetNumber(out double dummyValue);
    }

    public override Entry ToXaml(Grid vs, int rowCount)
    {
        Entry entry = new Entry
        {
            Text = Value,
            TextColor = ColorDictionary.TextColor,
            FontFamily = "Tomorrow-Regular.ttf",
            FontSize = 24,
            BackgroundColor = ColorDictionary.Background,
            MaxLength = 6,
        };

        vs.Add(new Label
        {
            Text = _name + ": ",
            TextColor = ColorDictionary.TextColor,
            FontFamily = "Tomorrow-Regular.ttf",
            FontSize = 24,
            BackgroundColor = ColorDictionary.Background,
            Padding = new Thickness(15, 15, 15, 15)
        }, 0, rowCount);

        vs.Add(entry, 1, rowCount);

        return entry;
    }
}
