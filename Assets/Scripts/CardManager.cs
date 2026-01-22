using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public class CardManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] public GameObject CardPrefab;

    [Space]
    [Header("Main Cards")]
    [SerializeField][Range(1, 10)][Tooltip("the minimum card value")] private int cardsMin;                //the minimum card value
    [SerializeField][Range(2, 20)][Tooltip("the maximum card value")] private int cardsMax;                //the maximum card value
    [SerializeField][Range(1, 10)][Tooltip("how many of each generalised card you would like")] private int cardCount;    //how many of each general card number there is

    [Header("Other")]
    [SerializeField][Tooltip("any other cards you wish to add to the stack")] private Card[] specificCards;                 //any other cards (and their count) added on top of the general card pile
    [SerializeField][Tooltip("how many times the cards should be randomly swapped")] private int shuffleCount = 1000;      //how many times the cards should be shuffled     

    //[Space]
    //[SerializeField][Tooltip("what holds all of the cards on the stack")] private List<Card> Stack = new List<Card>();    //the stack holding all of the cards

    private Transform _stack;
    private GameManager _gameManager;

    [SerializeField] private List<GameObject> _stackedCards = new List<GameObject>();  // Holds game objects of the stacked cards - replacing List<Card> Stack; the GameObject can serve as a reference to the card class

    private void Start()
    {
        _stack = GameObject.FindGameObjectWithTag("Stack").transform;
        if (_stack == null)
        {
            Debug.LogError("Stack does not exist.");
        }

        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("GameManager does not exist.");
        }
    }

    /// <summary>
    /// generates all of the cards using the general and spefic card rules set up
    /// </summary>
    public void GenerateCards()
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
        if (_stackedCards.Count > 0)
        {
            Debug.LogWarning("stack was not empty upon generation: clearing");
            _stackedCards.Clear();
        }

        if (_stack == null)
        {
            Debug.LogError("Cannot generate cards because Stack transform is null.");
            return;
        }

        if (CardPrefab == null)
        {
            Debug.LogError("CardPrefab is not assigned on CardManager.");
            return;
        }
        #endregion

        //the general loop to add in the cards
        //for each card number we want
        for (int i = cardsMin; i <= cardsMax; i++)
        {
            //for each time we want the card
            for (int j = 0; j < cardCount; j++)
            {
                GameObject newCard = Instantiate(CardPrefab, _stack);
                var card = newCard.GetComponent<Card>();
                if (card == null)
                {
                    Debug.LogError("CardPrefab does not contain a Card component.");
                    Destroy(newCard);
                    continue;
                }

                card.CreateCard(i, this);

                _stackedCards.Add(newCard);
            }
        }

        //for each specific card wanted
        for (int i = 0; i < specificCards.Length; i++)
        {
            var template = specificCards[i];
            if (template == null)
            {
                Debug.LogWarning($"specificCards[{i}] is null. Skipping.");
                continue;
            }

            int val = template.GetCustomCount();

            if (val != 0)
            {
                int templateNumber = template.GetNumber(); // use the template's actual number/value

                //for each time we want the specific card
                for (int j = 0; j < val; j++)
                {
                    GameObject newCard = Instantiate(CardPrefab, _stack);
                    var card = newCard.GetComponent<Card>();
                    if (card == null)
                    {
                        Debug.LogError("CardPrefab does not contain a Card component.");
                        Destroy(newCard);
                        continue;
                    }

                    card.CreateCard(templateNumber, this);

                    _stackedCards.Add(newCard);
                }
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
            return;
        }

        if (_stackedCards == null || _stackedCards.Count <= 1)
        {
            Debug.LogWarning("Not enough cards to shuffle.");
            return;
        }

        GameObject swap = null;
        int pos1;
        int pos2;

        //for how many times we want to shuffle
        for (int i = 0; i < times; i++)
        {
            //picks 2 random points in the stack of cards
            pos1 = Random.Range(0, _stackedCards.Count);
            pos2 = Random.Range(0, _stackedCards.Count);

            //gaurantees the positions are different
            while (pos2 == pos1)
            {
                pos2 = Random.Range(0, _stackedCards.Count);
            }

            //swaps positions
            swap = _stackedCards[pos1];
            _stackedCards[pos1] = _stackedCards[pos2];
            _stackedCards[pos2] = swap;

        }
    }

    /// <summary>
    /// allows a player to pull a card from the stack
    /// </summary>
    /// <returns>returns the card</returns>
    public GameObject PullCard(bool forHand)
    {
        //handles if the Stack is empty
        if (_stackedCards == null || _stackedCards.Count == 0)
        {
            Debug.LogWarning("Stack is empty.");
            return null;
        }

        int i = _stackedCards.Count - 1;

        GameObject card = _stackedCards[i];

        if (card == null)
        {
            Debug.LogWarning($"Card at index {i} is null. Removing and returning null.");
            _stackedCards.RemoveAt(i);
            return null;
        }

        if (forHand)
        {
            if (_gameManager != null && _gameManager.CurrentPlayer != null)
            {
                _gameManager.CurrentPlayer.AddCardToHand(card);
            }
            else
            {
                Debug.LogWarning("Cannot add card to hand: GameManager or CurrentPlayer is null.");
            }
        }

        _stackedCards.RemoveAt(i);

        return card;
    }

    public GameObject PickStartingcard()
    {
        // If there are no cards available try to generate them automatically once.
        if (_stackedCards == null || _stackedCards.Count == 0)
        {
            Debug.Log("No cards in stack when picking starting card. Attempting to GenerateCards() and shuffle.");
            GenerateCards();

            // Optionally shuffle after generation
            if (_stackedCards != null && _stackedCards.Count > 1 && shuffleCount > 0)
            {
                RandShuffle(shuffleCount);
            }

            if (_stackedCards == null || _stackedCards.Count == 0)
            {
                Debug.LogWarning("Still no cards after GenerateCards(). PickStartingcard will return null.");
                return null;
            }
        }

        GameObject card = PullCard(false);

        return card;
    }

    public string CheckIfCanSelect(int value)
    {
        return _gameManager.CurrentPlayer.CheckIfCanSelect(value);
    }

    public void SelectCard(int value, GameObject card)
    {
        _gameManager.CurrentPlayer.SelectedCard(value, card);
    }
    public void DeselectCard(int value, GameObject card)
    {
        _gameManager.CurrentPlayer.DeselectCard(value, card);
    }


    //debug function used to showcase the cards
    private void showCards()
    {
        for (int i = 0; i < _stackedCards.Count; i++)
        {
            print(_stackedCards[i].GetComponent<Card>().GetNumber());
        }
    }

    //temporary debug function used to give basic controls to the functions
    //please change back input handling to the Input System Package when no longer in use
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        RandShuffle(shuffleCount);
    //    }
    //    if (Input.GetKeyDown(KeyCode.LeftShift))
    //    {
    //        GenerateCards();
    //    }
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        showCards();
    //    }
    //    if (Input.GetKeyDown(KeyCode.W))
    //    {
    //        var pulled = PullCard(true);
    //        if (pulled != null)
    //        {
    //            print(pulled.GetComponent<Card>().GetNumber());
    //        }
    //        else
    //        {
    //            print("Pulled card was null.");
    //        }
    //    }
    //}

    public void ButtonPressedGenerate() { GenerateCards(); }
    public void ButtonPressedShow() { showCards(); }
    public void ButtonPressedPull() { var c = PullCard(true); if (c != null) c.GetComponent<Card>().GetNumber(); }
    public void ButtonPressedShuffle() { RandShuffle(shuffleCount); }
}
