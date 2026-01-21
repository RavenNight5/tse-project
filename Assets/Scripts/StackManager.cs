using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public class StackManager : MonoBehaviour
{
    [Header("Main Cards")]
    [SerializeField][Range(1, 10)] [Tooltip("the minimum card value")] private int cardsMin = 2;                //the minimum card value
    [SerializeField][Range(2, 20)] [Tooltip("the maximum card value")] private int cardsMax = 10;                //the maximum card value
    [SerializeField][Range(1, 10)][Tooltip("how many of each generalised card you would like")] private int cardCount = 4;    //how many of each general card number there is
    
    [Header("Other")]
    [SerializeField][Tooltip("any other cards you wish to add to the stack")] private Card[] specificCards;                 //any other cards (and their count) added on top of the general card pile
    [SerializeField][Tooltip("the count of each spefic card you would like")] private int[] specificCount;                  //the count of each specific card wanted
    [SerializeField][Tooltip("how many times the cards should be randomly swapped")]  private int shuffleCount = 1000;      //how many times the cards should be shuffled     



    //temporary debug function used to give basic controls to the functions
    //please change back input handling to the Input System Package when no longer in use
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StackHolder.RandShuffle(shuffleCount);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (specificCards.Length > specificCount.Length)
            {
                Debug.LogError("there are more specific cards then are what is being counted: specific cards for non associated cards is set to 1");
                int offset = specificCards.Length - specificCount.Length;
                int[] newCount = new int[specificCount.Length + offset];
                for (int i = 0; i < specificCount.Length; i++)
                {
                    newCount[i] = specificCount[i];
                }
                for (int i = specificCount.Length; i < specificCount.Length + offset; i++)
                {
                    newCount[i] = 1;
                }
                specificCount = newCount;
            }

            if (specificCards.Length < specificCount.Length)
            {
                Debug.LogError("there are more card counts then are cards: additional card counts will not be applied");
            }

            StackHolder.generateCards(cardsMin, cardsMax, cardCount, specificCards, specificCount);
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Card[] cards = StackHolder.GetStackCards();
            foreach (Card card in cards)
            {
                print(card.GetNumber());
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            print(StackHolder.PullCard().GetNumber());
        }
    }
}
