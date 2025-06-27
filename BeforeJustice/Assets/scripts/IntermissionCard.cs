using UnityEngine;
using UnityEngine.EventSystems;

public class IntermissionCard : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.currentState != GameManager.GameState.Intermission) return;
        IntermissionManager.Instance.SelectCard(gameObject);
    }
}