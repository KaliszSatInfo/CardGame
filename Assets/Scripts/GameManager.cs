using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DeckManager deckManager;
    public Transform playerActiveCardPosition;
    public Transform playerPassiveCardPosition;
    public Transform computerActiveCardPosition;
    public Transform computerPassiveCardPosition;

    public GameObject cardPrefab;
    public TMP_Text playerScoreText;
    public TMP_Text computerScoreText;

    private int playerScore = 0;
    private int computerScore = 0;

    private GameObject playerActiveCardObject;
    private GameObject playerPassiveCardObject;
    private GameObject computerActiveCardObject;
    private GameObject computerPassiveCardObject;

    private CardBehavior selectedPlayerCard = null;
    private bool isPlayerTurn = true;

    void Start()
    {
        StartNewGame();
    }

    void StartNewGame()
    {
        deckManager.InitializeDeck();
        playerScore = 0;
        computerScore = 0;
        UpdateScoreUI();
        StartNewRound();
    }

    void StartNewRound()
    {
        ClearCards();

        if (deckManager.DeckCount < 4)
        {
            Debug.LogWarning("Not enough cards to load 4 cards. Reshuffling...");
            deckManager.InitializeDeck();
        }

        // Draw cards
        Card playerActiveCard = deckManager.DrawCard();
        Card playerPassiveCard = deckManager.DrawCard();
        Card computerActiveCard = deckManager.DrawCard();
        Card computerPassiveCard = deckManager.DrawCard();

        // Instantiate cards at their positions
        playerActiveCardObject = Instantiate(cardPrefab, playerActiveCardPosition.position, Quaternion.identity);
        var playerActiveBehavior = playerActiveCardObject.GetComponent<CardBehavior>();
        playerActiveBehavior.SetCard(playerActiveCard);
        playerActiveBehavior.SetGameManager(this);
        playerActiveBehavior.SetClickable(true);

        playerPassiveCardObject = Instantiate(cardPrefab, playerPassiveCardPosition.position, Quaternion.identity);
        var playerPassiveBehavior = playerPassiveCardObject.GetComponent<CardBehavior>();
        playerPassiveBehavior.SetCard(playerPassiveCard);
        playerPassiveBehavior.SetGameManager(this);
        playerPassiveBehavior.SetClickable(true);

        computerActiveCardObject = Instantiate(cardPrefab, computerActiveCardPosition.position, Quaternion.identity);
        var computerActiveBehavior = computerActiveCardObject.GetComponent<CardBehavior>();
        computerActiveBehavior.SetCard(computerActiveCard);
        computerActiveBehavior.FlipCard(true);

        computerPassiveCardObject = Instantiate(cardPrefab, computerPassiveCardPosition.position, Quaternion.identity);
        var computerPassiveBehavior = computerPassiveCardObject.GetComponent<CardBehavior>();
        computerPassiveBehavior.SetCard(computerPassiveCard);
        computerPassiveBehavior.FlipCard(true);

        Debug.Log("New round started with 4 cards drawn.");
        isPlayerTurn = true; // Start with the player's turn
    }

    public void OnCardPlayed(CardBehavior playedCard)
    {
        if (!isPlayerTurn || selectedPlayerCard != null) return;

        selectedPlayerCard = playedCard;
        playedCard.SetClickable(false);

        Debug.Log($"Player played card: {playedCard.GetCardValue()}");

        // Now let the computer play
        isPlayerTurn = false;
        Invoke(nameof(ComputerPlay), 1.0f); // Wait 1 second for clarity
    }

    void ComputerPlay()
    {
        CardBehavior computerCardToPlay = computerActiveCardObject.GetComponent<CardBehavior>();
        Debug.Log($"Computer played card: {computerCardToPlay.GetCardValue()}");

        ResolveRound(selectedPlayerCard, computerCardToPlay);
    }

    void ResolveRound(CardBehavior playerCard, CardBehavior computerCard)
    {
        int playerValue = playerCard.GetCardValue();
        int computerValue = computerCard.GetCardValue();

        if (playerValue > computerValue)
        {
            playerScore++;
            Debug.Log("Player wins this round!");
        }
        else if (playerValue < computerValue)
        {
            computerScore++;
            Debug.Log("Computer wins this round!");
        }
        else
        {
            Debug.Log("Round is a tie!");
        }

        UpdateScoreUI();

        // Check for game over
        if (playerScore >= 8 || computerScore >= 8)
        {
            EndGame();
        }
        else
        {
            StartNewRound();
        }
    }

    void EndGame()
    {
        Debug.Log($"Game Over! {(playerScore >= 8 ? "Player" : "Computer")} wins!");
    }

    void UpdateScoreUI()
    {
        playerScoreText.text = $"Player Score: {playerScore}";
        computerScoreText.text = $"Computer Score: {computerScore}";
    }

    void ClearCards()
    {
        if (playerActiveCardObject) Destroy(playerActiveCardObject);
        if (playerPassiveCardObject) Destroy(playerPassiveCardObject);
        if (computerActiveCardObject) Destroy(computerActiveCardObject);
        if (computerPassiveCardObject) Destroy(computerPassiveCardObject);

        Debug.Log("Cleared all card objects from positions.");
    }
}
