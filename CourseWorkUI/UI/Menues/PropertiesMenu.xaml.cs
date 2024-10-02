namespace CourseWorkUI.UI.Menues;

public partial class PropertiesMenu : ContentPage
{
    // Saves refrences to Microsoft.Maui.Controls.Entry
    // to get passed data
    private List<Entry> _entries;    
    private Tile _tile;         // Tile passed to class 
    private TileGrid _tileGrid; // Grid passed to class
    private int _rowCounter;    // Row counter to be passed to each Property

    public PropertiesMenu(Tile tile, TileGrid grid)
	{
		InitializeComponent();

        _entries = new List<Entry>();   
        _tile = tile;
        _tileGrid = grid;
        _rowCounter = 0;

        if (_tile.Properties is not null) 
        {
            foreach (var prop in _tile.Properties)
            {
                // fills the entries list with passed entries from each property
                _entries.Add(prop.ToXaml(VSProperties, _rowCounter++));
            }
        }
	}

    /// <summary>
    /// Event handler for Delete button.
    /// Deletes Tile from Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnDelete_Clicked(object sender, EventArgs e)
    {
        _tileGrid.RemoveTile(_tile);
        Navigation.PopModalAsync();
    }

    /// <summary>
    /// Event handler for Save button.
    /// Sets all passed values of properties 
    /// to Tile's properties 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnSave_Clicked(object sender, EventArgs e)
    {
        // TODO: Check for correct pin number
        for (int i = 0; i < _entries.Count; i++)
        {
            if (string.IsNullOrEmpty(_entries[i].Text))  // checks whether passed strings are not null
            {
                DisplayAlert("Error", "Values cannot be empty", "OK");
                return;
            }
            _tile.Properties[i].Value = _entries[i].Text;   // sets all passed properties to tile properties

            if (!_tile.Properties[i].IsCorrect()) // checks whether properties are correct
            {
                DisplayAlert("Error", "Values cannot be empty", "OK");
                return;
            }
        }
       
        Navigation.PopModalAsync();
    }
}