using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
public static class FileHandler
{
    public static string resultsFile = Path.Combine(Application.streamingAssetsPath, "GameData", "levelsResult.txt");
    public static string shopInfoFile = Path.Combine(Application.streamingAssetsPath, "GameData", "ShopInfo.txt");
    public static string hintsInfoFile = Path.Combine(Application.streamingAssetsPath, "GameData", "HintsInfo.txt");


    public static void SaveData(string fileName, string[] lines)
    {
        File.WriteAllLines(fileName, lines);
    }

    public static string[] LoadData(string fileName)
    {
       return File.ReadAllLines(fileName);
        
    }
}
