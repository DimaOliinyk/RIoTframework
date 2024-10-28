using CourseWorkUI.UI;
using CourseWorkUI.UI.Tiles.TProperties;

namespace CourseWorkUI.View.Tiles.TProperties;

public class TPropertyState : TProperty
{
    private string _label;

    //Alias for base class property Value
    public bool Value 
    {
        set 
        {
            base.Value = value.ToString();
        } 
        get => base.Value == true.ToString();
    }

    public TPropertyState(string label, bool trueByDefault = false) 
        : base(trueByDefault.ToString())
    {
        Value = trueByDefault;
        _label = label;
    }

    public override IView ToXaml(Grid vs, int rowCount)
    {
        var entry = new Switch
        {
            IsToggled = (Value == true),
            // TODO:FIXMELATER: I guess colors just don't do anything
            BackgroundColor = ColorDictionary.Background,   
            OnColor = ColorDictionary.TransparentPrimary,   
            ThumbColor = ColorDictionary.Primary,
            Scale = 1.5,
            Margin = new Thickness(50, 10, 10, 10),
            HorizontalOptions = LayoutOptions.Start,
        };

        vs.Add(new Label
        {
            Text = _label+": ",
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
