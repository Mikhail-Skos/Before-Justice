using UnityEngine;
using System;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance;
    public int enemiesKilled = 0;
    public event Action OnEnemyKilled;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Инициализация
        ResetStats();
    }
    
    public void RegisterEnemyKill()
    {
        enemiesKilled++;
        OnEnemyKilled?.Invoke();
        Debug.Log($"Убито врагов: {enemiesKilled}");
    }
    
    public void ResetStats()
    {
        enemiesKilled = 0;
        OnEnemyKilled?.Invoke();
    }
    
    // Новый метод для принудительного обновления UI
    public void ForceUIUpdate()
    {
        OnEnemyKilled?.Invoke();
    }
}