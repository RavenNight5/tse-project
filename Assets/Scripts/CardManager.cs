using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public class CardManager : MonoBehaviour
{
    [Header("Main Cards")]
    [SerializeField] private int cardsMin;                  //the minimum card value
    [SerializeField] private int cardsMax;                  //the maximum card value
    [SerializeField] private int cardCount;                 //how many of each general card number there is
    
    [Header("Other")]
    [SerializeField] private Vector2Int[] specificCards;    //any other cards added (and their amounts) on top of the general card pile
    [SerializeField] private int shuffleCount = 1000;       //how many times the cards should be shuffled     

    [Header("Stack")]
    public List<int> Stack;

    /// <summary>
    /// generates all of the cards using the general and spefic card rules set up
    /// </summary>
    private void GenerateCards()
    {
        Debug.Log("Generating Stack Cards");

        #region errors
        //handles missinput making cardMax being higher than the minumum
        if (cardsMin > cardsMax)
        {
            Debug.LogWarning("Card Minumum is higher then the maximum!: swapping");
            int swap = cardsMax;
            cardsMax = cardsMin;
            cardsMin = swap;
        }

        //handles if cardCount is not valid (0 or less)
        if (cardCount <= 0)
        {
            Debug.LogWarning("cardCount is not valid (either set to 0 or a negative): setting to 1");
            cardCount = 1;
        }

        //handles if the stack already had assigned data
        if (Stack.Count > 0)
        {
            Debug.LogWarning("stack was not empty upon generation: clearing");
            Stack.Clear();
        }

        #endregion

        //the general loop to add in the cards
        //for each card number we want
        for (int i = cardsMin; i <= cardsMax; i++)
        {
            //for each time we want the card
            for (int j = 0; j < cardCount; j++)
            {
                Stack.Add(i);
            }
        }

        //for each value in the specific cards
        for (int i = 0; i < specificCards.Length; i++)
        {
            //for each time we want the specific card
            for(int j = 0; j < specificCards[i].y; j++)
            {
                Stack.Add(specificCards[i].x);
            }
        }
    }

    /// <summary>
    /// shuffles cards by randomly swapping their places
    /// </summary>
    /// <param name="times">how many times the RandShuffle should run</param>
    public void RandShuffle(int times)
    {
        int swap = 0;
        int pos1;
        int pos2;

        //for how many times we want to shuffle
        for (int i = 0 ; i<times; i++)
        {
            //picks 2 random points on the Stack
            pos1 = Random.Range(0, Stack.Count);
            pos2 = Random.Range(0, Stack.Count);

            //gaurantees the positions are different
            while (pos2 == pos1)
            {
                pos2 = Stack[Random.Range(0, Stack.Count)];
            }

            //swaps positions
            swap = Stack[pos1];
            Stack[pos1] = Stack[pos2];
            Stack[pos2] = swap;

        }
    }

    /// <summary>
    /// allows a player to pull a card from the stack
    /// </summary>
    /// <returns>returns the int of the card</returns>
    public int PullCard()
    {
        //handles if the Stack is empty
        if (Stack.Count == 0)
        {
            Debug.LogWarning("Stack is empty: returning -1");
            return -1;
        }

        int card = Stack[Stack.Count - 1];
        Stack.RemoveAt(Stack.Count - 1);
        return card;
    }

    //temporary debug function used to give basic controls to the functions
    //please change back input handling to the Input System Package when no longer in use
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            RandShuffle(shuffleCount);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            GenerateCards();
        }
    }
}
