using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    
    [Header("Настройки")]
    public List<GameObject> enemyPrefabs;
    public Transform enemySpawnPoint;
    
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        // Создаем первого врага при старте
        SpawnNewEnemy();
    }
    
    public void SpawnNewEnemy()
    {
        // Удаляем старого врага если есть
        Enemy existingEnemy = FindObjectOfType<Enemy>();
        if (existingEnemy != null) Destroy(existingEnemy.gameObject);
        
        if (enemyPrefabs.Count == 0) return;
        
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        GameObject newEnemy = Instantiate(enemyPrefab, enemySpawnPoint.position, Quaternion.identity);
        
        // Назначаем нового врага в TurnSystem
        TurnSystem.Instance.enemy = newEnemy.GetComponent<Enemy>();
    }
}