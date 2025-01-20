using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    private List<Card> deck = new List<Card>();
    private string[] suits = { "hearts", "diamonds", "clubs", "spades" };

    void Start()
    {
        InitializeDeck();
        ShuffleDeck();
    }
    void InitializeDeck()
    {
        foreach (string suit in suits)
        {
            for (int value = 1; value <= 13; value++)
            {
                for (int i = 0; i < 2; i++)
                {
                    deck.Add(new Card(value, suit));
                }
            }
        }

       foreach (string suit in suits)
        {
            deck.Add(new Card(14, suit));
        }

        ShuffleDeck();
    }
    void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = Random.Range(0, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }
    public Card DrawCard()
    {
        if (deck.Count == 0)
        {
            Debug.LogWarning("Deck is empty, reshuffling...");
            InitializeDeck();
            ShuffleDeck();
        }

        if (deck.Count > 0)
        {
            Card card = deck[0];
            deck.RemoveAt(0);
            return card;
        }

        Debug.LogError("Deck is still empty after reshuffle!");
        return null;
    }
}