using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkUI.UI.Tiles.TProperties;

public class TPropertyPin : TProperty
{
    public TPropertyPin(string pin) : base(pin)
    {
    }

    public override Entry ToXaml(VerticalStackLayout vs)
    {
        var entry = new Entry
        {
            Text = (!string.IsNullOrEmpty(Value)) ? Value : "N/A",
            TextColor = Colors.Grey,
            FontFamily = "Tomorrow-Regular.ttf",
            FontSize = 24,
            BackgroundColor = Color.FromArgb("#121212"),
            MaxLength = 3,
            HorizontalOptions = LayoutOptions.End
        };

        vs.Add(
            new Grid {
                new Label
                {
                    Text = "Pin: ",
                    TextColor = Colors.Grey,
                    FontFamily = "Tomorrow-Regular.ttf",
                    FontSize = 24,
                    BackgroundColor = Color.FromArgb("#121212"),
                    Padding = new Thickness(15,15,15,15)
                },
                entry
            }
        );

        return entry;
    }
}
