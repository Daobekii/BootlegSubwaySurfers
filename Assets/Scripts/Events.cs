using UnityEngine;
using UnityEngine.SceneManagement;

public class Events : MonoBehaviour
{
    public void ReplayGame() {
        SceneManager.LoadScene("MainGame");
    }

    public void QuitGame() {
        Application.Quit();
    }
}
