using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Coin: Cell
{
    public bool IsCollected = false;
    public Sprite collectedSprite;
    private Collider2D coinCollider;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
 
    public void Collect()
    {
        coinCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        spriteRenderer.sprite = collectedSprite;
        animator.enabled = false;
        coinCollider.enabled = false;
        IsCollected = true;
     
    }
}