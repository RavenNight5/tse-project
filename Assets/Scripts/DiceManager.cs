using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    [SerializeField] private GameObject dicePrefab;
    [SerializeField] private Transform diceHolder;  // Place the initialised dice when they are not being rolled
    [SerializeField] private Transform diceRolled;

    private List<GameObject> _currentDice = new List<GameObject>();

    public void CreateDice(int[] maxValues)
    {
        // Create new dice and set their max values, place them in the invisible diceHolder transform
        for (int i = 0; i < maxValues.Length; i++)
        {
            int diceValue = maxValues[i];

            GameObject newDice = Instantiate(dicePrefab, diceHolder);
            newDice.name = "D" + diceValue.ToString();
            newDice.GetComponent<Dice>().CreateDice(diceValue);

            _currentDice.Add(newDice);
        }
    }

    public int RollDice()
    {
        int totalRoll = 0;

        foreach (GameObject dice in _currentDice)
        {
            totalRoll += dice.GetComponent<Dice>().RollDice();
            dice.transform.SetParent(diceRolled);
        }

        return totalRoll;
    }

    public void OnDisable()
    {
        foreach (GameObject dice in _currentDice)
        {
            dice.transform.SetParent(diceHolder);
        }
    }
}
