using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public class CardManager : MonoBehaviour
{
    [Header("Main Cards")]
    [SerializeField][Range(1, 10)] [Tooltip("the minimum card value")] private int cardsMin;                //the minimum card value
    [SerializeField][Range(2, 20)] [Tooltip("the maximum card value")] private int cardsMax;                //the maximum card value
    [SerializeField][Tooltip("how many of each generalised card you would like")] private int cardCount;    //how many of each general card number there is
    
    [Header("Other")]
    [SerializeField][Tooltip("any other cards you wish to add to the stack")] private Card[] specificCards;                 //any other cards (and their count) added on top of the general card pile
    [SerializeField][Tooltip("how many times the cards should be randomly swapped")]  private int shuffleCount = 1000;      //how many times the cards should be shuffled     

    [Space]
    [SerializeField][Tooltip("what holds all of the cards on the stack")] private List<Card> Stack = new List<Card>();    //the stack holding all of the cards

    /// <summary>
    /// generates all of the cards using the general and spefic card rules set up
    /// </summary>
    private void generateCards()
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
                Stack.Add(new Card(i));
            }
        }

        //for each specific card wanted
        for (int i = 0; i < specificCards.Length; i++)
        {
            //for each time we want the specific card
            for(int j = 0; j < specificCards[i].GetCustomCount(); j++)
            {
                Stack.Add(specificCards[i]);
            }
        }
    }

    /// <summary>
    /// shuffles cards by randomly swapping their places
    /// </summary>
    /// <param name="times">how many times the RandShuffle should run</param>
    public void RandShuffle(int times)
    {
        //handles if the shuffleCount is negative
        if (times <= 0)
        {
            Debug.LogWarning("shuffling set to 0 or less: no shuffling has been done");
        }

        Card swap = null;
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
                pos2 = Random.Range(0, Stack.Count);
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
    /// <returns>returns the card</returns>
    public Card PullCard()
    {
        //handles if the Stack is empty
        if (Stack.Count == 0)
        {
            Debug.LogWarning("Stack is empty: returning null");
            return null;
        }

        Card card = Stack[Stack.Count - 1];
        Stack.RemoveAt(Stack.Count - 1);
        return card;
    }

    //debug function used to showcase the cards
    private void showCards()
    {
        for (int i = 0; i < Stack.Count; i++) 
        {
            print(Stack[i].GetNumber());
        }
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
            generateCards();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            showCards();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            print(PullCard().GetNumber());
        }
    }
}
