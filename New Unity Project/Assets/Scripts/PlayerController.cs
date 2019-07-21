using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    public GameObject bloodeffect;


    private enum State { idle, running, jump, fall, hurt, attacking }
    private State state = State.idle;
    
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpforce = 12f;
    private int jumpcount = 0;
    

    private float timeBtwnDash;
    public float startTimeBtwnDash = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (state != State.hurt) { 
        InputManager();
    }
        velocitystate();
        anim.SetInteger("state", (int)state);

        
    }
    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.tag == "collectable") {
            Destroy(collision.gameObject);
            //currenthealth += 15; or smth later
        }

    }

        private void OnCollisionEnter2D(Collision2D frog) {
            if (frog.gameObject.tag == "enemy") {

                state = State.hurt;
                if (frog.gameObject.transform.position.x > transform.position.x) {
                    rb.velocity = new Vector2(-15f, rb.velocity.y);
                }
                else {
                    rb.velocity = new Vector2(15f, rb.velocity.y);
                }
            }

        }

    private void InputManager()
    {
        //unity default input for horizontal movement
        float hDirection = Input.GetAxisRaw("Horizontal");

        

        if (hDirection < 0)
        {
            //speed
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            //direction player faces
            transform.localScale = new Vector2(-1, 1);
            //make running animation true

        }
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);

        }
        else { }
        if (coll.IsTouchingLayers(ground))
        {
            jumpcount = 0;
        }
        if (Input.GetButtonDown("Jump") && jumpcount <= 1)
        {
            
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            state = State.jump;
            jumpcount ++;
        }
        //attacking
        if (Input.GetKey(KeyCode.K)) {
            state = State.attacking;
        }

        //dashing
        if (timeBtwnDash <= 0)
        {
            if (Input.GetKey(KeyCode.J))
            {
                Instantiate(bloodeffect, transform.position, Quaternion.identity);
                if (transform.localScale.x == 1)
                {
                    transform.localPosition = new Vector2(transform.localPosition.x + 2, transform.localPosition.y);
                    
                }
                else
                {
                    transform.localPosition = new Vector2(transform.localPosition.x - 2, transform.localPosition.y);
                   
                }


                timeBtwnDash = startTimeBtwnDash;
            }
        }
        else
        {
            timeBtwnDash -= Time.deltaTime;
        }

    }

    private void velocitystate() {
        if (state == State.jump)
        {
            if (rb.velocity.y < 0.1f)
            {
                state = State.fall;
            }
        }
        else if (state == State.fall)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }

        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }

        else if (state == State.attacking)
        {
            if (Input.GetKeyUp(KeyCode.K)) { 
            state = State.idle;
        }
        }

        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            //running
            state = State.running;
        }
        else { state = State.idle; }
        
    }




}




