using UnityEngine;

[System.Serializable]
public class Card
{
    [SerializeField] private int number;                 //the number attatched to the card
    [SerializeField] private int customCount;            //only used for specific cards, allows the card to be added this many times

    //constructor
    public Card(int number)
    {
        this.number = number;
    }


    /// <summary>
    /// gets the number from the card
    /// </summary>
    /// <returns>returns the number attatched to the card</returns>
    public int GetNumber() { return number; }

    public int GetCustomCount() { return customCount; }
}
