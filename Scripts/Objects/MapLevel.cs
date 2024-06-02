using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MapLevel: MonoBehaviour
{
    public GameObject[] mapStars;
    public int levelIndex;

    public void SetStars(int count)
    {
        for (int i = 0; i < mapStars.Length; i++)
        {
            mapStars[i].SetActive(i < count);
        }
    }
}
