using UnityEngine;

public class Card
{
    public int Value; // Hodnota karty (1-14)
    public string Suit; // Barva karty (hearts, diamonds, clubs, spades)
    public string ImagePath; // Cesta k obrázku

    public Card(int value, string suit)
    {
        Value = value;
        Suit = suit;
        ImagePath = $"Cards/{value}_of_{suit}";
    }
}