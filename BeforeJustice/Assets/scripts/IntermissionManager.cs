using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class IntermissionManager : MonoBehaviour
{
    public static IntermissionManager Instance;

    [Header("Настройки")]
    public int maxIntermissionTurns = 3;
    public int currentIntermissionTurns;

    [Header("Пул карт")]
    public List<CardPool> cardPools;

    [Header("Позиции")]
    public Transform[] cardSlots = new Transform[3];
    
    [Header("UI")]
    public Button battleButton;

    public List<GameObject> currentSelectionCards = new List<GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (battleButton != null)
        {
            battleButton.onClick.AddListener(EndIntermissionButton);
            battleButton.gameObject.SetActive(false);
        }
    }

    public void StartIntermission()
    {
        currentIntermissionTurns = maxIntermissionTurns;
        GenerateSelectionCards();
        TurnSystem.Instance.UpdateTurnsUI();
        
        if (battleButton != null) battleButton.gameObject.SetActive(true);
    }

    void GenerateSelectionCards()
    {
        ClearSelectionCards();

        if (cardPools.Count == 0) return;

        CardPool selectedPool = cardPools[Random.Range(0, cardPools.Count)];

        if (selectedPool.card1 != null) CreateCard(selectedPool.card1, 0);
        if (selectedPool.card2 != null) CreateCard(selectedPool.card2, 1);
        if (selectedPool.card3 != null) CreateCard(selectedPool.card3, 2);
    }

    void CreateCard(GameObject cardPrefab, int slotIndex)
    {
        if (slotIndex >= cardSlots.Length || cardSlots[slotIndex] == null) return;

        GameObject card = Instantiate(cardPrefab, cardSlots[slotIndex].position, Quaternion.identity);
        currentSelectionCards.Add(card);

        DraggableCard draggable = card.GetComponent<DraggableCard>();
        if (draggable != null) draggable.enabled = false;

        IntermissionCard intermissionCard = card.GetComponent<IntermissionCard>();
        if (intermissionCard == null) card.AddComponent<IntermissionCard>();
    }

    public void SelectCard(GameObject card)
    {
        if (currentIntermissionTurns <= 0) return;

        PlayerHand hand = PlayerHand.Instance;
        if (hand.cardsInHand.Count < 5)
        {
            DraggableCard draggable = card.GetComponent<DraggableCard>();
            if (draggable != null)
            {
                draggable.InitializeComponents();
                draggable.enabled = true;
            }
            
            hand.AddCardToHand(card.transform);
            currentSelectionCards.Remove(card);
            SpendTurn();
        }
    }

    public bool TryReplaceCard(Transform oldCard, GameObject newCard)
    {
        if (currentIntermissionTurns <= 0) return false;

        PlayerHand.Instance.ReplaceCard(oldCard, newCard);
        currentSelectionCards.Remove(newCard);
        SpendTurn();
        return true;
    }

    public void SpendTurn()
    {
        currentIntermissionTurns--;
        TurnSystem.Instance.UpdateTurnsUI();

        if (currentIntermissionTurns <= 0)
        {
            EndIntermission();
        }
    }

    public void EndIntermissionButton()
    {
        EndIntermission();
    }

    void EndIntermission()
    {
        ClearSelectionCards();
        GameManager.Instance.StartBattle();
        
        if (battleButton != null) battleButton.gameObject.SetActive(false);
    }

    void ClearSelectionCards()
    {
        foreach (GameObject card in currentSelectionCards)
        {
            if (card != null) Destroy(card);
        }
        currentSelectionCards.Clear();
    }
}

[System.Serializable]
public class CardPool
{
    public GameObject card1;
    public GameObject card2;
    public GameObject card3;
}