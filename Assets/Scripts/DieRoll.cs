using Palmmedia.ReportGenerator.Core;
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
            float OneSix = Mathf.Round(Vector3.Dot(transform.up,Vector3.up));
            float ThreeFour = Mathf.Round(Vector3.Dot(transform.right, Vector3.up));
            float TwoFive = Mathf.Round(Vector3.Dot(transform.forward, Vector3.up));

            print (OneSix+ " "+ TwoFive+ " "+ ThreeFour);

            if (OneSix == 1)
            {
                val = 6;
            }
            if (OneSix == -1)
            {
                val = 1;
            }
            if (ThreeFour == 1)
            {
                val = 4;
            }
            if (ThreeFour == -1)
            {
                val = 3;
            }
            if (TwoFive == 1)
            {
                val = 5;
            }
            if (TwoFive == -1)
            {
                val = 2;
            }
        }
    }


    //links the die to the manager - as manager is public can probably be easily removed
    public void linkManager(PhysicalDie Getmanager)
    {
        manager = Getmanager;
    }

}
