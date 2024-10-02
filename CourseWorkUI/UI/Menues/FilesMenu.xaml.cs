namespace CourseWorkUI.UI.Menues;

public partial class FilesMenu : ContentPage
{
	public FilesMenu()
	{
		InitializeComponent();
	}

    /// <summary>
    /// Event handler for Open Project button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnOpen_Clicked(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// Event handler for New Project button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnNew_Clicked(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// Event handler for Saving Project button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnSave_Clicked(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }
}