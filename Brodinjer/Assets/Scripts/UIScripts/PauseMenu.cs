using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public BoolData gameIsPaused;
    public bool optionsMenuIsActive, controlsMenuUIIsActive;
    public GameObject pauseMenuUI, optionsMenuUI, controlsMenuUI;
    private bool dead;

    void Start()
    {
        optionsMenuIsActive = false;
        controlsMenuUIIsActive = false;
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
                    //Debug.Log("Reset Options");
                    gameIsPaused.value = true;
                    optionsMenuUI.SetActive(false);
                    optionsMenuIsActive = false;
                    pauseMenuUI.SetActive(true);
                }
                else if (controlsMenuUIIsActive)
                {
                    gameIsPaused.value = true;
                    controlsMenuUI.SetActive(false);
                    controlsMenuUIIsActive = false;
                    pauseMenuUI.SetActive(true);
                }
                else
                {
                    //Debug.Log("Resume");
                    Resume();    
                }
            }
            else
            {
                //Debug.Log("Pause");
                gameIsPaused.value = true;
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
        controlsMenuUI.SetActive(false);
        optionsMenuIsActive = false;
        controlsMenuUIIsActive = false;
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

    public void ControlsMenu()
    {
        controlsMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        controlsMenuUIIsActive = true;
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
