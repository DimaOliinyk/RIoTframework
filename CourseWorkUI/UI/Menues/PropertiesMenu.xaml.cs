namespace CourseWorkUI.UI.Menues;

public partial class PropertiesMenu : ContentPage
{
    List<Entry> entries = new List<Entry>();
    private Tile _tile;
    private TileGrid _tileGrid;

    public PropertiesMenu(Tile tile, TileGrid grid)
	{
		InitializeComponent();

        _tile = tile;
        _tileGrid = grid;

        if (_tile.Properties is not null) 
        {
            foreach (var prop in _tile.Properties)
            {
                entries.Add(prop.ToXaml(VSProperties));
            }
        }
	}

    private void BtnDelete_Clicked(object sender, EventArgs e)
    {
        // TODO: make a main page gv refresh
        _tileGrid.RemoveTile(_tile);
        Navigation.PopModalAsync();
    }

    private void BtnSave_Clicked(object sender, EventArgs e)
    {
        // TODO: Check for correct pin number
        for (int i = 0; i < entries.Count; i++)
        {
            if(string.IsNullOrEmpty(entries[i].Text))
            {
                DisplayAlert("Error", "Values cannot be empty", "OK");
                return;
            }
            _tile.Properties[i].Value = entries[i].Text; 
        }
        // TODO: make a main page gv refresh
        Navigation.PopModalAsync();
    }
}