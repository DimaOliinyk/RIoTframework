namespace CourseWorkUI.UI.Tiles.TProperties;

public class LogicalProperty : TProperty
{
    private delegate bool Condition(int x);
    private Condition? EvaluateExp = null;

    public LogicalProperty(string value) : base(value)
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
            Placeholder="None"
        };

        vs.Add(new Label
        {
            Text = "Expression: ",
            TextColor = ColorDictionary.TextColor,
            FontFamily = "Tomorrow-Regular.ttf",
            FontSize = 24,
            BackgroundColor = ColorDictionary.Background,
            Padding = new Thickness(15, 15, 15, 15)
        }, 0, rowCount);

        vs.Add(entry, 1, rowCount);

        return entry;
    }


    public override bool IsCorrect()
    { 
        var val = Value.Split(' ')[1];
        var cond = Value.Split(' ')[0];

        try
        {
            EvaluateExp = ConvertCondition();
        }
        catch
        {
            return false;
        }
        return true;
    }

    private Condition ConvertCondition()
    {
        if (String.IsNullOrEmpty(Value) || 
            Value != "None" ||
            !Value.Contains(' ') || 
            !Int32.TryParse(Value.Split(' ')[1], out int condVal)) 
            throw new ArgumentException();

        switch (Value.Split(' ')[0])
        { 
            case "<":
                return (int x) => { return x < condVal; };
            case ">":
                return (int x) => { return x > condVal; };
            case "=":
                return (int x) => { return x == condVal; };
            case "<=":
                return (int x) => { return x <= condVal; };
            case ">=":
                return (int x) => { return x >= condVal; };
            default:
                throw new ArgumentException();
        }
    }

    public bool ConditionIsTrue(int value) 
    {
        if (EvaluateExp is null)
            return false;
        try
        {
            return EvaluateExp(value);
        }
        catch 
        {
            return false;
        }
    }
}
