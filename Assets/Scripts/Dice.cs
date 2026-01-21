using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Dice : MonoBehaviour
{
    [SerializeField] private TMP_Text diceDisplayValue;
    
    private int _diceMaxValue;
    private int _diceRollValue;

    // Initialises the dice with its max value
    public void CreateDice(int diceValue)
    {
        if (diceValue < 1) { return; }

        _diceMaxValue = diceValue;
        diceDisplayValue.SetText(diceValue.ToString());
    }

    // Roll this dice and return a random value between 1 and its max val (inclusive)
    public int RollDice()
    {
        _diceRollValue = Random.Range(1, _diceMaxValue);

        diceDisplayValue.SetText(_diceRollValue.ToString());

        return _diceRollValue;
    }
}
