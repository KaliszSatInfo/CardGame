using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public DeckManager deckManager;
    public Transform playerActiveCardPosition;
    public Transform playerPassiveCardPosition;
    public Transform computerActiveCardPosition;
    public Transform computerPassiveCardPosition;

    public GameObject cardPrefab;
    private GameObject playerActiveCardObject;
    private GameObject playerPassiveCardObject;
    private GameObject computerActiveCardObject;
    private GameObject computerPassiveCardObject;

    public TMP_Text playerScoreText;
    public TMP_Text computerScoreText;

    private int playerScore = 0;
    private int computerScore = 0;

    private bool canSwapCards = true;
    private bool hasSwapped = false;

    void Start()
    {
        StartNewRound();
    }

    void StartNewRound()
    {
        if (deckManager.DeckCount < 4)
        {
            Debug.LogWarning("Not enough cards to load 4 cards. Reshuffling...");
            deckManager.InitializeDeck();
        }

        Card playerActiveCard = deckManager.DrawCard();
        Card playerPassiveCard = deckManager.DrawCard();
        Card computerActiveCard = deckManager.DrawCard();
        Card computerPassiveCard = deckManager.DrawCard();

        playerActiveCardObject = Instantiate(cardPrefab, playerActiveCardPosition.position, Quaternion.identity);
        playerActiveCardObject.GetComponent<CardBehavior>().SetCard(playerActiveCard);
        playerActiveCardObject.transform.SetParent(playerActiveCardPosition);
        playerActiveCardObject.GetComponent<CardBehavior>().SetClickable(false);
        playerActiveCardObject.GetComponent<CardBehavior>().SetGameManager(this);
        playerActiveCardObject.GetComponent<CardBehavior>().FlipCard(true);

        playerPassiveCardObject = Instantiate(cardPrefab, playerPassiveCardPosition.position, Quaternion.identity);
        playerPassiveCardObject.GetComponent<CardBehavior>().SetCard(playerPassiveCard);
        playerPassiveCardObject.transform.SetParent(playerPassiveCardPosition);
        playerPassiveCardObject.GetComponent<CardBehavior>().SetClickable(true);
        playerPassiveCardObject.GetComponent<CardBehavior>().SetGameManager(this);
        playerPassiveCardObject.GetComponent<CardBehavior>().FlipCard(false);

        computerActiveCardObject = Instantiate(cardPrefab, computerActiveCardPosition.position, Quaternion.identity);
        computerActiveCardObject.GetComponent<CardBehavior>().SetCard(computerActiveCard);
        computerActiveCardObject.transform.SetParent(computerActiveCardPosition);
        computerActiveCardObject.GetComponent<CardBehavior>().SetClickable(false);
        computerActiveCardObject.GetComponent<CardBehavior>().SetGameManager(this);
        computerActiveCardObject.GetComponent<CardBehavior>().FlipCard(true);

        computerPassiveCardObject = Instantiate(cardPrefab, computerPassiveCardPosition.position, Quaternion.identity);
        computerPassiveCardObject.GetComponent<CardBehavior>().SetCard(computerPassiveCard);
        computerPassiveCardObject.transform.SetParent(computerPassiveCardPosition);
        computerPassiveCardObject.GetComponent<CardBehavior>().SetClickable(false);
        computerPassiveCardObject.GetComponent<CardBehavior>().SetGameManager(this);
        computerPassiveCardObject.GetComponent<CardBehavior>().FlipCard(false);

        Debug.Log("New round started with 4 cards drawn.");
    }


    public void OnCardPlayed(CardBehavior cardBehavior)
    {
        if (hasSwapped) return;

        if (cardBehavior == playerPassiveCardObject.GetComponent<CardBehavior>())
        {
            CardBehavior activeCardBehavior = playerActiveCardObject.GetComponent<CardBehavior>();
            CardBehavior passiveCardBehavior = playerPassiveCardObject.GetComponent<CardBehavior>();

            Card tempCard = activeCardBehavior.GetCard();
            activeCardBehavior.SetCard(passiveCardBehavior.GetCard());
            passiveCardBehavior.SetCard(tempCard);

            activeCardBehavior.FlipCard(true);
            passiveCardBehavior.FlipCard(false);

            hasSwapped = true;
            Debug.Log("Player's cards swapped.");
        }
    }


    private void SwapCards(Transform activePosition, Transform passivePosition)
    {
        if (activePosition.childCount == 0 || passivePosition.childCount == 0)
        {
            Debug.LogError("Active or Passive position doesn't have a card to swap.");
            return;
        }

        Transform activeCard = activePosition.GetChild(0);
        Transform passiveCard = passivePosition.GetChild(0);

        Vector3 tempPosition = activeCard.position;
        activeCard.position = passiveCard.position;
        passiveCard.position = tempPosition;

        activeCard.GetComponent<CardBehavior>().SetClickable(true);
        passiveCard.GetComponent<CardBehavior>().SetClickable(false);

        activeCard.GetComponent<CardBehavior>().FlipCard(true);
        passiveCard.GetComponent<CardBehavior>().FlipCard(false);
    }

    void UpdateScoreUI()
    {
        playerScoreText.text = $"Player Score: {playerScore}";
        computerScoreText.text = $"Computer Score: {computerScore}";
    }
}
