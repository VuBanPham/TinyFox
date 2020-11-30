using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    [SerializeField] private float jumpLength = 5f;
    [SerializeField] private float jumpHeight = 6f;
    [SerializeField] private LayerMask ground;
    private Collider2D col;
    private Rigidbody2D rb;
    private Animator anim;

    private bool facingLeft = true;

   
    private void Start()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //transition from jump to fall
        if (anim.GetBool("Jumping"))
        {
            if (rb.velocity.y < .1f)
            {
                anim.SetBool("Falling", true);
                anim.SetBool("Jumping", false);
            }
        }
        //transition from fall to idle
        if (col.IsTouchingLayers(ground) && anim.GetBool("Falling"))
        {
            anim.SetBool("Falling", false);           
        }
    }

    private void Move()
    {
        if (facingLeft)
        {
            //enemy not exceed the left Cap
            if (transform.position.x > leftCap)
            {
                //Enemy facing the right direction otherwise reverse
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                //check the enemy on the ground, if so keep jumping
                if(col.IsTouchingLayers(ground))
                {
                    //Jump
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = false;
            }
        }

        else
        {
            //enemy not exceed the left Cap
            if (transform.position.x < rightCap)
            {
                //Enemy facing the right direction otherwise reverse
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                //check the enemy on the ground, if so keep jumping
                if (col.IsTouchingLayers(ground))
                {
                    //Jump
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = true;
            }
        }
    }


}
