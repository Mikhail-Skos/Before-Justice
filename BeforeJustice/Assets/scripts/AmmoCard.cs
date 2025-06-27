using UnityEngine;
using System.Collections;

public class AmmoCard : MonoBehaviour
{
    [Header("Настройки")]
    public int ammoAmount = 6;
    public float returnToHandSpeed = 0.3f;

    private DraggableCard draggable;
    private PlayerHand playerHand;
    private Vector3 startPosition;

    void Start()
    {
        draggable = GetComponent<DraggableCard>();
        playerHand = FindObjectOfType<PlayerHand>();
        startPosition = transform.position;
    }

    public bool TryReloadWeapon(WeaponCard weapon)
    {
        // Проверяем можно ли перезарядить это оружие
        if (weapon.isMelee || weapon.currentAmmo >= weapon.maxAmmo)
        {
            StartCoroutine(ReturnToHand());
            return false;
        }
        
        // Полная перезарядка
        weapon.currentAmmo = weapon.maxAmmo;
        weapon.UpdateCardUI();
        
        // Визуальные эффекты
        PlayReloadEffect(weapon.transform.position);
        
        // Уничтожаем карту патронов
        playerHand.RemoveCardFromHand(transform);
        Destroy(gameObject, 0.1f); // Небольшая задержка для эффектов
        
        return true;
    }

    private IEnumerator ReturnToHand()
    {
        float elapsed = 0f;
        Vector3 currentPos = transform.position;
        
        while (elapsed < returnToHandSpeed)
        {
            transform.position = Vector3.Lerp(currentPos, startPosition, elapsed/returnToHandSpeed);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        playerHand.AddCardToHand(transform);
    }

    private void PlayReloadEffect(Vector3 position)
    {
        // Здесь можно добавить частицы/звук
        Debug.Log("Оружие перезаряжено!");
    }
}