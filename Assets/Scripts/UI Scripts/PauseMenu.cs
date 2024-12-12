using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    public GameObject capacityText;
    public GameObject pointsText;
    public GameObject shopText;
    public GameObject healthBar;
    public GameObject digestBar;
    public GameOverManager gameOverManager;
    public PlayerMovement playerMovement;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (gameOverManager != null && gameOverManager.IsGameOver)
                return;
            if (GameIsPaused)
            {
                Resume();
            }else
            {
                Pause();
            }
        }
    }

    void Resume ()
    {
        playerMovement.enabled = true;
        pauseMenuUI.SetActive(false);
        capacityText.SetActive(true); shopText.SetActive(true); healthBar.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause ()
    {
        playerMovement.enabled = false;
        pauseMenuUI.SetActive(true);
        capacityText.SetActive(false); shopText.SetActive(false); healthBar.SetActive(false); digestBar.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
