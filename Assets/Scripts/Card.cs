using UnityEngine;

public class Card
{
    public int Value { get; private set; }
    public string Suit { get; private set; }

    public Card(int value, string suit)
    {
        Value = value;
        Suit = suit;
    }
    public Sprite GetSprite()
    {
        string spriteName = $"{Value}_of_{Suit}";

        string path = $"Cards/{spriteName}";

        Sprite sprite = Resources.Load<Sprite>(path);

        return sprite;
    }
}
