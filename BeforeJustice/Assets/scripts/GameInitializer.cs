using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    void Start()
    {
        // Убедимся, что GameStats существует
        if (GameStats.Instance == null)
        {
            GameObject statsObj = new GameObject("GameStats");
            statsObj.AddComponent<GameStats>();
        }
        
        // Принудительно обновляем UI
        if (GameStats.Instance != null)
        {
            GameStats.Instance.ForceUIUpdate();
        }
        
        // Находим и обновляем все UICounter
        UICounter[] counters = FindObjectsOfType<UICounter>(true);
        foreach (var counter in counters)
        {
            if (counter != null)
            {
                counter.UpdateCounterImmediately();
            }
        }
    }
}