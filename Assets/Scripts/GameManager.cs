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

    private bool canSwapCards = true;  // P�id�me flag, kter� umo�n� v�m�nu pouze jednou

    void Start()
    {
        StartNewRound();
    }

    void StartNewRound()
    {
        // Vyt�hneme 4 karty (2 pro hr��e a 2 pro po��ta�)
        Card playerActiveCard = deckManager.DrawCard();
        Card playerPassiveCard = deckManager.DrawCard();
        Card computerActiveCard = deckManager.DrawCard();
        Card computerPassiveCard = deckManager.DrawCard();

        // Vytvo��me objekty karet pro hr��e a po��ta�e
        playerActiveCardObject = Instantiate(cardPrefab, playerActiveCardPosition.position, Quaternion.identity);
        playerActiveCardObject.GetComponent<CardBehavior>().SetCard(playerActiveCard);
        playerActiveCardObject.GetComponent<CardBehavior>().SetActiveCard(true);  // Aktivn� karta hr��e

        playerPassiveCardObject = Instantiate(cardPrefab, playerPassiveCardPosition.position, Quaternion.identity);
        playerPassiveCardObject.GetComponent<CardBehavior>().SetCard(playerPassiveCard);
        playerPassiveCardObject.GetComponent<CardBehavior>().SetActiveCard(false);  // Pasivn� karta hr��e

        computerActiveCardObject = Instantiate(cardPrefab, computerActiveCardPosition.position, Quaternion.identity);
        computerActiveCardObject.GetComponent<CardBehavior>().SetCard(computerActiveCard);
        computerActiveCardObject.GetComponent<CardBehavior>().SetActiveCard(true);  // Aktivn� karta po��ta�e

        computerPassiveCardObject = Instantiate(cardPrefab, computerPassiveCardPosition.position, Quaternion.identity);
        computerPassiveCardObject.GetComponent<CardBehavior>().SetCard(computerPassiveCard);
        computerPassiveCardObject.GetComponent<CardBehavior>().SetActiveCard(false);  // Pasivn� karta po��ta�e

        playerActiveCardObject.GetComponent<CardBehavior>().SetGameManager(this);
        playerPassiveCardObject.GetComponent<CardBehavior>().SetGameManager(this);
        computerActiveCardObject.GetComponent<CardBehavior>().SetGameManager(this);
        computerPassiveCardObject.GetComponent<CardBehavior>().SetGameManager(this);

        // Povolit klik�n� na pasivn� kartu hr��e pro v�m�nu
        playerPassiveCardObject.GetComponent<CardBehavior>().SetClickable(true);

        Debug.Log("New round started with 4 cards drawn.");
    }

    public void OnCardPlayed(CardBehavior cardBehavior)
    {
        // Pokud hr�� hraje svou pasivn� kartu, provedeme v�m�nu
        if (cardBehavior == playerPassiveCardObject.GetComponent<CardBehavior>() && canSwapCards)
        {
            Debug.Log("Player played passive card. Swapping with active card.");

            // V�m�na karet mezi aktivn� a pasivn� kartou
            SwapCards(playerActiveCardObject, playerPassiveCardObject);

            // Zak�z�n� dal�� v�m�ny
            canSwapCards = false;
        }
    }

    void SwapCards(GameObject activeCardObject, GameObject passiveCardObject)
    {
        CardBehavior activeCardBehavior = activeCardObject.GetComponent<CardBehavior>();
        CardBehavior passiveCardBehavior = passiveCardObject.GetComponent<CardBehavior>();

        // V�m�na karet
        activeCardBehavior.SetActiveCard(false);
        passiveCardBehavior.SetActiveCard(true);

        // Zak�e klik�n� na pasivn� kartu po v�m�n�
        passiveCardBehavior.SetClickable(false);
    }

    void UpdateScoreUI()
    {
        playerScoreText.text = $"Player Score: {playerScore}";
        computerScoreText.text = $"Computer Score: {computerScore}";
    }
}
