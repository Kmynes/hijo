using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{

    Animator animator;
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey("left"))
        {
            rb2d.velocity = new Vector2(-3, 0);
            animator.Play("player walk");
            spriteRenderer.flipX = true;
        }
        else if (Input.GetKey("right"))
        {
            rb2d.velocity = new Vector2(3, 0);
            animator.Play("player walk");
            spriteRenderer.flipX = false;
        }
        else
        {
            rb2d.velocity = new Vector2(0, 0);
            animator.Play("player idle");
        }
    }
}
