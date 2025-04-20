using UnityEngine;
using TMPro;
using System.Collections;

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
    public TMP_Text roundResultText;

    public UnityEngine.UI.Button endRoundButton;

    private int playerScore = 0;
    private int computerScore = 0;

    private bool hasSwapped = false;
    private bool aiHasSwapped = false;
    private bool isRoundInProgress = false;

    private GameObject playerActiveCardObject;
    private GameObject playerPassiveCardObject;
    private GameObject computerActiveCardObject;
    private GameObject computerPassiveCardObject;

    void Start()
    {
        endRoundButton.interactable = false;
        deckManager.InitializeDeck();
        StartNewRound();
    }

    void StartNewRound()
    {
        isRoundInProgress = true;
        hasSwapped = false;
        aiHasSwapped = false;
        DestroyExistingCards();

        if (deckManager.DeckCount < 4)
        {
            deckManager.InitializeDeck();
        }

        CreateCardObjects();

        Debug.Log("AI starts!");
        StartCoroutine(AITurn());
    }

    private void CreateCardObjects()
    {
        Card playerActiveCard = deckManager.DrawCard();
        Card playerPassiveCard = deckManager.DrawCard();
        Card computerActiveCard = deckManager.DrawCard();
        Card computerPassiveCard = deckManager.DrawCard();

        playerActiveCardObject = CreateCard(playerActiveCard, playerActiveCardPosition, true);
        playerPassiveCardObject = CreateCard(playerPassiveCard, playerPassiveCardPosition, false);
        computerActiveCardObject = CreateCard(computerActiveCard, computerActiveCardPosition, true);
        computerPassiveCardObject = CreateCard(computerPassiveCard, computerPassiveCardPosition, false);
    }

    private GameObject CreateCard(Card card, Transform position, bool isActive)
    {
        GameObject cardObject = Instantiate(cardPrefab, position.position, Quaternion.identity);
        CardBehavior cardBehavior = cardObject.GetComponent<CardBehavior>();
        cardBehavior.SetCard(card);
        cardObject.transform.SetParent(position);
        cardBehavior.SetClickable(!isActive);
        cardBehavior.SetGameManager(this);
        cardBehavior.FlipCard(isActive);

        return cardObject;
    }

    IEnumerator AITurn()
    {
        if (!isRoundInProgress) yield break;

        Debug.Log("AI is playing...");
        yield return new WaitForSeconds(1f);

        bool shouldSwap = !aiHasSwapped && Random.value > 0.5f;
        if (shouldSwap)
        {
            Debug.Log("AI is swapping cards...");
            SwapAICards();
            aiHasSwapped = true;
            yield return new WaitForSeconds(1f);
        }

        Debug.Log("AI ends its round.");
        yield return new WaitForSeconds(1f);

        endRoundButton.interactable = true;
        hasSwapped = false;
    }

    public void EndPlayerTurn()
    {
        if (!isRoundInProgress || !hasSwapped) return;

        endRoundButton.interactable = false;
        EndRound();
    }

    IEnumerator PlayerSwapsCard()
    {
        yield return new WaitForSeconds(1f);

        CardBehavior activeCardBehavior = playerActiveCardObject.GetComponent<CardBehavior>();
        CardBehavior passiveCardBehavior = playerPassiveCardObject.GetComponent<CardBehavior>();

        Card tempCard = activeCardBehavior.GetCard();
        activeCardBehavior.SetCard(passiveCardBehavior.GetCard());
        passiveCardBehavior.SetCard(tempCard);

        activeCardBehavior.FlipCard(true);
        passiveCardBehavior.FlipCard(false);

        hasSwapped = true;
        Debug.Log("Player swapped cards.");

        endRoundButton.interactable = true;
    }

    public void OnCardPlayed(CardBehavior cardBehavior)
    {
        if (hasSwapped) return;
        StartCoroutine(PlayerSwapsCard());
    }

    void SwapAICards()
    {
        CardBehavior activeCardBehavior = computerActiveCardObject.GetComponent<CardBehavior>();
        CardBehavior passiveCardBehavior = computerPassiveCardObject.GetComponent<CardBehavior>();

        Card tempCard = activeCardBehavior.GetCard();
        activeCardBehavior.SetCard(passiveCardBehavior.GetCard());
        passiveCardBehavior.SetCard(tempCard);

        activeCardBehavior.FlipCard(true);
        passiveCardBehavior.FlipCard(false);

        Debug.Log("AI swapped its cards.");
    }

    private void EndRound()
    {
        if (playerActiveCardObject == null || computerActiveCardObject == null) return;

        isRoundInProgress = false;

        CardBehavior playerCardBehavior = playerActiveCardObject.GetComponent<CardBehavior>();
        CardBehavior computerCardBehavior = computerActiveCardObject.GetComponent<CardBehavior>();

        Card playerCard = playerCardBehavior.GetCard();
        Card computerCard = computerCardBehavior.GetCard();

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

        CheckGameOver();
    }

    private void CheckGameOver()
    {
        if (playerScore >= 8)
        {
            UpdateRoundResult("Player has won!");
            Invoke(nameof(EndGame), 1f);
        }
        else if (computerScore >= 8)
        {
            UpdateRoundResult("Computer has won!");
            Invoke(nameof(EndGame), 1f);
        }
        else
        {
            UpdateScoreUI();
            Invoke(nameof(StartNewRound), 2f);
        }
    }

    void EndGame()
    {
        endRoundButton.interactable = false;
        Debug.Log("Game Over!");
    }

    void UpdateScoreUI()
    {
        playerScoreText.text = $"Player Score: {playerScore}";
        computerScoreText.text = $"Computer Score: {computerScore}";
    }

    void UpdateRoundResult(string result)
    {
        roundResultText.text = result;
        Invoke(nameof(ClearRoundResultText), 2f);
    }

    void ClearRoundResultText()
    {
        roundResultText.text = "";
    }

    void DestroyExistingCards()
    {
        if (playerActiveCardObject != null) Destroy(playerActiveCardObject);
        if (playerPassiveCardObject != null) Destroy(playerPassiveCardObject);
        if (computerActiveCardObject != null) Destroy(computerActiveCardObject);
        if (computerPassiveCardObject != null) Destroy(computerPassiveCardObject);
    }
}