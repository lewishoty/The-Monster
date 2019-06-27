using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private int health = 100;
    public float speed;
    public GameObject bloodeffect;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        
    }



    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            deathanimation();
          
        }
    }

    public void TakeDamage(int damage) {
       
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
