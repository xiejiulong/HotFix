using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class FileUtil
{
    public static string getExt(string path)
    {
        string ext = System.IO.Path.GetExtension(path);
        return ext.Equals(string.Empty) ? ext : ext.Remove(0, 1);
    }

    public static string getFileName(string path)
    {
        int beginIndex = path.LastIndexOf("/");
        int endIndex = path.LastIndexOf(".");
        beginIndex = beginIndex == -1 ? 0 : beginIndex+1;
        endIndex = endIndex == -1 ? path.Length - 1 : endIndex;
        return path.Substring(beginIndex,endIndex-beginIndex);
    }

    public static bool isExistFile(string file)
    {
        return System.IO.File.Exists(file);
    }
}

