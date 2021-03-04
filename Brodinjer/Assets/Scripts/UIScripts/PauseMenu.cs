using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public BoolData gameIsPaused;
    private bool optionsMenuIsActive;
    public GameObject pauseMenuUI, optionsMenuUI;
    private bool dead;

    void Start()
    {
        optionsMenuIsActive = false;
        gameIsPaused.value = false;
        dead = false;
    }
    
    void Update()
    {
        if (!dead && Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused.value)
            {
                if (optionsMenuIsActive)
                {
                    optionsMenuUI.SetActive(false);
                    pauseMenuUI.SetActive(true);
                }
                else
                {
                    Resume();    
                }
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        if (!dead)
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1.0f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            gameIsPaused.value = false;
        }
    }

    public void Pause()
    {
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameIsPaused.value = true;
    }

    public void OptionsMenu()
    {
        optionsMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        optionsMenuIsActive = true;
    }

    public void QuitGame()
    {
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameIsPaused.value = false;
        SceneManager.LoadScene(0);
    }

    public void Dead()
    {
        dead = true;
        Pause();
    }
}
