using UnityEngine;
using TMPro;

public class DynamiteCard : MonoBehaviour
{
    [Header("Настройки")]
    public int minDamage = 20;
    public int maxDamage = 30;
    public float activationTime = 2f;

    [Header("Визуал")]
    public ParticleSystem fuseEffect;
    public ParticleSystem explosionEffect;
    public TextMeshProUGUI damageText;

    private bool isActivated;
    private float holdTimer;
    private DraggableCard draggable;
    private TurnSystem turnSystem;

    void Start()
    {
        draggable = GetComponent<DraggableCard>();
        turnSystem = FindObjectOfType<TurnSystem>();
        damageText.text = $"{minDamage}-{maxDamage}";
        if (fuseEffect != null) fuseEffect.Stop();
    }

    void Update()
    {
        if (isActivated) return;

        // Активация динамита при удержании
        if (draggable.IsDragging)
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= activationTime && turnSystem.TryUseTurn())
            {
                Activate();
            }
        }
        else if (holdTimer > 0)
        {
            holdTimer = 0f;
        }
    }

    void Activate()
    {
        isActivated = true;
        if (fuseEffect != null)
        {
            fuseEffect.Play();
        }
        turnSystem.SpendTurn(); // Тратим ход при активации
        Debug.Log("Динамит активирован! Потрачен 1 ход.");
    }

    public bool TryUseDynamite(Enemy enemy)
    {
        if (!isActivated) return false;

        // Проверяем и тратим ход при использовании
        if (!turnSystem.TryUseTurn())
        {
            Debug.Log("Не удалось потратить ход для использования динамита!");
            return false;
        }

        turnSystem.SpendTurn(); // Явно тратим ход
        Debug.Log("Динамит использован на врага! Потрачен еще 1 ход.");
        
        int damage = Random.Range(minDamage, maxDamage + 1);
        enemy.TakeDamage(damage);
        
        Destroy(gameObject);
        return true;
    }

    public void CheckForExplosion()
    {
        if (isActivated)
        {
            ExplodeInHand();
        }
    }

    void ExplodeInHand()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            int damage = Random.Range(minDamage, maxDamage + 1) / 2;
            playerHealth.TakeDamage(damage);
            Debug.Log($"Динамит взорвался в руке! Игрок получил {damage} урона.");
        }

        PlayerHand hand = FindObjectOfType<PlayerHand>();
        if (hand != null)
        {
            hand.RemoveCardFromHand(transform);
        }
        
        Destroy(gameObject);
    }
}