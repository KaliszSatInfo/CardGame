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
    public TMP_Text roundResultText;

    public UnityEngine.UI.Button endRoundButton;

    private int playerScore = 0;
    private int computerScore = 0;
    private int timeTillNewRound = 2;
    private int timeTillGameOver = 1;

    private bool hasSwapped = false;
    private bool gameOver = false;
    private bool isRoundInProgress = false;

    void Start()
    {
        endRoundButton.interactable = true;
        if (deckManager == null)
        {
            return;
        }

        if (endRoundButton != null)
        {
            endRoundButton.onClick.AddListener(EndRound);
        }

        deckManager.InitializeDeck();
        StartNewRound();
    }
    void StartNewRound()
    {
        isRoundInProgress = false;

        hasSwapped = false;
        DestroyExistingCards();

        if (deckManager.DeckCount < 4)
        {
            deckManager.InitializeDeck();
        }

        Card playerActiveCard = deckManager.DrawCard();
        Card playerPassiveCard = deckManager.DrawCard();
        Card computerActiveCard = deckManager.DrawCard();
        Card computerPassiveCard = deckManager.DrawCard();

        if (endRoundButton != null)
        {
            endRoundButton.interactable = true;
        }

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
    }
    private GameObject CreateCard(Card card, Transform position, bool isActive)
    {
        if (card == null)
        {
            return null;
        }

        GameObject cardObject = Instantiate(cardPrefab, position.position, Quaternion.identity);
        CardBehavior cardBehavior = cardObject.GetComponent<CardBehavior>();

        if (cardBehavior == null)
        {
            return null;
        }

        cardBehavior.SetCard(card);
        cardObject.transform.SetParent(position);
        cardBehavior.SetClickable(!isActive);
        cardBehavior.SetGameManager(this);
        cardBehavior.FlipCard(isActive);

        return cardObject;
    }
    public void EndRound()
    {
        if (isRoundInProgress) return;

        isRoundInProgress = true;

        if (endRoundButton != null)
        {
            endRoundButton.interactable = false;
        }


        if (playerActiveCardObject == null || computerActiveCardObject == null)
        {
            isRoundInProgress = false;
            return;
        }

        CardBehavior playerCardBehavior = playerActiveCardObject.GetComponent<CardBehavior>();
        CardBehavior computerCardBehavior = computerActiveCardObject.GetComponent<CardBehavior>();

        if (playerCardBehavior == null || computerCardBehavior == null)
        {
            isRoundInProgress = false;
            return;
        }

        Card playerCard = playerCardBehavior.GetCard();
        Card computerCard = computerCardBehavior.GetCard();

        if (playerCard == null || computerCard == null)
        {
            isRoundInProgress = false;
            return;
        }

        if (playerCard.Value > computerCard.Value)
        {
            playerScore++;
            UpdateRoundResult("Player wins this round!");
        }
        else if (playerCard.Value < computerCard.Value)
        {
            computerScore++;
            UpdateRoundResult("Computer wins this round!");
        }
        else
        {
            UpdateRoundResult("This round is a tie!");
        }

        if (playerScore >= 8)
        {;
            UpdateRoundResult("Player has won!");
            Invoke(nameof(EndGame), timeTillGameOver);
            return;
        }
        else if (computerScore >= 8)
        {
            UpdateRoundResult("Computer has won!");
            Invoke(nameof(EndGame), timeTillGameOver);
            return;
        }

        UpdateScoreUI();
        Invoke(nameof(StartNewRound), timeTillNewRound);
    }
    void UpdateRoundResultText(string result)
    {
        if (roundResultText != null)
        {
            roundResultText.text = result;
        }

        Invoke(nameof(ClearRoundResultText), timeTillGameOver);
    }
    void EndGame()
    {
        if (endRoundButton != null)
        {
            endRoundButton.interactable = false;
        }

        gameOver = true;

        playerScoreText.text = "";
        computerScoreText.text = "";
        roundResultText.text = "Game Over!";
    }
    void UpdateScoreUI()
    {
        if (!gameOver)
        {
            playerScoreText.text = $"Player Score: {playerScore}";
            computerScoreText.text = $"Computer Score: {computerScore}";
        }
        else
        {
            playerScoreText.text = "";
            computerScoreText.text = "";
        }
    }
    void DestroyExistingCards()
    {
        if (playerActiveCardObject != null) Destroy(playerActiveCardObject);
        if (playerPassiveCardObject != null) Destroy(playerPassiveCardObject);
        if (computerActiveCardObject != null) Destroy(computerActiveCardObject);
        if (computerPassiveCardObject != null) Destroy(computerPassiveCardObject);
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
        }
    }
    void UpdateRoundResult(string result)
    {
        if (roundResultText != null)
        {
            roundResultText.text = result;
        }
        Invoke(nameof(ClearRoundResultText), timeTillNewRound);
    }

    void ClearRoundResultText()
    {
        if (roundResultText != null)
        {
            roundResultText.text = "";
        }
    }
}