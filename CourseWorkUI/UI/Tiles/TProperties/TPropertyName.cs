using CourseWorkUI.UI.Tiles.TProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkUI.UI.Tiles.TPropertiesp;

public class TPropertyName : TProperty
{
    public TPropertyName(string name) : base(name)
    {
        
    }

    public override Entry ToXaml(VerticalStackLayout vs)
    {
        Entry entry = new Entry
        {
            Text = Value,
            TextColor = Colors.Grey,
            FontFamily = "Tomorrow-Regular.ttf",
            FontSize = 24,
            BackgroundColor = Color.FromArgb("#121212"),
            MaxLength = 6,
            HorizontalOptions = LayoutOptions.End
        };

        vs.Add(
            new Grid {
                new Label
                {
                    Text = "Name: ",
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
