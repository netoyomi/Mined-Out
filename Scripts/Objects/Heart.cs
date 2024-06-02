using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Heart: MonoBehaviour
{
    public Sprite wholeHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer.sprite = wholeHeart;
    }
}
