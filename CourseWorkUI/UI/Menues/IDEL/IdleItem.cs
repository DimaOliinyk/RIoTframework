using CourseWorkUI.Model;

namespace CourseWorkUI.UI.Menues.IDEL;

public class IdleItem
{
    private SwipeGestureRecognizer _swipeToDelete;
    public IdleItem()
    {
        _swipeToDelete = new SwipeGestureRecognizer
        {
            Direction = SwipeDirection.Right
        };

        _swipeToDelete.Swiped += (s, e) => IDLEPage.RemoveIDLERow((Entry)s!);
    }

    public int? TimeDelta { get; set; }
    public PinModel? PinModel { get; set; }

    public Entry[] ToXaml(VerticalStackLayout vs) 
    {
        var entrTime = (TimeDelta != null) ? InitEntry($"{TimeDelta}") : InitEntry("","Time");
        var entrPin = (PinModel != null) ? InitEntry($"{PinModel.Number}") : InitEntry("", "Pin");
        var entrVal = (PinModel != null) ? InitEntry($"{PinModel.Value}") : InitEntry("", "Value");
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
        {
            grid.Add(entry, 0, count++); 
        }

        vs.Add(grid);
        return entries;
    }

    private Entry InitEntry(string Value, string PalceHolder="None", int maxLength = 5) 
    {
        var entry = new Entry
        {
            Text = (Value!="") ? Value : null,
            TextColor = ColorDictionary.TextColor,
            BackgroundColor = ColorDictionary.TileBackground,
            FontFamily = "Tomorrow-Regular.ttf",
            FontSize = 24,
            Placeholder = PalceHolder,
            PlaceholderColor = ColorDictionary.TextColor
        };
        entry.GestureRecognizers.Add(_swipeToDelete);
        return entry;
    }
}
