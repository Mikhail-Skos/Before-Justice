using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public enum GameState { Battle, Intermission }
    public GameState currentState = GameState.Battle;
    
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
    public void StartIntermission()
    {
        currentState = GameState.Intermission;
        IntermissionManager.Instance.StartIntermission();
    }
    
    public void StartBattle()
    {
        currentState = GameState.Battle;
        EnemyManager.Instance.SpawnNewEnemy();
        TurnSystem.Instance.ResetTurns();
    }
}