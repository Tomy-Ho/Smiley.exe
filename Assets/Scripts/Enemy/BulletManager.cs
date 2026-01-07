using Unity.VisualScripting;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] public float BulletDamage = 1f;
    [SerializeField] private AudioClip hitSound;
    private AudioSource audioSource;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            
            // Apply damage
            PlayerController playerController = col.gameObject.GetComponent<PlayerController>();
            
            if (audioSource != null && hitSound != null && playerController.Health > 0)
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
            }

            playerController.Health -= BulletDamage;

            playerController.PlayHitAnimation();
            
            // Destroy the bullet
            Destroy(gameObject);
        }
    }
}