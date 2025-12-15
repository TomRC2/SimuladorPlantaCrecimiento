using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject UI;
    private bool isPaused = false;
    private bool isUI = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        isUI = !isUI;
        pauseMenu.SetActive(isPaused);
        UI.SetActive(isUI);
        Time.timeScale = isPaused ? 0f : 1f;
    }
    public void RestartScene()
    {
        SceneManager.LoadScene("Plant");
    }
    public void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        UI.SetActive(true);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

