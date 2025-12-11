using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public class MainMenu : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadScene("Plant");
    }

    public void QuitGame()
    {
        Application.Quit();
        UnityEngine.Debug.Log("Saliendo del juego...");
    }
}
