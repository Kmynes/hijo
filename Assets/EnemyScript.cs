using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;
    bool isFacingRight = true;
    public Transform startSight;
    //float speedMovement = 2.5f;
    public bool seesPlayer = false;
    public float stepsBeforeTurn;
    float stepsRemaining;
    public float waitTime;
    float waitTimeRemaining;
    public float range = 4;

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
        Vector3 endSight = startSight.position + new Vector3(((isFacingRight) ? range : range * -1), 0, 0);
        seesPlayer = Physics2D.Linecast(startSight.position, endSight, 1 << LayerMask.NameToLayer("Player"));
        Debug.DrawLine(startSight.position, endSight, (seesPlayer) ? Color.magenta : Color.green);
    }

    void Behaviour()
    {
        if (seesPlayer == true)
        {
            rb2d.velocity = new Vector2(0, 0);
            animator.Play("guard aim");
            if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                FindObjectOfType<AudioManager>().PlaySound("gunshot");
                FindObjectOfType<PlayerControler>().state = PlayerControler.States.Dead;
            }
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
                    }
                    else
                    {
                        spriteRenderer.flipX = false;
                        isFacingRight = true;
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