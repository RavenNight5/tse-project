using NUnit.Framework.Constraints;
using System.Linq;
using System.Security.Cryptography;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

public static class Testing
{
    /// <summary>
    /// tests all of the functions in code to see what works and what doesnt
    /// </summary>
    /// <returns>true if every test on all functions pass, false if otherwise</returns>
    public static bool TestAll()
    {
        bool allpass = TestStack();
        
        if (allpass)
        {
            Debug.Log("all tests have passed!");
        }
        else
        {
            Debug.LogError("not every test managed to pass! please revise code!");
        }
        
        return allpass;
    }

    /// <summary>
    /// tests our all currently added funcitons in stack to see if anything breaks
    /// </summary>
    /// <returns>the sucsessful conditions, a return of true means everything passed</returns>
    public static bool TestStack()
    {
        bool stackpass = true;
        //tests the generation and gathering functions
        StackHolder.generateCards(2, 10, 4, new Card[0], new int[0]);
        Card[] cards = StackHolder.GetStackCards();
        

        //creates what should be comprable data
        int[] correctVals = new int[36];
        int startvalue = 2;

        for (int i = 0; i < correctVals.Length; i += 4)
        {
            correctVals[i] = startvalue;
            correctVals[i + 1] = startvalue;
            correctVals[i + 2] = startvalue;
            correctVals[i + 3] = startvalue;

            startvalue++;
        }


        //showcases both sets of data for comparason
        Debug.Log("Testing Generation");
        Debug.Log("Input Data: " + getdata(cards));
        Debug.Log("Comprable Data: " + getdata(correctVals));


        //compares both sets of data to see if they match
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].GetNumber() != correctVals[i] )
            {
                stackpass = false;
                Debug.LogError("generator cards to not match inputted data");
            }
        }

        //tests the pulling function
        Card c = StackHolder.PullCard();
        Debug.Log("Testing pull function");
        Debug.Log("Input Data :" + c.GetNumber());
        Debug.Log("Comprable Data :" + 10);

        //compares data
        if (c.GetNumber() != 10)
        {
            stackpass = false;
            Debug.LogError("the pulled card does not match what was expected");
        }

        //tests the shuffle function
        StackHolder.RandShuffle(1000);
        cards = StackHolder.GetStackCards();
        Debug.Log("Testing shuffle funciton");
        Debug.Log("Input Data :" + getdata(cards));

        Debug.Log("checking similarities");

        //checks to see how many of the cards are in the same place
        int sameCount = 0;
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].GetNumber() == correctVals[i])
            {
                sameCount++;
            }
        }

        if (sameCount == cards.Length)
        {
            stackpass = false ;
            Debug.LogError("cards match after shuffle, no shuffling has been done!");
        }
        else if (sameCount > cards.Length/2) 
        {
            stackpass=false;
            Debug.LogWarning("similar cards match at over half the points, amount of matching cards: " + sameCount);
        }
        else
        {
            Debug.Log("amount of matching cards: " + sameCount);
        }
        if (stackpass)
        {
            Debug.Log("Stack functions pass all tests!");
        }
        else
        {
            Debug.LogError("Stack functions have failed in some or all tests!");
        }
        return stackpass;
    }


    private static string getdata(Card[] cards)
    {
        string data = "";
        foreach (Card card in cards)
        {
            data += card.GetNumber();
            data += ", ";
        }
        return data;
    }

    private static string getdata(int[] nums)
    {
        string data = "";
        foreach (int num in nums)
        {
            data += num;
            data += ", ";
        }
        return data;
    }
}
