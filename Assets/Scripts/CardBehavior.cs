using UnityEngine;

public class CardBehavior : MonoBehaviour
{
    private Card card;
    private GameManager gameManager;
    private bool isClickable = false;
    private SpriteRenderer cardRenderer;
    private bool isActiveCard = false;

    void Awake()
    {
        cardRenderer = GetComponent<SpriteRenderer>();

        if (cardRenderer == null)
        {
            cardRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
    }
    public void SetCard(Card newCard)
    {
        card = newCard;

        if (cardRenderer != null)
        {
            cardRenderer.sprite = card.GetSprite();
        }

        if (!isActiveCard)
        {
            SetCardBack();
        }
        else
        {
            ShowCard();
        }
    }
    public void SetGameManager(GameManager manager)
    {
        gameManager = manager;
    }
    public void SetActiveCard(bool isActive)
    {
        isActiveCard = isActive;

        if (isActiveCard)
        {
            ShowCard();
        }
        else
        {
            SetCardBack();
        }
    }
    private void ShowCard()
    {
        cardRenderer.sprite = card.GetSprite();
        cardRenderer.enabled = true;
    }
    private void SetCardBack()
    {
        cardRenderer.sprite = Resources.Load<Sprite>("card_back");
        cardRenderer.enabled = true;
    }
    public void SetClickable(bool clickable)
    {
        isClickable = clickable;
    }
    private void OnMouseDown()
    {
        if (isClickable)
        {
            if (gameManager == null)
            {
                return;
            }

            gameManager.OnCardPlayed(this);
            isClickable = false;
        }
    }
    public void FlipCard(bool showFront)
    {
        if (showFront)
        {
            cardRenderer.sprite = card.GetSprite();
        }
        else
        {
            cardRenderer.sprite = Resources.Load<Sprite>("card_back");
        }
    }
    public Card GetCard()
    {
        return card;
    }
}