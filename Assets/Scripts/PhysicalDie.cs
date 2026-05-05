using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

public class PhysicalDie : MonoBehaviour
{
    public int result = 0;      //the final result
    public GameObject dice;     //the dice prefab

    public GameObject die1;     //the first die & result
    public int die1res = 0;
    public GameObject die2;     //die 2 & result
    public int die2res = 0;

    public bool rollFinished = false;    //if the die has finished rolling


    [SerializeField] private Transform Cam;
    [SerializeField] private float spinForce = 10;
    [SerializeField] private float pushForce = 1;

    // Update is called once per frame
    void Update()
    {
        //if die is in play
        if (die1 != null && die1.GetComponent<DieRoll>().val != 0)
        {
            //return value
            die1res = die1.GetComponent<DieRoll>().val;
        }

        //if die is in play
        if (die2 != null && die2.GetComponent<DieRoll>().val != 0)
        {
            //return value
            die2res = die2.GetComponent<DieRoll>().val;


            //if both die have returned a value
            if (die1res != 0 && die2res != 0)
            {
                //set the result to the sum
                result = die1res + die2res;
                rollFinished = true;
                UpdateDiceUI();
            }
        }
    }

    public void roll()
    {
        rollFinished = false;
        //clear old die from board
        Destroy(die1);
        Destroy(die2);

        //reset result values
        die1res = 0;
        die2res = 0;
        result = 0;

        //instance new die
        die1 = Instantiate(dice, Cam.position, Quaternion.identity);
        die2 = Instantiate(dice, Cam.position, Quaternion.identity);

        //link dice to the manager
        die1.GetComponent<DieRoll>().linkManager(this);
        die2.GetComponent<DieRoll>().linkManager(this);


        //set angular velocity to roll fact (needs tuning)
        die1.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.value * spinForce, Random.value * spinForce, Random.value * spinForce);
        die2.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.value * spinForce, Random.value * spinForce, Random.value * spinForce);

        //sets linear velocity to push away
        die1.GetComponent<Rigidbody>().linearVelocity = Cam.forward * pushForce;
        die2.GetComponent<Rigidbody>().linearVelocity = Cam.forward * pushForce;
    }

    public int GetResults()
    {
        int sum = die1res + die2res;
        return sum;
    }

    public void UpdateDiceUI()
    {
        GameObject diceUI = GameObject.FindGameObjectWithTag("DiceUI"); // GameCanvas.GameView.Dice.RolledDice
        TextMeshProUGUI dice1text = diceUI.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>(); // Text of Dice1
        TextMeshProUGUI dice2text = diceUI.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>(); // Text of Dice2

        dice1text.SetText(die1res.ToString());
        dice2text.SetText(die2res.ToString());
    }

    //clears all of the dices data for the next player
    public void ClearDice()
    {
        die1res = 0;
        die2res = 0;
        result = 0;
        Destroy(die1, 1);
        Destroy(die2, 1);
    }
}

