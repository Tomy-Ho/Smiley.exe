using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(gameObject.CompareTag("Level1"))
            {
                SceneManager.LoadScene(3);
            }
            else if(gameObject.CompareTag("Level2")){
                SceneManager.LoadScene(4);
            }
            else if(gameObject.CompareTag("Level3")){
                SceneManager.LoadScene(5);
            }
        }
    }
}