using CourseWorkUI.UI.Tiles.TProperties;

namespace CourseWorkUI.UI.Tiles.TPropertiesp;

/// <summary>
/// Class resposible for saving the name 
/// of the Tile as TProperty
/// </summary>
public class TPropertyName : TProperty
{
    public TPropertyName(string name) : base(name)
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
            MaxLength = 6,
        };

        vs.Add(new Label
        {
            Text = "Name: ",
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
