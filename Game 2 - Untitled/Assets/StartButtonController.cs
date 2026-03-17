using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void OnStartClick()
    {
        if (SceneManager.sceneCountInBuildSettings > 1)
        {
            SceneManager.LoadScene("Test"); // <-- fixed name
        }
        else
        {
            Debug.LogError("StartMenu: Test scene not in Build Profiles! Go to File → Build Profiles and add it.");
        }
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}