using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour
{
    public bool playerinrange = false;

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player") && FindObjectOfType<PlayerControler>().objectivDone == false)
        {
            playerinrange = true;
            FindObjectOfType<ImageManager>().PrintOrPutAwayChild(this.name);
        }
    }

    void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player") && FindObjectOfType<PlayerControler>().objectivDone == false)
        {
            playerinrange = false;
            FindObjectOfType<ImageManager>().PrintOrPutAwayChild(this.name);
        }
    }
}