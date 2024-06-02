using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Star: Cell
{
    public bool IsCollected = false;
    public Sprite collectedSprite;
    private Collider2D starCollider;
    private SpriteRenderer spriteRenderer;
    public void Collect()
    {
        Debug.Log("Collected");
        IsCollected = true;
        starCollider = GetComponent<Collider2D>();
        starCollider.enabled = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = collectedSprite;
    }
}