using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [Header("Настройки")]
    public int maxHP = 30;
    public int currentHP;

    [Header("Ссылки")]
    public SpriteRenderer spriteRenderer;
    public TextMeshProUGUI hpText;
    public PlayerHealth playerHealth;
    public float dodgeOffset = 0.3f; // Смещение при уклонении

    [Header("Атака")]
    public Vector2 damageRange = new Vector2(5, 10);
    public float accuracy = 0.7f;

    void Start()
    {
        currentHP = maxHP;
        UpdateHPUI();
        
        // Автопоиск если не задан в инспекторе
        if (playerHealth == null)
            playerHealth = FindObjectOfType<PlayerHealth>();
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        UpdateHPUI();
        StartCoroutine(DamageEffect());
        
        if (currentHP <= 0) Die();
    }

    public void PlayDodgeEffect()
    {
        StartCoroutine(DodgeEffect());
    }

    void UpdateHPUI()
    {
        hpText.text = $"{currentHP}/{maxHP}";
    }

    System.Collections.IEnumerator DodgeEffect()
    {
        // Анимация уклонения (белый цвет + смещение)
        Vector3 originalPos = transform.position;
        Color originalColor = spriteRenderer.color;
        
        transform.position = originalPos + Vector3.right * dodgeOffset;
        spriteRenderer.color = Color.white;
        
        yield return new WaitForSeconds(0.1f);
        
        transform.position = originalPos;
        spriteRenderer.color = originalColor;
    }

    System.Collections.IEnumerator DamageEffect()
    {
        // Анимация получения урона (красный цвет)
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        
        yield return new WaitForSeconds(0.1f);
        
        spriteRenderer.color = originalColor;
    }

    void Die()
    {
        // Регистрируем убийство врага
        GameStats.Instance.RegisterEnemyKill();
        
        GameManager.Instance.StartIntermission();
        Destroy(gameObject);
    }

    public void AttackPlayer()
    {
        if (Random.value > accuracy)
        {
            Debug.Log("Враг промахнулся!");
            return;
        }

        int damage = Mathf.RoundToInt(Random.Range(damageRange.x, damageRange.y));
        playerHealth.TakeDamage(damage);
        Debug.Log($"Враг нанес {damage} урона!");
    }
}