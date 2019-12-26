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
    bool seesPlayer = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        Debug.DrawLine(startSight.position, endSight.position, Color.green);
        while (seesPlayer == false)
        {
            Debug.DrawLine(startSight.position, endSight.position, Color.green);
        }

        while (seesPlayer == true)
        {
            Debug.DrawLine(startSight.position, endSight.position, Color.magenta);
        }
    }
}
