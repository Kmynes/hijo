using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControler : MonoBehaviour
{

    public enum States
    {
        Default = 0, //etat normal du jeu (peut se deplacer, faire des actions)
        Hide = 1, //etat où le perso est caché
        Dead = 2, //etat où le perso est dead (pour check une fin de game par ex ou autre ... chais pas)
        BlockedByGame = 3 //etat où le perso est bloqué par le jeu (cinématique par exemple ou dialogue ...etc)
    };

    Animator animator;
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    public States state; //etat du mec simplement avec une enum simple a piger (et pas besoin de check les layers a chaque fois)
    RaycastHit2D lastInteractHit2D;
    float speedMovement = 9; //vitesse du mec, peut etre edit si tu veux le faire courir, ralentir, ou simplement modifier pour coder
    bool lastPressedStateKeyE = false; //var utilisé pour savoir si entre 2 frames ont a relaché le bouton E, pour eviter le bug que tu as eu au tout début : 
                                       //le bug etait que frame x = tu appuyais sur E, ca allait dans la condition ou tu allais dans le casier
                                       //frame x + 1 = tu etais encore appuyé sur E (physiquement tu peux pas appuyer sur un bouton pendant 1 seule frame ahah) 
                                       //et ducoup frame x + 1 tu rerentrais dans la condition ou tu etais appuyé sur E  et ca te ressortais du casier
                                       //frame x + 2 ...etc jusqu'a ce que ton corps lache la touche. (si tu veux vérif, rajoute dans ton code d'avant un console.log)
    public bool objectivDone = false;
    public Transform rangeBack, rangeFront;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        state = States.Default;
        Physics2D.IgnoreLayerCollision(9, 11, true);
    }

    private void FixedUpdate()
    {
        if(this.state == States.Dead)
        {
            animator.Play("player idle");
            FindObjectOfType<ImageManager>().PrintImg("GameOver");
        }

        //DEPLACEMENTS
        if (Input.GetKey("left") && state == States.Default) //si le mec est dans son etat normal et que la touche est appuyée
        {
            MoveRigidBody(new Vector2(-1, 0)); //on lui dit d'aller a gauche , tu peux remplacer par Vector2.left
        }
        else if (Input.GetKey("right") && state == States.Default) //si le mec est dans son etat normal et que la touche est appuyée
        {
            MoveRigidBody(new Vector2(1, 0)); //on lui dit d'aller a droite , tu peux remplacer par Vector2.right
        }
        else
        {
            MoveRigidBody(Vector2.zero); //le mec fait rien, alors on met son deplacement a 0,0
        }

        //ACTIONS
        if ((Input.GetKey("e")))//si le mec est dans son etat normal ou qu'il est caché et que la touche est appuyée et qu'a la frame d'avant le bouton etait laché
        {
            if (!lastPressedStateKeyE)
            {
                lastInteractHit2D = CanInteractWithSomething();
                if (lastInteractHit2D)
                {
                    interactWithSomething(lastInteractHit2D.collider);
                }
            }

            lastPressedStateKeyE = true;
        }
        else
        {
            lastPressedStateKeyE = false;
        }

        //RESTART
        if (Input.GetKey("r") && state == States.Dead)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    private void MoveRigidBody(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            rigidBody.velocity = direction;
            animator.Play("player idle");
        }
        else
        {
            rigidBody.velocity = direction * new Vector2(speedMovement, 1); //on multiplie la direction par la vitesse de base du bonhomme (qui peut etre modifiée a tout moment)
            animator.Play("player walk");
            spriteRenderer.flipX = (direction.x < 0); //si < 0 alors true sinon false
        }
    }

    private RaycastHit2D CanInteractWithSomething()
    {
        return Physics2D.Linecast(rangeBack.position, rangeFront.position, 1 << LayerMask.NameToLayer("Interactibles"));
    }

    private void interactWithSomething(Collider2D ColliderHit)
    {
        //switch case par rapport a quel objet (selon le tag) on interact
        switch (ColliderHit.tag)
        {
            case "hiding": //si tu touche un "hiding"
                if (state == States.Default) //si on interact avec un hiding alors qu'on est en state normal alors on se cache :)
                {
                    state = States.Hide;
                    spriteRenderer.sortingOrder = 0;
                    this.gameObject.layer = LayerMask.NameToLayer("Hiding");
                    this.transform.position = new Vector3(ColliderHit.transform.position.x, this.transform.position.y, this.transform.position.z);
                }
                else if (state == States.Hide) //si on interact avec un hiding alors qu'on est en state hide alors on ressort
                {
                    state = States.Default;
                    spriteRenderer.sortingOrder = 3;
                    this.gameObject.layer = LayerMask.NameToLayer("Player");
                }
                break;
            case "ladder": //si tu touche un "ladder"
                FindObjectOfType<AudioManager>().PlaySound("climb ladder");
                if (this.transform.position.y < ColliderHit.bounds.center.y)
                {
                    this.transform.position = new Vector3(ColliderHit.bounds.max.x, ColliderHit.bounds.max.y + 1.5f, ColliderHit.bounds.max.z);
                }
                else
                {
                    this.transform.position = new Vector3(ColliderHit.bounds.min.x, ColliderHit.bounds.min.y + 1.5f, ColliderHit.bounds.min.z);
                }
                break;
            case "desktop": //si tu touche un "des bureaux avec documents a lire"
                if (ColliderHit.name == "Objective")
                {
                    this.objectivDone = true;
                }
                FindObjectOfType<ImageManager>().PrintOrPutAwayChild(ColliderHit.name);
                if (this.state != States.BlockedByGame)
                {
                    FindObjectOfType<AudioManager>().PlaySound("papper shuffle");
                    this.state = States.BlockedByGame;
                }
                else
                {
                    this.state = States.Default;
                }
                break;
            case "end of level": //si tu touche un "fin de niveau"
                if (this.objectivDone == true)
                {
                    MoveRigidBody(Vector2.zero);
                    animator.Play("player idle");
                    FindObjectOfType<ImageManager>().PrintImg("GameOver");
                    this.state = States.BlockedByGame;
                }
                break;
        }
    }
}