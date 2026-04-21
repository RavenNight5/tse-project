using UnityEngine;

public class DieRoll : MonoBehaviour
{
    public PhysicalDie manager;     //the linked manager
    Rigidbody rb;                   //the rigidbody on the die
    public int val = 0;             //the result of the die

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //initialise values
        rb = GetComponent<Rigidbody>();
        val = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //if stopped rolling
        if (rb.angularVelocity == Vector3.zero && val == 0)
        {
            //DEMO - just pick a random number from 1 - 6
            val = Random.Range(1, 7);
        }
    }


    //links the die to the manager - as manager is public can probably be easily removed
    public void linkManager(PhysicalDie Getmanager)
    {
        manager = Getmanager;
    }

}
