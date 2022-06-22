using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public static class FileManager
{
    public static bool WriteToFile(string a_FileName, string a_FileContents)
    {
        try
        {
            File.WriteAllText(a_FileName, a_FileContents);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to write to {a_FileName} with exception {e}");
            return false;
        }
    }

    public static bool LoadFromFile(string a_FileName, out string result)
    {
        try
        {
            result = File.ReadAllText(a_FileName);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to read from {a_FileName} with exception {e}");
            result = "";
            return false;
        }
    }
}
