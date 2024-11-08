using CourseWorkUI.Controller;
using CourseWorkUI.UI.Tiles.TProperties;
using CourseWorkUI.Utilities.Exceptions;

namespace CourseWorkUI.UI.Menues;

// TODO:Important: Change TProperty to TPropertyNumber of Pins 

public partial class SettingsMenu : ContentPage
{
    private static bool _pageIsOpen = false;
    private int _rowCounter;
    private List<IView> _entries = new();

    public SettingsMenu()
	{
        if (_pageIsOpen)
            throw new SinglePageException();
        _pageIsOpen = true;

        InitializeComponent();

        _rowCounter = 0;
        foreach (TProperty prop in WIFIProperties.Properties) 
        {
            _entries.Add(prop.ToXaml(GrdProperties, _rowCounter++));
        }
    }

    private void BtnSave_Clicked(object sender, EventArgs e)
    {
        for (int i = 0; i < _entries.Count; i++)
        {
            if (string.IsNullOrEmpty(_entries[i] is Entry ?
                                    ((Entry)_entries[i]).Text : "+"))
            {
                DisplayAlert("Error", "Values cannot be empty", "OK");
                return;
            }
            if (_entries[i] is Entry)
            {
                WIFIProperties.Properties[i].Value = ((Entry)_entries[i]).Text;   // saves all properties
            }
            if (!WIFIProperties.Properties[i].IsCorrect()) // checks whether properties are correct
            {
                DisplayAlert("Error", $"Wrong value detected at row {i+1}", "OK");
                return;
            }
        }
        Navigation.PopModalAsync();
    }

    private void BtnCancel_Clicked(object sender, EventArgs e) => Navigation.PopModalAsync();

    protected override void OnDisappearing() => _pageIsOpen = false;
}