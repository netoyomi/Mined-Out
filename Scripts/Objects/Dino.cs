using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Dino: Enemy
{
    private Player player;
    public Animator animator;
    public Rigidbody2D rb;
 
    public event Action PlayerCaughtByDinoEvent;
    public event Action MoveDino;
    
    
    private float timeSinceLastMove = 0f;
    private float moveInterval = 3f;
    void CatchPlayer()
    {
      
        playerCatched = true;
        PlayerCaughtByDinoEvent?.Invoke();
    }
    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    void FixedUpdate()
    {
        if(!playerCatched)
        {
            Move();
        }
        
    }
    private void Move()
    {
        timeSinceLastMove += Time.deltaTime;

        if (timeSinceLastMove >= moveInterval)
        {
            MoveDino?.Invoke();
            timeSinceLastMove = 0f;
        }
    }
 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CatchPlayer();
        }
    }
    public void UpdateAnimator(Vector2 targetPosition)
    {
        animator.SetFloat("Horizontal", targetPosition.x);
        animator.SetFloat("Vertical", targetPosition.y);
        animator.SetFloat("Speed", targetPosition.magnitude);
    }
   
}
