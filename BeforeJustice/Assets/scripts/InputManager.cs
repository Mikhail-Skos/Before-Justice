using UnityEngine;

public class InputManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Клик/Тап в позиции: " + touchPos);
            
            // Здесь будет логика обработки кликов (например, выбор карты)
        }
    }
}