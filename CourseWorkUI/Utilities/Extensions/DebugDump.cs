using System.Diagnostics;
using System.Reflection;

namespace CourseWorkUI.Utilities.Extensions;

// https://github.com/MoaidHathot/Dumpify?tab=readme-ov-file
public static class ObjectExtentions
{
    public static void DebugDump(this object ob) 
    {
#if DEBUG
        Type type = ob.GetType();
        Debug.WriteLine($"┍────── {type} ───────");
        Debug.WriteLine($"┝             Properties ");
        foreach (var prop in type.GetProperties())
        {
            Debug.WriteLine($"│  {prop.Name}: {prop.GetValue(ob)}");
        }
        Debug.WriteLine($"│\n┝             Fields ");
        foreach (var prop in type.GetFields(
                                      //BindingFlags.NonPublic |
                                      BindingFlags.Instance | 
                                      BindingFlags.Public | 
                                      BindingFlags.Static))
        {
            Debug.WriteLine($"│  {prop.Name}: {prop.GetValue(ob)}");
        }
        Debug.WriteLine($"┕──────────────\n");
#endif
    }
}
