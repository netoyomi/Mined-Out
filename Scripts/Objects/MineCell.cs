using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MineCell : Cell
{
    public bool revealed;
    public Sprite unrevealedSprite;
    public Sprite revealedSprite;
    private SpriteRenderer spriteRenderer;
    public GameObject explosionPrefab;

    private void Update()
    {
        if (revealed)
        {
            spriteRenderer.sprite = revealedSprite;
            gameObject.tag = "RevealedMine";
            
        }
        else
        {
            spriteRenderer.sprite = unrevealedSprite;
        }
    }

    private void Start()
    {
        revealed = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
 
    }

    public void Reveal()
    {
        revealed = true;

        GameObject explosion = Instantiate(explosionPrefab, new Vector3(position.x, position.y + 3), Quaternion.identity);
        Destroy(explosion, explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }

    public void Open()
    {
        revealed = true;

    }
}
