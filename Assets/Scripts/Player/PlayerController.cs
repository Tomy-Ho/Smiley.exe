using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // movespeed
    public float moveSpeed = 1f;
    // distance to colliding target and player, to detect collision
    public float collisionOffset = 0.05f;
    // only detect specific collisions
    public ContactFilter2D contactFilter;
    // add Player to attack to
    public Rigidbody2D rb;
    // add SwordAttack for flip direction
    public SwordAttack swordAttack;
    // Health
    private bool isDead = false;
    public float health = 10f;
    public float Health 
    {
    set 
        {
            // Store previous health value
            float previousHealth = health;
            
            // Set new health value
            health = value;
            
            // Only check for defeat if health has actually decreased
            if(health <= 0 && previousHealth > 0)
            {
                Defeated();
            }
        }
        get { return health; }
    }

    // movementInput
    Vector2 movementInput;
    // save collision in a List of results, if length of list is 0 -> no collisions
    List<RaycastHit2D> results = new List<RaycastHit2D>();
    // render our character
    SpriteRenderer spriteRenderer;
    Animator animator;
    public AudioSource audiosource;
    public AudioClip swordAttackSound;
    public AudioClip deathSound;


    bool canMove = true;

    public static event Action OnPlayerDied;
    
    void Start()
    {
        // define the target Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audiosource = GetComponent<AudioSource>();

    }

    // use fixed update to not recall every frame
    private void FixedUpdate()
    {
        if(canMove)
        {
                // input received
            if(movementInput != Vector2.zero)
            { 
                bool success = TryMove(movementInput);
                
                // if we detect collision, scan for input in x direction
                if(!success)
                {
                    TryMove(new Vector2(movementInput.x, 0));
                }

                // if we detect collision, scan for input in y direction
                if(!success)
                {
                    TryMove(new Vector2(0, movementInput.y));    
                }

                //animator.SetBool("IsMoving", success);
            
            }
            else
            {
                //animator.SetBool("IsMoving", false);
            }
            

            // flip character in movement direction, x<0 = left, x>0 = right
            if(movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if(movementInput.x > 0)
            {
                spriteRenderer.flipX = false;
            }
        }

    }

    // let player "slide" across edges instead of totally stopping when collision is detected
    private bool TryMove(Vector2 direction)
    {
        if(direction != Vector2.zero)
        {
        float distance = moveSpeed * Time.fixedDeltaTime + collisionOffset;

        // collision check
            // movementInput : is our Vector2 direction
            // contactFilter : what should count as collision
            // results       : output information about our collision
            // distance      : distance of our Ray
            // returns the number of results in the "results" List
            int count = rb.Cast(
                direction,
                contactFilter,
                results,
                distance);

            // movement script, only move if no collisions detected, returns True if no collision is detected in the direction of movement
            if(count == 0)
            {
                rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * direction);        
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;    
        }
    }

    // move function, move when certain input is pressed
    void OnMove(InputValue movementValue)
    {
        print("Moving");
        // movementValue = Button that is pressed in the InputSystem, save into movementInput to later determine where we should move
        movementInput = movementValue.Get<Vector2>();      
    }

    // attack logic
    void OnAttack()
    {
        print("Attacking");
        animator.SetTrigger("swordAttack");
        
    }

    public void SwordAttack()
    {
        if(!isDead)
        {
            LockMovement();
            if(spriteRenderer.flipX == true)
            {
                swordAttack.AttackLeft();            
            }
            else
            {
                swordAttack.AttackRight();   
            }  

            
            audiosource.PlayOneShot(swordAttackSound);
        }
    
        
    }
    
    public void PlayHitAnimation()
    {
        animator.SetTrigger("Hit");
    }
    public void EndSwordAttack()
    {
        UnlockMovement();
        swordAttack.StopAttack();
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }

    public void Defeated()
    {
        // Only proceed if we haven't already triggered death
        if (!isDead)
        {
            OnPlayerDied.Invoke();
            isDead = true;
            print("Dead");
            LockMovement();
            
            GetComponent<PlayerInput>().enabled = false;
            
            // Play death sound only once
            if (audiosource != null && deathSound != null)
            {
                audiosource.PlayOneShot(deathSound);
            }
            
            //StartCoroutine(LoadMainMenuAfterDelay(1.5f));
        }
    }

    private IEnumerator LoadMainMenuAfterDelay(float delay)
    {
    // Wait for the specified delay
    yield return new WaitForSeconds(delay);
    
    // Load the main menu scene
    // Note: Make sure your main menu scene is added to Build Settings
    UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    
    // If your main menu scene has a different name, replace "MainMenu" with that name
    // For example: SceneManager.LoadScene("TitleScreen");
    }
}
