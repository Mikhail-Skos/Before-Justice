using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Настройки")]
    public int maxHP = 100;
    public int currentHP = 10;
    
    [Header("Ссылки")]
    public TextMeshProUGUI healthText;

    void Start()
    {
        currentHP = Mathf.Clamp(currentHP, 1, maxHP);
        UpdateHealthUI();
    }

public void TakeDamage(int damage)
{
    currentHP -= damage;
    currentHP = Mathf.Max(currentHP, 0); // Гарантируем неотрицательное HP
    UpdateHealthUI();
    
    if (currentHP <= 0 && !GameOverManager.Instance.gameOverPanel.activeSelf)
    {
        GameOverManager.Instance.ShowGameOver(GameStats.Instance.enemiesKilled);
    }
}

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = $"{currentHP}/{maxHP}";
    }
}