using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public PlayerPickup playerPickup;
    public GameObject gameOverCanvas;
    public GameObject regularUICanvas;
    public PlayerMovement playerMovement;

    private bool isGameOver = false;

    public AudioSource musicAudioSource; // AudioSource for background music

    public bool IsGameOver => isGameOver;

    private void Start()
    {
        if (musicAudioSource != null)
        {
            musicAudioSource.loop = true; // Ensure the music loops
            musicAudioSource.Play();      // Start playing the music
        }
    }

    public void GameOver()
    {
        // Stop the game
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        playerMovement.enabled = false;
        gameOverCanvas.SetActive(true);
        regularUICanvas.SetActive(false);
        isGameOver = true;

        // Stop the music
        if (musicAudioSource != null)
        {
            musicAudioSource.Stop();
        }
    }

    public void RestartGame()
    {
        // Restart the game by reloading the scene
        isGameOver = true;
        Time.timeScale = 1f; // Ensure time is resumed
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
