using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;
    bool isFacingRight = true;
    public Transform startSight, leftpatrol, rightpatrol;
    public float speedMovement = 2.5f;
    public bool seesPlayer = false;
    public float waitTime;
    float waitTimeRemaining;
    public float range = 4;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            if(FindObjectOfType<PlayerControler>().state == PlayerControler.States.BlockedByGame || FindObjectOfType<PlayerControler>().state == PlayerControler.States.Dead)
            {
                rb2d.velocity = new Vector2(0, 0);
            }
            else if(this.transform.position.x > leftpatrol.position.x && this.transform.position.x < rightpatrol.position.x)
            {
                animator.Play("guard walk");
                if (isFacingRight == true)
                {
                    rb2d.velocity = new Vector2(speedMovement, 0);
                }
                else
                {
                    rb2d.velocity = new Vector2(-speedMovement, 0);
                }
            }
            else
            {
                rb2d.velocity = new Vector2(0, 0);

                if (this.transform.position.x < leftpatrol.position.x)
                {
                    this.transform.position = new Vector3(leftpatrol.position.x, this.transform.position.y, this.transform.position.z);
                    animator.Play("guard idle");
                }
                else if (this.transform.position.x > rightpatrol.position.x)
                {
                    this.transform.position = new Vector3(rightpatrol.position.x, this.transform.position.y, this.transform.position.z);
                    animator.Play("guard idle");
                }

                if (waitTimeRemaining <= 0)
                {
                    waitTimeRemaining = waitTime;
                    if(isFacingRight == true)
                    {
                        spriteRenderer.flipX = true;
                        this.transform.position = new Vector3(rightpatrol.position.x - 0.000001f, this.transform.position.y, this.transform.position.z);
                        isFacingRight = false;
                    }
                    else
                    {
                        this.transform.position = new Vector3(leftpatrol.position.x + 0.000001f, this.transform.position.y, this.transform.position.z);
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