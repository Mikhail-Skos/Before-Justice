using UnityEngine;
using TMPro;

public class HealCard : MonoBehaviour
{
    [Header("Настройки")]
    public int minHeal = 15;
    public int maxHeal = 20;

    [Header("Эффекты")]
    public ParticleSystem healEffect;

    public bool TryHeal()
    {
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth == null) return false;
        
        bool inIntermission = GameManager.Instance.currentState == GameManager.GameState.Intermission;
        
        if (playerHealth.currentHP >= playerHealth.maxHP)
            return false;

        if (inIntermission)
        {
            if (IntermissionManager.Instance.currentIntermissionTurns <= 0) return false;
        }
        else
        {
            if (!TurnSystem.Instance.TryUseTurn()) return false;
        }

        int healAmount = minHeal == maxHeal ? minHeal : Random.Range(minHeal, maxHeal + 1);
        playerHealth.Heal(healAmount);
        
        if (healEffect != null)
            Instantiate(healEffect, playerHealth.transform.position, Quaternion.identity);
        
        PlayerHand hand = FindObjectOfType<PlayerHand>();
        if (hand != null) hand.RemoveCardFromHand(transform);
        
        Destroy(gameObject, 0.1f);
        
        return true;
    }
}