using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour
{
    public bool playerinrange = false;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player") && FindObjectOfType<PlayerControler>().objectivDone == false)
        {
            playerinrange = true;
            this.spriteRenderer.enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player") && FindObjectOfType<PlayerControler>().objectivDone == false)
        {
            playerinrange = false;
            this.spriteRenderer.enabled = false;
        }
    }
}