using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Ссылки")]
    public GameObject tutorialPanel;
    
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene"); // Замените на имя вашей игровой сцены
    }
    
    public void ToggleTutorial()
    {
        tutorialPanel.SetActive(!tutorialPanel.activeSelf);
    }
    
    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}