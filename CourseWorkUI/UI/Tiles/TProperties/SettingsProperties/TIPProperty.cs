
namespace CourseWorkUI.UI.Tiles.TProperties.SettingsProperties;

public class TIPProperty : TProperty
{
    public TIPProperty(string value) : base(value)
    {
    }

    public override IView ToXaml(Grid vs, int rowCount)
    {
        Entry entry = new Entry
        {
            Text = (Value.Contains("http://") ? Value.Replace("http://", "") : Value),
            TextColor = ColorDictionary.TextColor,
            FontFamily = "Tomorrow-Regular.ttf",
            FontSize = 24,
            BackgroundColor = ColorDictionary.Background,
            MaxLength = 30,
        };

        vs.Add(new Label
        {
            Text = "IP: ",
            TextColor = ColorDictionary.TextColor,
            FontFamily = "Tomorrow-Regular.ttf",
            FontSize = 24,
            BackgroundColor = ColorDictionary.Background,
            Padding = new Thickness(15, 15, 15, 15)
        }, 0, rowCount);

        vs.Add(entry, 1, rowCount);

        return entry;
    }

    public override bool IsCorrect()
    {
        try 
        {
            if (!Value.Contains("http://")) 
            {
                Value = "http://" + Value;
            }
        }
        catch 
        {
            return false;
        }
        return true;
    }
}
