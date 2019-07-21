using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    public GameObject bloodeffect;


    private enum State { idle, running, jump, fall, hurt, attacking }
    private State state = State.idle;

    //jump variables
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed;
    [SerializeField] private float jumpforce;
    [SerializeField] private float jumptime;
    private float jumptimecounter;
    private int jumpcount = 0;

    //dash variables
    [SerializeField] private float timeBtwnDash;
    [SerializeField] private float startTimeBtwnDash;

    //slider health
    public Slider healthbar;
    [SerializeField] private float maxhealth  = 100f;
    private float health;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        jumptimecounter = jumptime;
        health = maxhealth;
        healthbar.maxValue = maxhealth;
    }

    private void Update()
    {
        if (state != State.hurt)
        {
            InputManager();
        }
        velocitystate();
        anim.SetInteger("state", (int)state);
        healthbar.value = health;

    }

    //collectible
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "collectable")
        {
            Destroy(collision.gameObject);
            if(health >= maxhealth - 10)
            {
                health = 100;
            }
            else
            {
                health += 10;
            }
        }
    }

    //touch frog
    private void OnCollisionEnter2D(Collision2D frog)
    {
        if (frog.gameObject.tag == "enemy")
        {
            health -= 10;
            state = State.hurt;
            if (frog.gameObject.transform.position.x > transform.position.x)
            {
                rb.velocity = new Vector2(-15f, rb.velocity.y);
            }
            else
            {
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
        else {}

        //jump
        if (coll.IsTouchingLayers(ground))
        {
            jumpcount = 0;
        }

        if (Input.GetButton("Jump"))
        {
            state = State.jump;

            if (jumptimecounter > 0 && jumpcount <= 1)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpforce);
                jumptimecounter -= Time.deltaTime;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            jumpcount++;
            jumptimecounter = jumptime;
        }

        //attacking
        if (Input.GetButton("Attack"))
        {
            state = State.attacking;
        }

        //dashing
        if (timeBtwnDash <= 0)
        {
            if (Input.GetButton("Dash"))
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

    private void velocitystate()
    {
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
            if (Input.GetButtonUp("Attack"))
            {
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




