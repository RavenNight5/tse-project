using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Dice : MonoBehaviour
{
    [SerializeField] private TMP_Text diceDisplayValue;
    
    private int _diceMaxValue;      //the maximum value the dice can have
    private int _diceRollValue;     //the current rolled value of the dice


    /// <summary>
    /// Initialises the dice with its max value
    /// </summary>
    /// <param name="diceValue">the maximum value of the die</param>
    public void CreateDice(int diceValue)
    {
        if (diceValue < 1) { return; }

        _diceMaxValue = diceValue;
        diceDisplayValue.SetText(diceValue.ToString());
    }


    /// <summary>
    /// Roll this dice and return a random value between 1 and its max val (inclusive)
    /// </summary>
    /// <returns>a random value between 1 and its maximum</returns>
    public int RollDice()
    {
        _diceRollValue = Random.Range(1, _diceMaxValue);

        diceDisplayValue.SetText(_diceRollValue.ToString());

        return _diceRollValue;
    }
}
