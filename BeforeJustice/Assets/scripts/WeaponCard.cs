using UnityEngine;
using TMPro;

public class WeaponCard : MonoBehaviour
{
    [Header("Настройки")]
    public int minDamage = 5;
    public int maxDamage = 7;
    public float accuracy = 0.8f;
    public bool isMelee = true;
    public int maxAmmo = 6; // Новое поле
    public int currentAmmo = 6; // Новое поле
    

    [Header("Ссылки")]
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI accuracyText;
    public TextMeshProUGUI ammoText; // Новое поле

    void Start()
    {
        UpdateCardUI();
    }

    public void UpdateCardUI()
    {
        damageText.text = $"{minDamage}-{maxDamage}";
        accuracyText.text = $"{(accuracy * 100)}%";
        // Показываем патроны только для огнестрела
        ammoText.gameObject.SetActive(!isMelee);
        if (!isMelee) ammoText.text = $"{currentAmmo}/{maxAmmo}";
    }

    public bool TryUseAmmo(bool spendTurn = true)
    {
        if (isMelee) return true;
        
        if (currentAmmo > 0)
        {
            currentAmmo--;
            UpdateCardUI();
            return true;
        }
        return false; // Не тратим ход если нет патронов
    }
    public void ReloadWeapon()
    {
        currentAmmo = maxAmmo;
        UpdateCardUI();
    }
}