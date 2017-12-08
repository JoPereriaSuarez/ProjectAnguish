using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCharacter : MonoBehaviour
{
    SpriteRenderer rend;
    Animator anim;
    [SerializeField] string walkAniParam    = "IsWalking";
    public Rigidbody2D rig2D { get; private set; }

    public float speed = 5F;
    public bool canMove { get; private set; }
    public bool canInteract { get; private set; }

    Vector2 vel;

    float lastDirection = 1.0F;

    private void Awake()
    {
        rend = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rig2D = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 direction)
    {
        vel = direction * (speed / 2);
        rig2D.velocity = vel;
        anim.SetFloat(walkAniParam, Mathf.Abs(direction.x + direction.y));

        if(direction.x == 0.0F)
        {
            return;
        }
        float signDirection = Mathf.Sign(direction.x);
        if(lastDirection != signDirection)
        {
            rend.flipX = (signDirection < 0.0F) ? true : false;
            //anim.SetBool(turningAniParam, true);
            lastDirection = signDirection;
        }
    }

    public void StopMoving()
    {
        anim.speed = 0;
        vel = rig2D.velocity;
        rig2D.velocity = Vector2.zero;
    }
    public void NormalizeAnimator()
    {
        anim.speed = 1;
    }
}
