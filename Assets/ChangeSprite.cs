using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    public Sprite inrange;
    public Sprite outofrange;
    SpriteRenderer spriteRenderer;
    public Transform rangeBack, rangeFront;
    string nameofobject;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        nameofobject = this.gameObject.name;
    }

    private void FixedUpdate()
    {

        if (Physics2D.Linecast(rangeBack.position, rangeFront.position, 1 << LayerMask.NameToLayer("Interactibles")))
        {
            if(Physics2D.Linecast(rangeBack.position, rangeFront.position, 1 << LayerMask.NameToLayer("Interactibles")).collider.name == nameofobject)
            {
                spriteRenderer.sprite = inrange;
            }
            else
            {
                spriteRenderer.sprite = outofrange;
            }
        }
        else
        {
            spriteRenderer.sprite = outofrange;
        }
    }
}
