using System;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Card : MonoBehaviour
{
    [SerializeField] public bool Selected = false;

    [SerializeField] private GameObject defaultCard;
    [SerializeField] private GameObject selectedCard;

    [SerializeField] private TMP_Text[] numberLabels;

    private int _number;  //the number attatched to the card
    private int _customCount;  //only used for specific cards, allows the card to be added this many times

    private CardManager _cardManager;

    ////constructor
    //public Card(int number)
    //{
    //    this._number = number;

    //    CreateCard();
    //}

    public void CreateCard(int number, CardManager cm)
    {
        _number = number;
        foreach (var label in numberLabels)
        {
            label.SetText(_number.ToString());
        }

        _cardManager = cm;
    }

    // When the card is clicked by the player
    public void Clicked()
    {
        if (transform.parent.tag != "PlayerHand") { return; }

        if (Selected == false)
        {
            string canSelect = _cardManager.CheckIfCanSelect(_number);

            if (canSelect == null)
            {
                _cardManager.SelectCard(_number, gameObject);

                print("Selected!");
                selectedCard.SetActive(true);
                defaultCard.SetActive(false);

                Selected = true;
            }
            else
            {
                Debug.Log(canSelect);

                // Add notify message
            }
        }
        else
        {
            Deselect();
        }
    }

    public void Deselect(bool onlyVisual=false)
    {
        print("Deselected");
        defaultCard.SetActive(true);
        selectedCard.SetActive(false);

        if (!onlyVisual) { _cardManager.DeselectCard(_number, gameObject); }

        Selected = false;
    }

    /// <summary>
    /// gets the number from the card
    /// </summary>
    /// <returns>returns the number attatched to the card</returns>
    public int GetNumber() { return _number; }

    public int GetCustomCount() { if (_customCount >= 2 && _customCount <= 14) { return _customCount; } else { return 0; } }
}
