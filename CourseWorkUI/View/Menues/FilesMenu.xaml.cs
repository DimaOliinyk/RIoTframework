using CourseWorkUI.Controller;
using CourseWorkUI.Utilities.Exceptions;
using System.Diagnostics;

namespace CourseWorkUI.UI.Menues;

public partial class FilesMenu : ContentPage
{
    private static bool _pageIsOpen = false;
    private List<TileGrid> _tileGrids;

    // Define file type for .riot
    private static FilePickerFileType _riotFileType =
            new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                //TODO: add support for IOS
                //{ DevicePlatform.iOS, new[] { "public.my.comic.extension" } }, // UTType values
                { DevicePlatform.Android, new[] { "application/riot" } },         // MIME type
                { DevicePlatform.WinUI, new[] { ".riot" } },                     // file extension
                { DevicePlatform.macOS, new[] { "riot" } },                      // UTType values
            });

    /// <summary>
    /// Accepts list of Grids containing Tiles
    /// </summary>
    /// <param name="tileGrids"></param>
    /// <exception cref="SinglePageException"></exception>
    public FilesMenu(List<TileGrid> tileGrids)
	{
        if (_pageIsOpen)
            throw new SinglePageException();
        _pageIsOpen = true;

        InitializeComponent();
        _tileGrids = tileGrids;
	}

    /// <summary>
    /// Event handler for Open Project button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void BtnOpen_Clicked(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions 
        {
            PickerTitle = "Current Project: "+FileController.GetProjectName(),
#if WINDOWS     // TODO:FIXMELATER: I guess filtering works too good on Android
            FileTypes = _riotFileType,
#endif
        });

        if (result == null)
        {
            await DisplayAlert("", "Cannot open files", "OK");
            return;
        }

        try
        {
            MainPage.tileGrids = FileController.Open(result.FullPath);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            await DisplayAlert("", "Cannot open file", "OK");
        }

        await Navigation.PopModalAsync();
    }

    /// <summary>
    /// Event handler for New Project button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void BtnNew_Clicked(object sender, EventArgs e)
    {
        string propmpt = await PropmptProjectNameAsync();

        FileController.Create(propmpt);
        FileController.Save(_tileGrids);
        await Navigation.PopModalAsync();
    }

    /// <summary>
    /// Event handler for Saving Project button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void BtnSave_ClickedAsync(object sender, EventArgs e)
    {
        if (FileController.GetProjectName() == "RIoT")  // if projects is defualt
        {                                               // ask for new one
            string propmpt;
            do
            {
                propmpt = await PropmptProjectNameAsync();
            }
            while (!FileController.SetProjectName(propmpt));    // Checks if name can be set
        }
        FileController.Save(_tileGrids);                    // Saves the new project
        await Navigation.PopModalAsync();
    }

    protected override void OnDisappearing() => _pageIsOpen = false;

    /// <summary>
    /// Cancel button is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnCancel_Clicked(object sender, EventArgs e) => Navigation.PopModalAsync();

    /// <summary>
    /// Prompts the project's name with checking. Returns correct naem
    /// </summary>
    /// <returns></returns>
    private async Task<string> PropmptProjectNameAsync() 
    {
        string propmpt = await DisplayPromptAsync("", "Enter file name: ");
        while (true)
        {
            if (String.IsNullOrEmpty(propmpt))
            {
                await DisplayAlert("Incorrect data", "File name cannot be empty", "OK");
            }
            else if (propmpt == "RIoT")
            {
                await DisplayAlert("Incorrect data", "File name cannot be \"RIoT\"", "OK");
            }
            else if (propmpt.Length > 6)
            {
                await DisplayAlert("Incorrect data", "File name cannot be longer than 6 characters", "OK");
            }
            else
            {
                break;
            }
            propmpt = await DisplayPromptAsync("", "Enter file name: ");
        }

        return propmpt;
    }
}