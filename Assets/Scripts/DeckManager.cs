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
                for (int i = 0; i < 8; i++)
                {
                    deck.Add(new Card(value, suit));
                }
            }
        }

        for (int i = 0; i < 4; i++)
        {
            deck.Add(new Card(14, "joker"));
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
            Debug.Log("Deck is empty, reshuffling...");
            InitializeDeck();
            ShuffleDeck();
        }

        Card drawnCard = deck[0];
        deck.RemoveAt(0);
        return drawnCard;
    }

}