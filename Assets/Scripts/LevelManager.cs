using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMAnager : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")){
            SceneManager.LoadScene(2);
        }
    }
}
