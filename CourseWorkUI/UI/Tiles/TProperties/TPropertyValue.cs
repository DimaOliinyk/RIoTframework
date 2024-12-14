using System.ComponentModel;
using System.Numerics;

namespace CourseWorkUI.UI.Tiles.TProperties;


#if NONGENERIC_OPTION_ENABLE
public class TPropertyValue : TProperty
{
    private string _name;

    public TPropertyValue(string numValue, string name) : base(numValue)
    {
        _name = name;
    }

    public bool GetNumber(out double numValue) 
    {
        if (Double.TryParse(Value, out double result)) 
        {
            numValue = result;
            return true;
        }
        numValue = default;
        return false;
    }

    public double GetNumber() 
    {
        return Double.Parse(Value);
    }

    public override bool IsCorrect()
    {
        return GetNumber(out double _);
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
            Text = _name + ": ",
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
#endif

public class TPropertyValue<T> : TProperty where T : INumber<T>
{
    private string _name;

    public TPropertyValue(string numValue, string name) : base(numValue)
    {
        _name = name;
    }

    public bool GetNumber(out T numValue)
    {
        try 
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            T res = (T)converter.ConvertFromString(Value)!;
            if (res != null)
            {
                numValue = res;
                return true;
            }
        } 
        catch 
        {
        }
        numValue = default(T)!;
        return false;
    }

    public T GetNumber()
    {
        var converter = TypeDescriptor.GetConverter(typeof(T));
        T res = (T)converter.ConvertFromString(Value)!;
        if (res == null) throw new Exception("Convertion error");
        return res;
    }

    public override bool IsCorrect() => GetNumber(out T _);

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
            Text = _name + ": ",
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