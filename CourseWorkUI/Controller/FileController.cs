using CourseWorkUI.Model;
using CourseWorkUI.UI;
using CourseWorkUI.Utilities;

namespace CourseWorkUI.Controller;

/// <summary>
/// Fowards passed actions from View to
/// Model responsible for work with files
/// </summary>
public static class FileController
{
    /// <summary>
    /// Saving to file
    /// </summary>
    /// <param name="grids"></param>
    public static void Save(List<TileGrid> grids) 
    {
        FileSaver.Save(GridEncoder.Encode(grids));
    }

    /// <summary>
    /// Opening file with passed path and name separately
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static List<TileGrid> Open(string path, string name) 
    {
        string? data = FileSaver.Open(path, name);
        return GridEncoder.Decode(data);
    }

    /// <summary>
    /// Opening file with full path passed
    /// </summary>
    /// <param name="fullpath"></param>
    /// <returns></returns>
    public static List<TileGrid> Open(string fullpath)
    {
        string? data = FileSaver.Open(fullpath);
        return GridEncoder.Decode(data);
    }

    /// <summary>
    /// Creates file with passed name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static List<TileGrid> Create(string name) 
    {
        // The path to projects gets reset when new project created to keep it default
        FileSaver.ResetPath();      
        FileSaver.Name = name;

        return new List<TileGrid> 
        { 
            TileGrid.Create()
        };        
    }

    /// <summary>
    /// Returns projects name
    /// </summary>
    /// <returns></returns>
    public static string GetProjectName() => FileSaver.Name;

    /// <summary>
    /// Sets the projects name when meeting the requirements.
    /// Returns false if requirements are not met.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static bool SetProjectName(string name) 
    { 
        if(String.IsNullOrEmpty(name) ||
           name.Length > 6 ||
           name == "RIoT")
        { 
            return false; 
        }

        FileSaver.Name = name;
        return true;
    }
}
