using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

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
    private bool isPlayerTurn = false;

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

        isPlayerTurn = false;
        endRoundButton.interactable = false;

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
        computerActiveCardObject = CreateCard(computerActiveCard, computerActiveCardPosition, false);
        computerPassiveCardObject = CreateCard(computerPassiveCard, computerPassiveCardPosition, false);

        StartCoroutine(AnimateCardEntry(playerActiveCardObject));
        StartCoroutine(AnimateCardEntry(playerPassiveCardObject));
        StartCoroutine(AnimateCardEntry(computerActiveCardObject));
        StartCoroutine(AnimateCardEntry(computerPassiveCardObject));
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

        yield return new WaitForSeconds(1f);

        CardBehavior activeCardBehavior = computerActiveCardObject.GetComponent<CardBehavior>();
        int activeValue = activeCardBehavior.GetCard().Value;

        float swapProbability = activeValue < 7 ? 0.75f : 0.15f;

        bool shouldSwap = !aiHasSwapped && Random.value < swapProbability;
        if (shouldSwap)
        {
            SwapAICards();
            aiHasSwapped = true;
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(1f);

        isPlayerTurn = true;
        hasSwapped = false;

        endRoundButton.interactable = true;
    }


    public void EndPlayerTurn()
    {
        if (!isRoundInProgress || !isPlayerTurn || !hasSwapped) return;

        endRoundButton.interactable = false;
        isPlayerTurn = false;

        EndRound();
    }

    IEnumerator PlayerSwapsCard()
    {
        yield return StartCoroutine(AnimateCardSwap(playerActiveCardObject, playerPassiveCardObject));

        CardBehavior active = playerActiveCardObject.GetComponent<CardBehavior>();
        CardBehavior passive = playerPassiveCardObject.GetComponent<CardBehavior>();

        Card temp = active.GetCard();
        active.SetCard(passive.GetCard());
        passive.SetCard(temp);

        active.FlipCard(true);
        passive.FlipCard(false);

        hasSwapped = true;
        Debug.Log("Player swapped cards.");

        if (isPlayerTurn)
            endRoundButton.interactable = true;
    }

    public void OnCardPlayed(CardBehavior cardBehavior)
    {
        if (!isPlayerTurn || hasSwapped) return;

        StartCoroutine(PlayerSwapsCard());
    }

    void SwapAICards()
    {
        StartCoroutine(AISwapRoutine());
    }

    IEnumerator AISwapRoutine()
    {
        yield return StartCoroutine(AnimateCardSwap(computerActiveCardObject, computerPassiveCardObject));

        CardBehavior active = computerActiveCardObject.GetComponent<CardBehavior>();
        CardBehavior passive = computerPassiveCardObject.GetComponent<CardBehavior>();

        Card temp = active.GetCard();
        active.SetCard(passive.GetCard());
        passive.SetCard(temp);

        active.FlipCard(false);
        passive.FlipCard(false);

        Debug.Log("AI swapped its cards.");
    }


    private void EndRound()
    {
        if (playerActiveCardObject == null || computerActiveCardObject == null) return;

        isRoundInProgress = false;
        isPlayerTurn = false;

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
            Invoke(nameof(EndGame), 1f);
        }
        else if (computerScore >= 8)
        {
            Invoke(nameof(EndGame), 1f);
        }
        else
        {
            UpdateScoreUI();
            StartNewRound();
        }
    }

    void EndGame()
    {
        endRoundButton.interactable = false;
        Debug.Log("Game Over!");

        Invoke(nameof(ReturnToMainMenu), 2f);
    }

    void ReturnToMainMenu()
    {
        if (playerScore >= 8)
            SceneManager.LoadScene("WinScene");
        else if (computerScore >= 8)
            SceneManager.LoadScene("LoseScene");
    }

    void UpdateScoreUI()
    {
        playerScoreText.text = $"Player: {playerScore}";
        computerScoreText.text = $"Computer: {computerScore}";
    }

    void UpdateRoundResult(string result)
    {
        roundResultText.text = result;
        Invoke(nameof(ClearRoundResultText), 1f);
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

    IEnumerator AnimateCardEntry(GameObject cardObject)
    {
        float duration = 0.4f;
        float elapsed = 0f;

        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one * 0.45f;

        cardObject.transform.localScale = startScale;

        SpriteRenderer sr = cardObject.GetComponent<SpriteRenderer>();
        Color startColor = new Color(1, 1, 1, 0);
        Color endColor = new Color(1, 1, 1, 1);
        sr.color = startColor;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            cardObject.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            sr.color = Color.Lerp(startColor, endColor, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        cardObject.transform.localScale = endScale;
        sr.color = endColor;
    }

   IEnumerator AnimateCardSwap(GameObject card1, GameObject card2, float moveDuration = 0.5f, float flipDuration = 0.5f, float delayBetween = 0.2f, System.Action onFlipComplete = null)
    {
        Vector3 startPos1 = card1.transform.position;
        Vector3 startPos2 = card2.transform.position;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            card1.transform.position = Vector3.Lerp(startPos1, startPos2, t);
            card2.transform.position = Vector3.Lerp(startPos2, startPos1, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        card1.transform.position = startPos2;
        card2.transform.position = startPos1;

        yield return new WaitForSeconds(delayBetween);

        elapsed = 0f;

        Quaternion rot1 = card1.transform.rotation;
        Quaternion rot2 = card2.transform.rotation;

        bool swapped = false;

        while (elapsed < flipDuration)
        {
            float t = elapsed / flipDuration;

            float angle;
            if (t < 0.5f)
            {
                angle = Mathf.Lerp(0f, 90f, t * 2f);
            }
            else
            {
                angle = Mathf.Lerp(90f, 0f, (t - 0.5f) * 2f);
            }

            card1.transform.rotation = rot1 * Quaternion.Euler(0f, angle, 0f);
            card2.transform.rotation = rot2 * Quaternion.Euler(0f, angle, 0f);

            if (!swapped && t >= 0.5f)
            {
                swapped = true;
                onFlipComplete?.Invoke();
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        card1.transform.rotation = rot1;
        card2.transform.rotation = rot2;
    }
}