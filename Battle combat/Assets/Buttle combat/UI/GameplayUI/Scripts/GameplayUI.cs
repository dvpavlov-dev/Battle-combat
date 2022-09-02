using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayUI : MonoBehaviour
{
    public GameObject RightPanel;

    public void OnClick_ReturnToMenu()
    {
        SceneManager.LoadScene("Start menu");
    }

    public void OnClick_Exit()
    {
#if !UNITY_EDITOR
        Application.Quit();
#else
        EditorApplication.ExitPlaymode();
#endif
    }

    public void OnClick_OpenMenu()
    {
        RightPanel.SetActive(!RightPanel.activeSelf);
    }
}
