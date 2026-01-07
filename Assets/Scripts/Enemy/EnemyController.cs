using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Collections;
using UnityEngine;


public class EnemyController : MonoBehaviour
{   
    [SerializeField] public float Attackdelay = 1f;
    [SerializeField] public float EnemyDetectRange = 1.2f;
    [SerializeField] public float EnemySpeed = 0.5f;
    public float Health 
    {
        set 
        {
            health = value;
            if(health <= 0)
            {
                Defeated();
            }
        }
        get
        {
            return health;
        }
    }
    public float health = 1;
    bool AttackCDActive = true;
    float AttackCoolDown;
    float MoveRangePly = 0.2f;
    Shooting Attack;
    GameObject Player;
    public AudioSource audiosource;
    public AudioClip hitSound;
    public AudioClip deathSound;
    void Start(){
        audiosource = GetComponent<AudioSource>();
        Attack = GetComponent<Shooting>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update(){
        float dist = Vector2.Distance(transform.localPosition, Player.transform.localPosition);

        if(dist < EnemyDetectRange){
            if(dist >= MoveRangePly){
            transform.position = Vector2.MoveTowards(this.transform.position, Player.transform.position, EnemySpeed * Time.deltaTime);  //Follows the Player Position
            }
            if(AttackCD() == false){
                Attack.ShootTarget();
                AttackCoolDown = Attackdelay;
                AttackCDActive = true;
            }
        }
    }   

    bool AttackCD(){
        if(AttackCDActive && AttackCoolDown >= 0.0f){
            AttackCoolDown -= Time.deltaTime;
        }
        if(AttackCoolDown <= 0.0f){
            AttackCDActive = false;
        }
        return AttackCDActive;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        audiosource.PlayOneShot(hitSound);
    }

    public void Defeated()
    {
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
    }

   

}
