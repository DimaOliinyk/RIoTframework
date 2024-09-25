namespace CourseWorkUI.UI.Menues;

public partial class FilesMenu : ContentPage
{
	public FilesMenu()
	{
		InitializeComponent();
	}

    private void BtnOpen_Clicked(object sender, EventArgs e)
    {

    }

    private void BtnNew_Clicked(object sender, EventArgs e)
    {

    }

    private void BtnSave_Clicked(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }
}