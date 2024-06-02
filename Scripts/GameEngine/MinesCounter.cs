using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MinesCounter: MonoBehaviour
{
    private Text textField;

    void Start()
    {
        textField = GetComponent<Text>();
    }
    public void UpdateAdjacentMines(int count)
    {
        textField.text = "Adjacent mines: " + count.ToString();
    }
}