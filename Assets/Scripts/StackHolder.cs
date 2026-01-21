using System.Collections.Generic;
using UnityEngine;

public static class StackHolder
{
 
    private static List<Card> stack = new List<Card>();     //the stack holding all of the cards
    private static List<Card> playField = new List<Card>(); //the played card pile
    
    /// <summary>
    /// generates all of the cards using the general and spefic card rules set up
    /// </summary>
    public static void generateCards(int genMin, int genMax, int genCount, Card[] spefCards, int[] spefCount)
    {
        Debug.Log("Generating Stack Cards");

        //handles errors due to missinput`
        #region errors
        //handles missinput making cardMax being higher than the minumum
        if (genMin > genMax)
        {
            Debug.LogWarning("Card Minumum is higher then the maximum!: swapping");
            int swap = genMax;
            genMax = genMin;
            genMin = swap;
        }

        //handles if cardCount is not valid (0 or less)
        if (genCount <= 0)
        {
            Debug.LogWarning("cardCount is not valid (either set to 0 or a negative): setting to 1");
            genCount = 1;
        }

        //handles if the stack already had assigned data
        if (stack.Count > 0)
        {
            Debug.LogWarning("stack was not empty upon generation: clearing");
            stack.Clear();
        }
        #endregion

        //the general loop to add in the cards
        //for each card number we want
        for (int i = genMin; i <= genMax; i++)
        {
            //for each time we want the card
            for (int j = 0; j < genCount; j++)
            {
                stack.Add(new Card(i));
            }
        }

        //for each specific card wanted
        for (int i = 0; i < spefCards.Length; i++)
        {
            //for each time we want the specific card
            for (int j = 0; j < spefCount[i]; j++)
            {
                stack.Add(spefCards[i]);
            }
        }
    }

    /// <summary>
    /// shuffles cards by randomly swapping their places
    /// </summary>
    /// <param name="shuffleCount">how many times the RandShuffle should run</param>
    public static void RandShuffle(int shuffleCount)
    {
        //handles if the shuffleCount is negative
        if (shuffleCount <= 0)
        {
            Debug.LogWarning("shuffling set to 0 or less: no shuffling has been done");
        }

        Card swap = null;
        int pos1;
        int pos2;

        //for how many times we want to shuffle
        for (int i = 0; i < shuffleCount; i++)
        {
            //picks 2 random points on the Stack
            pos1 = Random.Range(0, stack.Count);
            pos2 = Random.Range(0, stack.Count);

            //gaurantees the positions are different
            while (pos2 == pos1)
            {
                pos2 = Random.Range(0, stack.Count);
            }

            //swaps positions
            swap = stack[pos1];
            stack[pos1] = stack[pos2];
            stack[pos2] = swap;

        }
    }

    /// <summary>
    /// allows a player to pull a card from the stack
    /// </summary>
    /// <returns>returns the card</returns>
    public static Card PullCard()
    {
        //handles if the Stack is empty
        if (stack.Count == 0)
        {
            Debug.LogWarning("Stack is empty: returning null");
            return null;
        }

        Card card = stack[stack.Count - 1];
        stack.RemoveAt(stack.Count - 1);
        return card;
    }

    /// <summary>
    /// get all the cards in the stack
    /// </summary>
    /// <returns>an array containing all the collected cards currently in the stack</returns>
    public static Card[] GetStackCards()
    {
        Card[] cards = new Card[stack.Count];

        for (int i = 0; i < stack.Count; i++)
        {
            cards[i] = stack[i];
        }
        return cards;
    }
}
