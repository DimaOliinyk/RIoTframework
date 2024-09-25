using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkUI.UI.Tiles.TProperties;

public abstract class TProperty
{
    public string Value { get; set; }

    protected TProperty(string value)
    {
        Value = value;
    }

    public virtual Entry ToXaml(VerticalStackLayout vs) 
    {
        return new Entry();
    }
}
