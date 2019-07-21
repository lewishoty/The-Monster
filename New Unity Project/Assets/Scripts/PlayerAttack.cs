using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timeBtwnAtk;
    public float startTimeBtwnAtk = 0;
    public Transform attackPos;
    public float attackRange;
    public LayerMask whatIsEnemies;
    public int damage;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timeBtwnAtk <= 0)
        {
            if (Input.GetKey(KeyCode.K))
            {

                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<EnemyController>().TakeDamage(damage);
                }

                timeBtwnAtk = startTimeBtwnAtk;
            }
        }
        else
        {
            timeBtwnAtk -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);

    }
}
