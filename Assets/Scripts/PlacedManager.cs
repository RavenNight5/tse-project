using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlacedManager : MonoBehaviour
{
    [SerializeField] public int placedValue;
    [SerializeField] private Transform placedCards;

    private List<GameObject> _placedCards = new List<GameObject>();  // Holds game objects of the placed/played cards

    // When the player matches their value to current, or if the game is reset/starting
    public void ResetPlaced(GameObject card)
    {
        int value = card.GetComponent<Card>().GetNumber();

        placedValue = value;
        card.transform.SetParent(placedCards, false);
        card.transform.position = placedCards.position;
        _placedCards.Add(card);
    }

    public void PlaceCards(int value, List<GameObject> cards)
    {
        placedValue = cards[cards.Count - 1].GetComponent<Card>().GetNumber();
        
        foreach (var card in cards)
        {
            card.GetComponent<Card>().Deselect(true);

            card.transform.SetParent(placedCards, false);
            card.transform.position = placedCards.position;
            _placedCards.Add(card);
        }
    }
}
