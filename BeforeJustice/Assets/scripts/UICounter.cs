using UnityEngine;
using TMPro;

public class UICounter : MonoBehaviour
{
    [Header("Ссылка на текст")]
    public TextMeshProUGUI enemiesKilledText;
    
    private void OnEnable()
    {
        // Регистрируемся при каждом включении объекта
        if (GameStats.Instance != null)
        {
            GameStats.Instance.OnEnemyKilled += UpdateCounter;
        }
        UpdateCounterImmediately();
    }
    
    private void OnDisable()
    {
        // Отписываемся при выключении
        if (GameStats.Instance != null)
        {
            GameStats.Instance.OnEnemyKilled -= UpdateCounter;
        }
    }
    
    public void UpdateCounterImmediately()
    {
        if (this == null) return; // Защита от уничтоженного объекта
        
        if (GameStats.Instance != null && enemiesKilledText != null)
        {
            enemiesKilledText.text = $"Врагов убито: {GameStats.Instance.enemiesKilled}";
        }
        else
        {
            // Пробуем найти текстовое поле, если ссылка потеряна
            if (enemiesKilledText == null)
            {
                enemiesKilledText = GetComponentInChildren<TextMeshProUGUI>();
                if (enemiesKilledText != null)
                {
                    enemiesKilledText.text = $"Врагов убито: {GameStats.Instance?.enemiesKilled ?? 0}";
                }
            }
        }
    }
    
    private void UpdateCounter()
    {
        UpdateCounterImmediately();
    }
}