using UnityEngine;
using TMPro;
using System.Collections;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance;

    [Header("Настройки")]
    public int maxTurns = 3;
    public int currentTurns;
    public float enemyAttackDelay = 1f;

    [Header("Ссылки")]
    public TextMeshProUGUI turnsText;
    public Enemy enemy;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        ResetTurns();
    }

    public void ResetTurns()
    {
        currentTurns = maxTurns;
        UpdateTurnsUI();
    }

    public bool TryUseTurn()
    {
        return currentTurns > 0;
    }

    public void SpendTurn()
    {
        if (currentTurns <= 0) return;

        currentTurns--;
        UpdateTurnsUI();

        if (currentTurns <= 0 && GameManager.Instance.currentState == GameManager.GameState.Battle)
        {
            StartCoroutine(EnemyAttackCoroutine());
        }
    }

    IEnumerator EnemyAttackCoroutine()
    {
        yield return new WaitForSeconds(enemyAttackDelay);

        DynamiteCard[] dynamites = FindObjectsOfType<DynamiteCard>();
        foreach (var dynamite in dynamites)
        {
            if (dynamite != null)
                dynamite.CheckForExplosion();
        }

        if (enemy != null)
        {
            enemy.AttackPlayer();
        }

        ResetTurns();
    }

    public void UpdateTurnsUI()
    {
        if (turnsText != null)
        {
            if (GameManager.Instance.currentState == GameManager.GameState.Intermission)
                turnsText.text = $"Ходы между боями: {IntermissionManager.Instance.currentIntermissionTurns}";
            else
                turnsText.text = $"Ходы: {currentTurns}/{maxTurns}";
        }
    }
}