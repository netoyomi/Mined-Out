using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Chest: Cell
{
    public bool isOpened = false;
    public SpriteRenderer spriteRenderer;
    public Sprite openedSprite;
    public void Open()
    {
        isOpened = true;
        spriteRenderer.sprite = openedSprite;

        Player player = FindObjectOfType<Player>();
        player.TakeAward();
    }
}
