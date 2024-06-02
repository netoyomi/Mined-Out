using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LifeHandler: MonoBehaviour
{
    public Heart[] hearts;
    public void SetHearts()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            hearts[i].spriteRenderer.sprite = hearts[i].wholeHeart;
        }
    }
    public void DecreaseHearts(int health)
    {
        int wholeHearts = health / 2;
        int halfHearts = health % 2;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < wholeHearts)
            {
                hearts[i].spriteRenderer.sprite = hearts[i].wholeHeart;
            }
            else if (i == wholeHearts && halfHearts == 1)
            {
                hearts[i].spriteRenderer.sprite = hearts[i].halfHeart;
            }
            else
            {
                hearts[i].spriteRenderer.sprite = hearts[i].emptyHeart;
            }
        }
    }
}