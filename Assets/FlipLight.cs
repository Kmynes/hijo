using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipLight : MonoBehaviour
{
    public EnemyScript enemy;

    private void FixedUpdate()
    {
        if(enemy.fliplight == true)
        {
            Flip();
            enemy.fliplight = false;
        }
    }

    void Flip()
    {
        this.transform.rotation = Quaternion.Inverse(this.transform.rotation);
    }
}
