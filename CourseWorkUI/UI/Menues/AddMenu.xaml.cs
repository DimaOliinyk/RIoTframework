namespace CourseWorkUI.UI.Menues;

public partial class AddMenu : ContentPage
{
	public AddMenu()
	{
		InitializeComponent();
		var arr = Enum.GetValues(typeof(TileTypes));
		var list = new List<string>();

		foreach (var t in arr) 
		{
			list.Add(t.ToString()!);	
		}

		VwCollectionView.ItemsSource = list;
    }

    private void VwCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
		MainPage.TileType = e.CurrentSelection[0].ToString();
		Navigation.PopModalAsync();		
    }

    private void BtnCancel_Clicked(object sender, EventArgs e)
    {
		MainPage.TileType = null;
        Navigation.PopModalAsync();
    }
}