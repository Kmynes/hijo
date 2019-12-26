using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;
    bool isFacingRight = true;
    public Transform startSight, endSight;
    public bool seesPlayer = false;
    public float stepsBeforeTurn;
    float stepsRemaining;
    public float waitTime;
    float waitTimeRemaining;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        stepsRemaining = stepsBeforeTurn;
        waitTimeRemaining = waitTime;
    }

    private void FixedUpdate()
    {
        Raycasting();
        Behaviour();
    }

    void Raycasting()
    {
        Debug.DrawLine(startSight.position, endSight.position, Color.green);
        seesPlayer = Physics2D.Linecast(startSight.position, endSight.position, 1 << LayerMask.NameToLayer("Player"));
        if (seesPlayer == true)
        {
            Debug.DrawLine(startSight.position, endSight.position, Color.magenta);
            seesPlayer = Physics2D.Linecast(startSight.position, endSight.position, 1 << LayerMask.NameToLayer("Player"));
        }
    }

    void Behaviour()
    {
        if (seesPlayer == true)
        {
            rb2d.velocity = new Vector2(0, 0);
            animator.Play("guard aim");
        }
        else
        {
            if(stepsRemaining > 0)
            {
                animator.Play("guard walk");
                if (isFacingRight == true)
                {
                    rb2d.velocity = new Vector2(2, 0);
                }
                else
                {
                    rb2d.velocity = new Vector2(-2, 0);
                }
                stepsRemaining--;
            }
            else
            {
                rb2d.velocity = new Vector2(0, 0);
                animator.Play("guard idle");
                if(waitTimeRemaining <= 0)
                {
                    stepsRemaining = stepsBeforeTurn;
                    waitTimeRemaining = waitTime;
                    if(isFacingRight == true)
                    {
                        spriteRenderer.flipX = true;
                        isFacingRight = false;
                        endSight.position += new Vector3(-8, 0, 0);
                    }
                    else
                    {
                        spriteRenderer.flipX = false;
                        isFacingRight = true;
                        endSight.position += new Vector3(8, 0, 0);
                    }
                }
                else
                {
                    waitTimeRemaining -= Time.deltaTime;
                }
            }
        }
    }
}