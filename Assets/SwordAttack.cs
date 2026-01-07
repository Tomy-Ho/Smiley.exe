using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public float damage = 3f;
    Vector2 rightAttackOffset;
    public Collider2D swordCollider;

    private void Start()
    {
        rightAttackOffset = transform.position;
    }

    
    public void AttackRight()
    {
        print("Attack Right");
        swordCollider.enabled = true;
        transform.localPosition = rightAttackOffset;
    }

    public void AttackLeft()
    {
        print("Attack Left");
        swordCollider.enabled = true;
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
    }

    public void StopAttack()
    {
        swordCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();

            if(enemy != null)
            {
                enemy.Health -= damage;
            }     
        }       
    }
}
