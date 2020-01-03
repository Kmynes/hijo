using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    public Sprite inrange;
    public Sprite outofrange;
    SpriteRenderer spriteRenderer;
    public Transform rangeBack, rangeFront;
    string nameofabject;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        nameofabject = this.gameObject.name;
    }

    private void FixedUpdate()
    {

        if (Physics2D.Linecast(rangeBack.position, rangeFront.position, 1 << LayerMask.NameToLayer("Interractibles")))
        {
            if(Physics2D.Linecast(rangeBack.position, rangeFront.position, 1 << LayerMask.NameToLayer("Interractibles")).collider.name == nameofabject)
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
