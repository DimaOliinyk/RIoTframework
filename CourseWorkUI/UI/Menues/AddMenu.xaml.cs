using System.Linq;
namespace CourseWorkUI.UI.Menues;

/// <summary>
/// Menu responsible for asking the user 
/// types of Tiles
/// </summary>
public partial class AddMenu : ContentPage
{
	public AddMenu()
	{
		InitializeComponent();

        // Gets all Enum values and adds to CollectionView (VwCollectionView)
        var arr = Enum.GetValues(typeof(TileTypes));	
		var strList = new List<string>();

		foreach (var t in arr) 
		{
            strList.Add(t.ToString()!);	
		}

        VwCollectionView.ItemsSource = strList;
    }

    /// <summary>
    /// Event handler for when the item 
    /// in the collection view is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void VwCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Sets a static field in the main page responcible for tile type
		MainPage.TileType = e.CurrentSelection[0].ToString();
		Navigation.PopModalAsync();		
    }

    /// <summary>
    /// Event handler for when the Cnacel button is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnCancel_Clicked(object sender, EventArgs e)
    {
        // Sets a static field in the main page responcible for tile type
        MainPage.TileType = null;
        Navigation.PopModalAsync();
    }
}