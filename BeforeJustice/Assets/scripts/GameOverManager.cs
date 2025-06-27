using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [Header("Ссылки")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI killsText;
    
    public static GameOverManager Instance;
    
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
    public void ShowGameOver(int kills)
    {
        if (gameOverPanel == null)
        {
            Debug.LogError("GameOverPanel not assigned!");
            return;
        }
        
        killsText.text = $"Убито монстров: {kills}";
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Пауза игры
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        
        // Сбрасываем статистику
        GameStats.Instance.ResetStats();
        
        // Принудительное обновление перед загрузкой
        GameStats.Instance.ForceUIUpdate();
        
        // Перезагружаем сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        
        // Сбрасываем статистику
        GameStats.Instance.ResetStats();
        
        // Принудительное обновление перед загрузкой
        GameStats.Instance.ForceUIUpdate();
        
        SceneManager.LoadScene("MenuScene");
    }
}