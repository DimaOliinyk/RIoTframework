using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Diagnostics;

namespace CourseWorkUI.Model;

/// <summary>
/// Performs actions on file which stores project data
/// </summary>
public static class FileSaver
{
    public static string Name = "RIoT";     // Default projects name (without extention)
    private static string _path;            // Path to the file (without name)
    private const string _extention =  
#if WINDOWS
    ".riot";  // Extention of project file
#elif ANDROID
     "";  // Extention of project file
#endif

    static FileSaver()
    {
        ResetPath();
    }

    /// <summary>
    /// Writes to file the passed data
    /// </summary>
    /// <param name="data"></param>
    public static void Save(string data) 
    {
        // Gets full path
        var fullpath = Path.Combine(_path, Name+_extention);
        if (Path.Exists(fullpath))              // if exists => rewrite the file
            File.WriteAllText(fullpath, data);
        else                                    // if doesn't exist => create the file
            File.AppendAllText(fullpath, data);
    }

    /// <summary>
    /// Reads from file and writes the data to string.
    /// Gets the file by name and path as separate args.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string? Open(string path, string name) 
    {
        string? data = "";
#if WINDOWS
        using (StreamReader inputFile =
            new StreamReader(Path.Combine(path, Name+_extention)))
        {
            data = inputFile.ReadLine();
        }
#elif ANDROID
        using (StreamReader inputFile =
            new StreamReader(Path.Combine(_path, Name+_extention)))
        {
            data = inputFile.ReadLine();
        }
#endif
        return data;
    }

    /// <summary>
    /// Reads from file and writes the data to string.
    /// Gets the file full path.
    /// </summary>
    /// <param name="fullpath"></param>
    /// <returns></returns>
    public static string? Open(string fullpath)
    {
        string? data;
#if WINDOWS
        Name = Path.GetFileNameWithoutExtension(fullpath);          // Gets the name of file without extention
        _path = fullpath.Replace(Path.GetFileName(fullpath), "");   // Gets the path without name of the file and its extention
#elif ANDROID
        fullpath = Path.Combine(_path, fullpath+_extention);
#endif
        using (StreamReader inputFile = 
            new StreamReader(fullpath))
        {
            data = inputFile.ReadLine();
        }
        return data;
    }


    /// <summary>
    /// Writes to file
    /// </summary>
    /// <param name="name"></param>
    public static void Create(string name)
    {
        Name = name;
        using (StreamWriter outputFile = 
            new StreamWriter(Path.Combine(_path, Name+_extention)))
        {
            outputFile.Write("_");  // -_-
        }
    }

    /// <summary>
    /// Resets the path to default
    /// </summary>
    public static void ResetPath() 
    {
#if WINDOWS
        _path = Environment.GetFolderPath(
            Environment.SpecialFolder.MyDocuments);
#elif ANDROID
        _path = _path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).AbsolutePath;
#endif
    }
}
