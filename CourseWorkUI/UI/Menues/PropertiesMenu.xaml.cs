using CourseWorkUI.Utilities.Exceptions;

namespace CourseWorkUI.UI.Menues;

public partial class PropertiesMenu : ContentPage
{
    // Saves refrences to Microsoft.Maui.Controls.Entry
    // to get passed data
    private List<IView> _entries;    
    private Tile _tile;         // Tile passed to class 
    private TileGrid _tileGrid; // Grid passed to class
    private int _rowCounter;    // Row counter to be passed to each Property

    private static bool _pageIsOpen = false;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tile"></param>
    /// <param name="grid"></param>
    /// <exception cref="SinglePageException"></exception>
    public PropertiesMenu(Tile tile, TileGrid grid)   // Dependency injection 💉
    {
        if (_pageIsOpen)
            throw new SinglePageException();
        _pageIsOpen = true;

        InitializeComponent();

        _entries = new List<IView>();   
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
            // checks whether passed strings are not null
            if (string.IsNullOrEmpty(_entries[i] is Entry ? 
                                    ((Entry)_entries[i]).Text : "+"))
            {
                DisplayAlert("Error", "Values cannot be empty", "OK");
                return;
            }
            if (_entries[i] is Entry)
                _tile.Properties[i].Value = ((Entry)_entries[i]).Text;   // sets all passed properties to tile properties
            else
                _tile.Properties[i].Value = ((Switch)_entries[i]).IsToggled.ToString();

            if (!_tile.Properties[i].IsCorrect()) // checks whether properties are correct
            {
                DisplayAlert("Error", "Values cannot be empty", "OK");
                return;
            }
        }
       
        Navigation.PopModalAsync();
    }

    protected override void OnDisappearing()
    {
        _pageIsOpen = false;
    }
}