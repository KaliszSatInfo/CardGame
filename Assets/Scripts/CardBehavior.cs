using UnityEngine;

public class CardBehavior : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Card cardData;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void SetCard(Card card)
    {
        cardData = card;
        Sprite cardSprite = Resources.Load<Sprite>(card.ImagePath);
        spriteRenderer.sprite = cardSprite;
    }
    public void FlipCard(bool showFront)
    {
        if (showFront)
        {
            spriteRenderer.sprite = Resources.Load<Sprite>(cardData.ImagePath);
        }
        else
        {
            spriteRenderer.sprite = Resources.Load<Sprite>("Cards/Card_Back");
        }
    }
    public int GetCardValue()
    {
        return cardData.Value;
    }
}
