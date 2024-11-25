using CourseWorkUI.Model;

namespace CourseWorkUI.UI.Menues.IDEL;

public class IdleItem
{
    public int? TimeDelta { get; set; }
    public PinModel? PinModel { get; set; }

    public Entry[] ToXaml(VerticalStackLayout vs) 
    {
        var entrTime = InitEntry((TimeDelta != null) ? $"{TimeDelta}" : "Time");
        var entrPin = InitEntry((PinModel != null) ? $"{PinModel.Number}" : "Pin");
        var entrVal = InitEntry((PinModel != null) ? $"{PinModel.Value}" : "Value");
        var entries = new Entry[] { entrTime, entrPin, entrVal };

        var grid = new Grid
        {
            Margin = new Thickness(5, 5, 5, 5),
            BackgroundColor = ColorDictionary.TileBackground,
            RowDefinitions = 
            {
                new RowDefinition(),
                new RowDefinition(),
                new RowDefinition()
            }
        };

        int count = 0;
        foreach (var entry in entries)
            grid.Add(entry, 0, count++);

        vs.Add(grid);
        return entries;
    }

    private Entry InitEntry(string PalceHolder, int maxLength = 5) 
    {
        return new Entry
        {
            BackgroundColor = ColorDictionary.TileBackground,
            //TODO: Add font&size
            Placeholder = PalceHolder,
            PlaceholderColor = ColorDictionary.TextColor,
        };
    }
}
