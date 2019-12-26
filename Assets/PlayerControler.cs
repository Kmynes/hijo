using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{

    Animator animator;
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;
    public Transform rangeBack, rangeFront;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey("left") && this.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            rb2d.velocity = new Vector2(-4, 0);
            animator.Play("player walk");
            spriteRenderer.flipX = true;
        }
        else if (Input.GetKey("right") && this.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            rb2d.velocity = new Vector2(4, 0);
            animator.Play("player walk");
            spriteRenderer.flipX = false;
        }
        else if (Input.GetKey("e") && Physics2D.Linecast(rangeBack.position, rangeFront.position, 1 << LayerMask.NameToLayer("Interractibles")))
        {
            if(this.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                spriteRenderer.sortingOrder = 0;
                this.gameObject.layer = LayerMask.NameToLayer("Hiding");
            }
            else
            {
                spriteRenderer.sortingOrder = 3;
                this.gameObject.layer = LayerMask.NameToLayer("Player");
            }
        }
        else
        {
            rb2d.velocity = new Vector2(0, 0);
            animator.Play("player idle");
        }
    }
}