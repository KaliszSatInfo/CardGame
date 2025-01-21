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

    private bool canSwapCards = true;  // Pøidáme flag, který umožní výmìnu pouze jednou

    void Start()
    {
        StartNewRound();
    }

    void StartNewRound()
    {
        // Vytáhneme 4 karty (2 pro hráèe a 2 pro poèítaè)
        Card playerActiveCard = deckManager.DrawCard();
        Card playerPassiveCard = deckManager.DrawCard();
        Card computerActiveCard = deckManager.DrawCard();
        Card computerPassiveCard = deckManager.DrawCard();

        // Vytvoøíme objekty karet pro hráèe a poèítaèe
        playerActiveCardObject = Instantiate(cardPrefab, playerActiveCardPosition.position, Quaternion.identity);
        playerActiveCardObject.GetComponent<CardBehavior>().SetCard(playerActiveCard);
        playerActiveCardObject.GetComponent<CardBehavior>().SetActiveCard(true);  // Aktivní karta hráèe

        playerPassiveCardObject = Instantiate(cardPrefab, playerPassiveCardPosition.position, Quaternion.identity);
        playerPassiveCardObject.GetComponent<CardBehavior>().SetCard(playerPassiveCard);
        playerPassiveCardObject.GetComponent<CardBehavior>().SetActiveCard(false);  // Pasivní karta hráèe

        computerActiveCardObject = Instantiate(cardPrefab, computerActiveCardPosition.position, Quaternion.identity);
        computerActiveCardObject.GetComponent<CardBehavior>().SetCard(computerActiveCard);
        computerActiveCardObject.GetComponent<CardBehavior>().SetActiveCard(true);  // Aktivní karta poèítaèe

        computerPassiveCardObject = Instantiate(cardPrefab, computerPassiveCardPosition.position, Quaternion.identity);
        computerPassiveCardObject.GetComponent<CardBehavior>().SetCard(computerPassiveCard);
        computerPassiveCardObject.GetComponent<CardBehavior>().SetActiveCard(false);  // Pasivní karta poèítaèe

        playerActiveCardObject.GetComponent<CardBehavior>().SetGameManager(this);
        playerPassiveCardObject.GetComponent<CardBehavior>().SetGameManager(this);
        computerActiveCardObject.GetComponent<CardBehavior>().SetGameManager(this);
        computerPassiveCardObject.GetComponent<CardBehavior>().SetGameManager(this);

        // Povolit klikání na pasivní kartu hráèe pro výmìnu
        playerPassiveCardObject.GetComponent<CardBehavior>().SetClickable(true);

        Debug.Log("New round started with 4 cards drawn.");
    }

    public void OnCardPlayed(CardBehavior cardBehavior)
    {
        // Pokud hráè hraje svou pasivní kartu, provedeme výmìnu
        if (cardBehavior == playerPassiveCardObject.GetComponent<CardBehavior>() && canSwapCards)
        {
            Debug.Log("Player played passive card. Swapping with active card.");

            // Výmìna karet mezi aktivní a pasivní kartou
            SwapCards(playerActiveCardObject, playerPassiveCardObject);

            // Zakázání další výmìny
            canSwapCards = false;
        }
    }

    void SwapCards(GameObject activeCardObject, GameObject passiveCardObject)
    {
        CardBehavior activeCardBehavior = activeCardObject.GetComponent<CardBehavior>();
        CardBehavior passiveCardBehavior = passiveCardObject.GetComponent<CardBehavior>();

        // Výmìna karet
        activeCardBehavior.SetActiveCard(false);
        passiveCardBehavior.SetActiveCard(true);

        // Zakáže klikání na pasivní kartu po výmìnì
        passiveCardBehavior.SetClickable(false);
    }

    void UpdateScoreUI()
    {
        playerScoreText.text = $"Player Score: {playerScore}";
        computerScoreText.text = $"Computer Score: {computerScore}";
    }
}
