using UnityEngine;
using System.Collections;

public class DraggableCard : MonoBehaviour
{
    [Header("Настройки")]
    public float returnSpeed = 0.4f;

    public bool IsDragging { get; private set; }
    private PlayerHand playerHand;
    private Vector3 startPosition;
    private Camera mainCamera;

    void Start()
    {
        InitializeComponents();
    }

    public void InitializeComponents()
    {
        playerHand = PlayerHand.Instance;
        mainCamera = Camera.main;
        startPosition = transform.position;
    }

    void OnEnable()
    {
        if (mainCamera == null) mainCamera = Camera.main;
    }

    void OnMouseDown()
    {
        if (Input.touchCount > 1) return;
        if (GetComponent<CardInHand>() == null) return;

        IsDragging = true;
        playerHand?.RemoveCardFromHand(transform);
        transform.SetAsLastSibling();
        startPosition = transform.position;
    }

    void OnMouseDrag()
    {
        if (!IsDragging) return;

        Vector3 mousePos = GetMouseWorldPosition();
        transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
    }

    void OnMouseUp()
    {
        if (!IsDragging) return;

        IsDragging = false;

        if (GameManager.Instance.currentState == GameManager.GameState.Intermission)
        {
            CheckIntermissionActions();
        }
        else
        {
            CheckBattleActions();
        }
    }

    void CheckBattleActions()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f);

        foreach (var hit in hits)
        {
            HealZone healZone = hit.GetComponent<HealZone>();
            if (healZone != null && TryGetComponent(out HealCard healCard))
            {
                if (healCard.TryHeal())
                {
                    TurnSystem.Instance.SpendTurn();
                    return;
                }
            }

            if (TryGetComponent(out AmmoCard ammo) && hit.TryGetComponent(out WeaponCard weapon))
            {
                if (ammo.TryReloadWeapon(weapon))
                {
                    TurnSystem.Instance.SpendTurn();
                    return;
                }
            }

            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (TryGetComponent(out DynamiteCard dynamite))
                {
                    if (dynamite.TryUseDynamite(enemy))
                    {
                        return;
                    }
                }
                else if (TryGetComponent(out WeaponCard weaponCard))
                {
                    if (TryAttackEnemy(enemy))
                    {
                        TurnSystem.Instance.SpendTurn();
                        return;
                    }
                }
            }
        }

        StartCoroutine(ReturnToHand());
    }

    void CheckIntermissionActions()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f);

        foreach (var hit in hits)
        {
            HealZone healZone = hit.GetComponent<HealZone>();
            if (healZone != null && TryGetComponent(out HealCard healCard))
            {
                if (healCard.TryHeal())
                {
                    IntermissionManager.Instance.SpendTurn();
                    return;
                }
            }

            if (TryGetComponent(out AmmoCard ammo) && hit.TryGetComponent(out WeaponCard weapon))
            {
                if (ammo.TryReloadWeapon(weapon))
                {
                    IntermissionManager.Instance.SpendTurn();
                    return;
                }
            }

            if (GetComponent<CardInHand>() != null)
            {
                foreach (var card in IntermissionManager.Instance.currentSelectionCards)
                {
                    if (hit.gameObject == card)
                    {
                        if (IntermissionManager.Instance.TryReplaceCard(transform, card))
                        {
                            return;
                        }
                    }
                }
            }
        }

        StartCoroutine(ReturnToHand());
    }

    IEnumerator ReturnToHand()
    {
        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < returnSpeed)
        {
            transform.position = Vector3.Lerp(startPos, startPosition, elapsed/returnSpeed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        playerHand?.AddCardToHand(transform);
    }

    Vector3 GetMouseWorldPosition()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        
        Vector3 mousePos = Input.mousePosition;
        if (Input.touchCount > 0)
        {
            mousePos = Input.GetTouch(0).position;
        }
        mousePos.z = 10f;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

    bool TryAttackEnemy(Enemy enemy)
    {
        WeaponCard weapon = GetComponent<WeaponCard>();
        TurnSystem turnSystem = TurnSystem.Instance;

        if (!weapon.isMelee && weapon.currentAmmo <= 0)
        {
            Debug.Log("Нет патронов! Ход не тратится");
            StartCoroutine(ReturnToHand());
            return false;
        }

        if (weapon == null || !turnSystem.TryUseTurn())
        {
            StartCoroutine(ReturnToHand());
            return false;
        }

        bool attackSuccess = false;

        if (weapon.TryUseAmmo())
        {
            if (Random.value <= weapon.accuracy)
            {
                int damage = Random.Range(weapon.minDamage, weapon.maxDamage + 1);
                enemy.TakeDamage(damage);
                Debug.Log($"Нанесено {damage} урона!");
                attackSuccess = true;
            }
            else
            {
                Debug.Log("Промах!");
                enemy.PlayDodgeEffect();
                turnSystem.SpendTurn();
            }
        }

        StartCoroutine(ReturnToHand());
        return attackSuccess;
    }
}