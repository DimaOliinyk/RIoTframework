
namespace CourseWorkUI.UI.Tiles.TProperties.SettingsProperties;

public class EmailProperty : TProperty
{
    public EmailProperty(string value) : base(value)
    {
    }

    public override IView ToXaml(Grid vs, int rowCount)
    {
        Entry entry = new Entry
        {
            Text = Value,
            TextColor = ColorDictionary.TextColor,
            FontFamily = "Tomorrow-Regular.ttf",
            FontSize = 24,
            BackgroundColor = ColorDictionary.Background,
            MaxLength = 30,
            Placeholder = "Your Email",
            PlaceholderColor = ColorDictionary.TextColor
        };
        vs.SetColumnSpan(entry, 2);
        vs.Add(entry);
        
        return entry;
    }
}
