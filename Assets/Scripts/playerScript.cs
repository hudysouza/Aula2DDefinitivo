using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    private Animator PlayerAnimator;
    private Rigidbody2D PLayerRB;
    public Transform GroundCheck;
    public float Speed;
    public float JumpForce;
    public bool Grounded;
    public int IdAnimation;
    public bool LookLeft;
    public bool Attacking;
    private float Horizontal;
    private float Vertical;
    public Collider2D Standing;
    public Collider2D Crouching;


    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        PLayerRB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Grounded = Physics2D.OverlapCircle(GroundCheck.position, 0.02f);
        PLayerRB.velocity = new Vector2(Horizontal * Speed, PLayerRB.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");

        if (Horizontal > 0 && LookLeft && !Attacking)
        {
            Flip();
        }
        else if (Horizontal < 0 && !LookLeft && !Attacking)
        {
            Flip();
        }

        if (Vertical < 0)
        {
            IdAnimation = 2;
            if (Grounded)
                Horizontal = 0;
        }
        else if (Horizontal != 0)
        {
            IdAnimation = 1;
        }
        else
        {
            IdAnimation = 0;
        }

        if (Input.GetButtonDown("Fire1") && Vertical >= 0 && !Attacking)
        {
            PlayerAnimator.SetTrigger("attack");
        }

        if (Input.GetButtonDown("Jump") && Grounded && !Attacking)
        {
            PLayerRB.AddForce(new Vector2(0, JumpForce));
        }

        if (Attacking && Grounded)
        {
            Horizontal = 0;
        }

        if (Vertical < 0 && Grounded)
        {
            Crouching.enabled = true;
            Standing.enabled = false;
        }
        else if (Vertical >= 0 && Grounded)
        {
            Crouching.enabled = false;
            Standing.enabled = true;
        }
        else if (Vertical != 0 && !Grounded)
        {
            Crouching.enabled = false;
            Standing.enabled = true;
        }

        PlayerAnimator.SetBool("grounded", Grounded);
        PlayerAnimator.SetInteger("idAnimation", IdAnimation);
        PlayerAnimator.SetFloat("speedY", PLayerRB.velocity.y);
    }

    private void Flip()
    {
        LookLeft = !LookLeft;
        var localSacleX = transform.localScale.x;
        localSacleX *= -1;
        transform.localScale = new Vector3(localSacleX, transform.localScale.y, transform.localScale.z);
    }

    public void Attack(int attacking)
    {
        if (attacking == 0)
        {
            Attacking = false;
            return;
        }

        Attacking = true;
    }
}
