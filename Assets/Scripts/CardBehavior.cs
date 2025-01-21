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
            cardRenderer = gameObject.AddComponent<SpriteRenderer>();  // Pokud není pøítomen, pøidáme SpriteRenderer
            Debug.Log("SpriteRenderer was not found. Adding one.");
        }
    }

    public void SetCard(Card newCard)
    {
        card = newCard;

        // Nastavíme obrázek karty
        if (cardRenderer != null)
        {
            cardRenderer.sprite = Resources.Load<Sprite>(card.getSprite());
        }
        else
        {
            Debug.LogError("cardRenderer is not assigned.");
        }

        if (!isActiveCard)
        {
            SetCardBack();  // Pokud je pasivní karta, zobrazíme card_back
        }
        else
        {
            ShowCard();  // Pokud je aktivní karta, zobrazíme skuteèný obrázek
        }
    }

    public void SetActiveCard(bool isActive)
    {
        isActiveCard = isActive;

        // Pokud se karta stane aktivní, ukážeme ji
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
        cardRenderer.sprite = Resources.Load<Sprite>(card.getSprite());
        cardRenderer.enabled = true;
    }

    private void SetCardBack()
    {
        cardRenderer.sprite = Resources.Load<Sprite>("card_back");
        cardRenderer.enabled = true;
    }

    public void SetGameManager(GameManager manager)
    {
        gameManager = manager;
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
                Debug.LogError("GameManager is not assigned in CardBehavior.");
                return;
            }

            gameManager.OnCardPlayed(this);
            isClickable = false;
        }
    }


    public void FlipCard(bool show)
    {
        Debug.Log(show ? $"Flipping card face up: {card}" : "Flipping card face down.");
    }
}
