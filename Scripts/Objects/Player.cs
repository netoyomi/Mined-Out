using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System;

public class Player : Cell
{
    [System.NonSerialized]

    private float normalStep = 0.5f;
    private float slowedStep = 1.5f;
    public float stepSize;
    private float timer = 0f;
    private bool isSlowed = false;
    public int Health = 6;
    public Animator animator;
    public bool reachedEnd = false;
    private bool isDying = false;
    public Rigidbody2D rb;
    public List<MineCell> adjacentMines;
    public int Wallet;
    public event Action PlayerReachedTheEndEvent;
    public event Action PlayerExplodedMine;
    public event Action PlayerDied;
    public event Action PlayerUpdatedWallet;
    public event Action PlayerTookAward;
    public int award;

    void Start()
    {
        stepSize = normalStep;
    }
    
     
    void Update()
    {
        if (isSlowed)
        {
            SlowingHandle();
        }
        if (Health == 0 && !isDying)
        {
            StartCoroutine(DelayedPlayerDied());
        }
    }
    
    private IEnumerator DelayedPlayerDied()
    {
        isDying = true;
        yield return new WaitForSeconds(0.65f);
        PlayerDied?.Invoke();
    }
    public void UpdateAnimator(Vector2 pos)
    {
        animator.SetFloat("Horizontal", pos.x);
        animator.SetFloat("Vertical", pos.y);
        animator.SetFloat("Speed", pos.magnitude);
    }
  
    private void SlowingHandle()
    {
        timer += Time.deltaTime;
        if (timer > 3.5f)
        {
            isSlowed = false;
            timer = 0f;
            stepSize = normalStep;
        }

    }
    public void OnMineEnter()
    {
        isSlowed = true;
        if ((Health - 1) == 0)
        {
            stepSize = 0f;
        }
        else
        {
            stepSize = slowedStep;
        }

        Health -= 1;
        PlayerExplodedMine?.Invoke();
    }

    public void CollectCoin()
    {
        Wallet += 1;
        PlayerUpdatedWallet?.Invoke();
    }
    public void TakeAward()
    {
        award = UnityEngine.Random.Range(1, 16);
        Wallet += award;
        PlayerTookAward?.Invoke();
        PlayerUpdatedWallet?.Invoke();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("ExitArea") && !reachedEnd)
        {

            PlayerReachedTheEndEvent?.Invoke();
        }
    }
}
