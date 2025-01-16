using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DeckManager deckManager;
    public Transform playerCardPosition;
    public Transform computerCardPosition;

    public GameObject cardPrefab;
    GameObject playerCardObject;
    GameObject computerCardObject;

    public TMP_Text playerScoreText;
    public TMP_Text computerScoreText;

    private int playerScore = 0;
    private int computerScore = 0;

    private List<Item> playerItems = new List<Item>();
    private List<Item> computerItems = new List<Item>();

    void Start()
    {
        GenerateItems();
        StartNewRound();
    }

    void StartNewRound()
    {
        Card playerCard = deckManager.DrawCard();
        if (playerCard == null)
        {
            Debug.LogError("Failed to draw player card!");
            return;
        }

        playerCardObject = Instantiate(cardPrefab, playerCardPosition.position, Quaternion.identity);
        playerCardObject.GetComponent<CardBehavior>().SetCard(playerCard);
        Debug.Log($"Player card: {playerCard.Value}");

        Card computerCard = deckManager.DrawCard();
        if (computerCard == null)
        {
            Debug.LogError("Failed to draw computer card!");
            return;
        }

        computerCardObject = Instantiate(cardPrefab, computerCardPosition.position, Quaternion.identity);
        computerCardObject.GetComponent<CardBehavior>().SetCard(computerCard);
        Debug.Log($"Computer card: {computerCard.Value}");

        computerCardObject.GetComponent<CardBehavior>().FlipCard(false);

        Invoke(nameof(EndRound), 10.0f);
    }


    void EndRound()
    {
        int playerValue = playerCardObject.GetComponent<CardBehavior>().GetCardValue();
        int computerValue = computerCardObject.GetComponent<CardBehavior>().GetCardValue();

        playerCardObject.GetComponent<CardBehavior>().FlipCard(true);
        computerCardObject.GetComponent<CardBehavior>().FlipCard(true);

        if (playerValue > computerValue)
        {
            playerScore++;
            Debug.Log("Player wins the round!");
        }
        else if (playerValue < computerValue)
        {
            computerScore++;
            Debug.Log("Computer wins the round!");
        }
        else
        {
            Debug.Log("It's a tie! No one gets a point.");
        }

        UpdateScoreUI();

        if (playerScore >= 8)
        {
            Debug.Log("Player wins the game!");
            EndGame();
        }
        else if (computerScore >= 8)
        {
            Debug.Log("Computer wins the game!");
            EndGame();
        }
        else
        {
            StartNewRound();
        }
    }


    void EndGame()
    {
        Debug.Log("Game Over");
    }
    void UpdateScoreUI()
    {
        playerScoreText.text = $"Player Score: {playerScore}";
        computerScoreText.text = $"Computer Score: {computerScore}";
    }

    void GenerateItems()
    {
        playerItems.Add(new Item(
            "Swap Cards",
            "Swap the player's and computer's active cards.",
            SwapCards
        ));

        playerItems.Add(new Item(
            "Random Card",
            "Replace the player's active card with a random card from the deck.",
            ReplaceActiveCard
        ));

        playerItems.Add(new Item(
            "Peek Card",
            "Peek at your passive card or the computer's passive card.",
            PeekCard
        ));
    }

    void SwapCards()
    {
        Transform playerCard = playerCardPosition.GetChild(0);
        Transform computerCard = computerCardPosition.GetChild(0);

        Vector3 tempPosition = playerCard.position;
        playerCard.position = computerCard.position;
        computerCard.position = tempPosition;
    }

    void ReplaceActiveCard()
    {
        Transform playerCard = playerCardPosition.GetChild(0);
        Destroy(playerCard.gameObject);

        Card newCard = deckManager.DrawCard();
        GameObject newCardObject = Instantiate(cardPrefab, playerCardPosition.position, Quaternion.identity);
        newCardObject.GetComponent<CardBehavior>().SetCard(newCard);
    }

    void PeekCard()
    {
        Debug.Log("Peek effect not implemented yet.");
    }
}