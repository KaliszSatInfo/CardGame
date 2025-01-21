using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    private List<Card> deck = new List<Card>();
    private string[] suits = { "hearts", "diamonds", "clubs", "spades" };
    public int DeckCount => deck.Count;


    void Start()
    {
        InitializeDeck();
        ShuffleDeck();
    }
    public void InitializeDeck()
    {
        deck.Clear();
        Debug.Log("Initializing deck...");

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

        Debug.Log($"Deck initialized with {deck.Count} cards.");
        ShuffleDeck();
        DebugDeckContent();
    }

    public void DebugDeckContent()
    {
        foreach (Card card in deck)
        {
            Debug.Log($"{card.Value} of {card.Suit}");
        }
    }


    void ShuffleDeck()
    {
        Debug.Log("Shuffling deck...");
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = Random.Range(0, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
        Debug.Log("Deck shuffled.");
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