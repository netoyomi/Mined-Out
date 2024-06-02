using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
public class MineHints:MonoBehaviour
{
    public int amount;
    private Text text;
    private string[] lines;

    private void Start()
    {
        lines = FileHandler.LoadData(FileHandler.hintsInfoFile);
        text = GetComponent<Text>();
        amount = int.Parse(lines[0]);
        text.text = "Mine Hints: " + amount.ToString();
    }
    public void UpdateCount()
    {
        lines[0] = amount.ToString();
        FileHandler.SaveData(FileHandler.hintsInfoFile, lines);
        text.text = "Mine Hints: " + amount.ToString();
    }
}