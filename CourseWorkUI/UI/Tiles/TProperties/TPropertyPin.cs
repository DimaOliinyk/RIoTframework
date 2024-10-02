namespace CourseWorkUI.UI.Tiles.TProperties;

public class TPropertyPin : TProperty
{
    public TPropertyPin(string pin) : base(pin)
    {
    }

    public override Entry ToXaml(Grid vs, int rowCount)
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
