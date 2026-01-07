using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    public GameObject GameOver;
    void Start()
    {
        PlayerController.OnPlayerDied += GameOverScreen;
        GameOver.SetActive(false);
    }

    void GameOverScreen(){
        GameOver.SetActive(true);
    }

    public void ResetGame(){
        GameOver.SetActive(false);
        LoadLevel();
    }

    void LoadLevel(){
        SceneManager.LoadScene(2);
    }

    public void ReturnMainMenu(){
        SceneManager.LoadScene(0);
    }

    void OnDestroy()
    {
        PlayerController.OnPlayerDied -= GameOverScreen;
    }
}
