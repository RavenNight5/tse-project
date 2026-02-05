using TMPro;
using UnityEngine;

[System.Serializable]
public class Card : MonoBehaviour
{
    [SerializeField] public bool Selected = false;      //card is selected for play

    [SerializeField] private GameObject defaultCard;        //visual to showcase it isn't selected (I think)
    [SerializeField] private GameObject selectedCard;       //visual to showcase is selected

    [SerializeField] private TMP_Text[] numberLabels;   //the text fields where the number should be displayed

    private int _number;  //the number attatched to the card
    private int _customCount;  //only used for specific cards, allows the card to be added this many times

    private CardManager _cardManager;       //the manager that generates and handles all of the card stacks

    /// <summary>
    /// creates the card and sets up it's labels to be correct
    /// </summary>
    /// <param name="number">the number to instantiate the card with</param>
    /// <param name="cm">the card manager that needs to be pulled in</param>
    public void CreateCard(int number, CardManager cm)
    {
        _number = number;
        foreach (var label in numberLabels)
        {
            label.SetText(_number.ToString());
        }

        _cardManager = cm;
    }


    /// <summary>
    /// When the card is clicked by the player
    /// </summary>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="onlyVisual"></param>
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

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetCustomCount() { if (_customCount >= 2 && _customCount <= 14) { return _customCount; } else { return 0; } }
}
