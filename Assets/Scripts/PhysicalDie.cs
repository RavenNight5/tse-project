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

    [SerializeField] private float spinForce = 10;
    [SerializeField] private float pushForce = 1;

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space)) //demo key -- replace with UI button

        //{
        //    roll();
        //}

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
            }

            //if the result is found & die still exsist
            if (result != 0 && die1 != null && die2 != null)
            {
                //clear die after a second
                //Destroy(die1, 1);
                //Destroy(die2, 1);

            }
        }
    }

    public void roll()
    {

        //clear old die from board
        Destroy(die1);
        Destroy(die2);

        //reset result values
        die1res = 0;
        die2res = 0;
        result = 0;

        //instance new die
        die1 = Instantiate(dice, transform.position, Quaternion.identity);
        die2 = Instantiate(dice, transform.position, Quaternion.identity);

        //link dice to the manager
        die1.GetComponent<DieRoll>().linkManager(this);
        die2.GetComponent<DieRoll>().linkManager(this);


        //set angular velocity to roll fact (needs tuning)
        die1.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.value * spinForce, Random.value * spinForce, Random.value * spinForce);
        die2.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.value * spinForce, Random.value * spinForce, Random.value * spinForce);

        //sets linear velocity to push away
        die1.GetComponent<Rigidbody>().linearVelocity = Vector3.forward * pushForce;
        die2.GetComponent<Rigidbody>().linearVelocity = Vector3.forward * pushForce;

    }
}