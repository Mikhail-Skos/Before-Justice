using UnityEngine;
using System.Collections.Generic;

public class PlayerHand : MonoBehaviour
{
    public static PlayerHand Instance;

    [Header("Настройки")]
    public float cardSpacing = 2f;
    public float baseCardScale = 1.78f;
    public float moveSpeed = 10f;

    [Header("Начальные наборы")]
    public List<StartingHand> startingHands;

    public List<Transform> cardsInHand = new List<Transform>();
    public GameObject cardToReplace;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        DrawStartingHand();
    }

    public void DrawStartingHand()
    {
        if (startingHands.Count == 0) return;

        StartingHand selectedHand = startingHands[Random.Range(0, startingHands.Count)];

        if (selectedHand.card1 != null) CreateCard(selectedHand.card1);
        if (selectedHand.card2 != null) CreateCard(selectedHand.card2);
        if (selectedHand.card3 != null) CreateCard(selectedHand.card3);
        if (selectedHand.card4 != null) CreateCard(selectedHand.card4);
        if (selectedHand.card5 != null) CreateCard(selectedHand.card5);
    }

    void CreateCard(GameObject cardPrefab)
    {
        GameObject newCard = Instantiate(cardPrefab, transform);
        AddCardToHand(newCard.transform);
    }

    void Update()
    {
        UpdateCardPositions();
    }

    public void UpdateCardPositions()
    {
        if (cardsInHand.Count == 0) return;

        float totalWidth = (cardsInHand.Count - 1) * cardSpacing;
        float startX = -totalWidth / 2;

        for (int i = 0; i < cardsInHand.Count; i++)
        {
            if (cardsInHand[i] == null) continue;

            Vector3 targetPos = transform.position + new Vector3(startX + i * cardSpacing, 0, 0);
            cardsInHand[i].position = Vector3.Lerp(
                cardsInHand[i].position,
                targetPos,
                moveSpeed * Time.deltaTime
            );

            cardsInHand[i].localScale = Vector3.Lerp(
                cardsInHand[i].localScale,
                Vector3.one * baseCardScale,
                moveSpeed * Time.deltaTime
            );
        }
    }

    public void AddCardToHand(Transform card)
    {
        if (!cardsInHand.Contains(card))
        {
            cardsInHand.Add(card);
            card.SetParent(transform);

            if (!card.GetComponent<CardInHand>())
                card.gameObject.AddComponent<CardInHand>();
            
            DraggableCard draggable = card.GetComponent<DraggableCard>();
            if (draggable != null)
            {
                draggable.InitializeComponents();
                draggable.enabled = true;
            }
        }
    }

    public void RemoveCardFromHand(Transform card)
    {
        if (cardsInHand.Contains(card))
        {
            cardsInHand.Remove(card);
        }
    }

    public void ReplaceCard(Transform oldCard, GameObject newCard)
    {
        RemoveCardFromHand(oldCard);
        Destroy(oldCard.gameObject);
        AddCardToHand(newCard.transform);
    }

    public void DrawNewHand(bool keepExisting)
    {
        if (!keepExisting)
        {
            foreach (Transform card in cardsInHand.ToArray())
            {
                if (card != null) Destroy(card.gameObject);
            }
            cardsInHand.Clear();
        }

        if (GameManager.Instance.currentState == GameManager.GameState.Battle) return;

        int cardsToAdd = 5 - cardsInHand.Count;
        for (int i = 0; i < cardsToAdd; i++)
        {
            if (IntermissionManager.Instance.cardPools.Count > 0)
            {
                CardPool randomPool = IntermissionManager.Instance.cardPools[
                    Random.Range(0, IntermissionManager.Instance.cardPools.Count)
                ];

                GameObject randomCard = randomPool.card1;
                if (randomCard != null) CreateCard(randomCard);
            }
        }
    }
}

[System.Serializable]
public class StartingHand
{
    public GameObject card1;
    public GameObject card2;
    public GameObject card3;
    public GameObject card4;
    public GameObject card5;
}