using UnityEngine;

public class CardBehavior : MonoBehaviour
{
    private Card card;
    private GameManager gameManager;
    private bool isClickable = false;
    public SpriteRenderer cardRenderer;

    public void SetCard(Card newCard)
    {
        card = newCard;
        Debug.Log($"Setting card to: {card.Value} of {card.Suit}");

        cardRenderer.sprite = Resources.Load<Sprite>(card.getSprite());
    }



    public int GetCardValue()
    {
        return card.Value;
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
            gameManager.OnCardPlayed(this);
        }
    }

    public void FlipCard(bool show)
    {
        Debug.Log(show ? $"Flipping card face up: {card}" : "Flipping card face down.");
    }
}
