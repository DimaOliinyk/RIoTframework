namespace CourseWorkUI.UI.Menues.IDEL;

public partial class IDLEPage : ContentPage
{
    public static List<int[]> TimePinVal { get; private set; } = new List<int[]>();
    private static List<Entry[]> _entries = new List<Entry[]>();
    private Button _addBtn = DrawAddBtn();

    private static event Action? UpdateScreenEvent;

    public IDLEPage()
    {
        InitializeComponent();
        _addBtn.Clicked += AddEntry!;
        UpdateScreenEvent += UpdateScreen;
        UpdateScreen();
    }

    void UpdateScreen() 
    {
        VSMain.Children.Clear();
        for (int i = 0; i < _entries.Count; i++)
        {
            var entry = _entries[i];
            try
            {
                var slot = new IdleItem()
                {
                    TimeDelta = Int32.Parse(entry[0].Text),
                    PinModel = new(Int32.Parse(entry[1].Text), Int32.Parse(entry[2].Text))
                };
                _entries[i] = slot.ToXaml(VSMain);
            }
            catch
            {
                _entries.Remove(entry);
            }
        }
        VSMain.Add(_addBtn);
    }

    static private Button DrawAddBtn()
    {
        return new Button
        {
            VerticalOptions = LayoutOptions.Center,
            Style = (Style)Application.Current!.Resources["CancelButton"],
            Text = "+",
            TextColor = Colors.Grey,            
        };
    }

    private void BtnSave_Clicked(object sender, EventArgs e)
    {
        Func<int, string> DecideErrorPos = (int x) => x switch
        {
            (int)RowPosition.TIME => "Time",
            (int)RowPosition.PIN => "Pin",
            (int)RowPosition.VAL => "Value",
            _ => throw new ArgumentException("Enum with respective value not found")
        };

        // Check Properties before leaving
        for (int i = 0; i < _entries.Count; i++)
        {
            if (!Int32.TryParse(_entries[i][(int)RowPosition.TIME].Text, out int _))
            {
                DisplayAlert("", $"Wrong Value at row {i + 1} and entry 'Time'", "OK");
                return;
            }
            for (int j = (int)RowPosition.PIN; j <= (int)RowPosition.VAL; j++)
            {
                if (!Int32.TryParse(_entries[i][j].Text, out int _))
                {
                    DisplayAlert("", $"Wrong Value at row {i + 1} and " + DecideErrorPos(j), "OK");
                    return;
                }
            }
        }
        TimePinVal.Clear();
        for (int i = 0; i < _entries.Count; i++)
        {
            var timePinValRow = new int[3];
            for (int j = 0; j < 3; j++)
            {
                timePinValRow[j] = Int32.Parse(_entries[i][j].Text);
            }
            TimePinVal.Add(timePinValRow);
        }
        Navigation.PopModalAsync();
    }

    private void AddEntry(object sender, EventArgs e)
    {
        VSMain.Remove(_addBtn);
        var item = new IdleItem();
        _entries.Add(item.ToXaml(VSMain));
        VSMain.Add(_addBtn);
    }

    public static void RemoveIDLERow(Entry entry) 
    {
        for (int i = 0; i < _entries.Count; i++)
        {
            for (int j = 0; j < _entries[i].Length; j++)
            {
                if (entry == _entries[i][j]) 
                {
                    _entries.Remove(_entries[i]);
                    UpdateScreenEvent?.Invoke();
                    return;
                }
            }
        }
    }

    private void BtnClear_Clicked(object sender, EventArgs e)
    {
        TimePinVal.Clear();
        _entries.Clear();
        Navigation.PopModalAsync();
    }

    public enum RowPosition 
    {
        TIME = 0,
        PIN,
        VAL,
    }    
}