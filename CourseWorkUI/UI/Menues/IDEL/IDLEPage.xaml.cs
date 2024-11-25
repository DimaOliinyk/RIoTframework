
namespace CourseWorkUI.UI.Menues.IDEL;

public partial class IDLEPage : ContentPage
{
    //TODO: Create arrays of integer values to refrence in Idle Mode and send data
    private static List<Entry[]> _entries = new List<Entry[]>();
    private Button _addBtn = DrawAddBtn();

    public IDLEPage()
	{
		InitializeComponent();
        _addBtn.Clicked += AddEntry!;

        for(int i = 0; i < _entries.Count; i++) 
        {
            var entry = _entries[i];
            var slot = new IdleItem()
            {
                TimeDelta = Int32.Parse(entry[0].Text),
                PinModel = new(Int32.Parse(entry[1].Text), Int32.Parse(entry[2].Text))
            };
            //_entries[i] = slot.ToXaml(VSMain);
            slot.ToXaml(VSMain);
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
            TextColor = Colors.Grey
        };
    }

    private void BtnSave_Clicked(object sender, EventArgs e)
    {
        Func<int, string> DecideErrorPos = (int x) => x switch
        {
            0 => "Time",
            1 => "Pin",
            _ => "Value"
        };

        // Check Properties before leaving
        for (int i = 0; i < _entries.Count; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (!Int32.TryParse(_entries[i][j].Text, out int res)) 
                {
                    DisplayAlert("",$"Wrong Value at row {i+1} and "+ DecideErrorPos(j), "OK");
                    return;
                }
            }
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
}