using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class StartMenuUI : MonoBehaviour
{
    public void OnClick_StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void OnClick_Exit()
    {
#if !UNITY_EDITOR
        Application.Quit();
#else
        EditorApplication.ExitPlaymode();
#endif
    }
}
