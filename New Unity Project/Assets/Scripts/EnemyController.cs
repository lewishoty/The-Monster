using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private int health = 100;
    public float speed;
    public GameObject bloodeffect;
    private Animator anim;

    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    [SerializeField] private float jumplength = 3f;
    [SerializeField] private float jumpheight = 7f;
    [SerializeField] private LayerMask ground;

    private Collider2D coll;
    private Rigidbody2D rb;

    private bool facingLeft = true;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();


    }



    // Update is called once per frame
    void Update()
    {
        checkhealth();
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < .1)
            {
                anim.SetBool("falling", true);
                anim.SetBool("jumping", false);
            }
        }

        if (coll.IsTouchingLayers(ground) && anim.GetBool("falling"))
        {
            anim.SetBool("falling", false);
        }

    }

    private void checkhealth()
    {
        if (health <= 0)
        {
            deathanimation();

        }
    }

    private void jump()
    {
        if (facingLeft)
        {
            //test if past leftcap
            if (transform.position.x > leftCap)
            {
                //make sprite face to the right 
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }

                //test if enemy touching ground, allow jump
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(-jumplength, jumpheight);
                    anim.SetBool("jumping", true);
                }
            }
            else
            {
                facingLeft = false;
            }
        }

        else
        {
            //test if past leftcap
            if (transform.position.x < rightCap)
            {
                //make sprite face to the right 
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }

                //test if enemy touching ground, allow jump
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(jumplength, jumpheight);
                    anim.SetBool("jumping", true);
                }
            }
            else
            {
                facingLeft = true;
            }
        }

    }

    public void TakeDamage(int damage)
    {

        health -= damage;
        Debug.Log("damage taken");
        Instantiate(bloodeffect, transform.position, Quaternion.identity);
    }

    public void deathanimation()
    {
        anim.SetTrigger("death");
    }
    public void death()
    {

        Destroy(this.gameObject);
    }

}
