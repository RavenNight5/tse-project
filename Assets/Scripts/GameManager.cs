using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public int PlayerCount { get; private set; }  // How many players there are in the game

    [HideInInspector]
    public List<GameObject> Players = new List<GameObject>();
    public Player CurrentPlayer { get; private set; }

    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;

    [Header("Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject stackActions;
    [SerializeField] private GameObject diceRoll;
    [SerializeField] private GameObject placed;
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject buffer;

    private bool gameReady = false;

    [Header("Veriables")]
    [SerializeField] private int startingCards = 5;

    private DiceManager _diceManager;
    private PlacedManager _placedManager;
    private CardManager _cardManager;
    private PhysicalDie _physDice;

    private Transform _players;
    private int _currentPlayer = 0;  // Index of the current player (who's turn it is)

    private int _currentPlacedValue = 0;

    [SerializeField] public static int MaxMergeValue = 7;  // Can merge cards up to and including this value
    [SerializeField] public static int MaxCardMerges = 3;  // How many cards can be merged at once

    private void Start()
    {
        mainMenu.SetActive(true);
        win.SetActive(false);
        stackActions.SetActive(false);
        buffer.SetActive(false);

        _players = GameObject.FindGameObjectWithTag("Players").transform;
        _diceManager = diceRoll.GetComponent<DiceManager>();
        _placedManager = placed.GetComponent<PlacedManager>();
        _cardManager = GameObject.FindGameObjectWithTag("CardManager").GetComponent<CardManager>();

        _physDice = GameObject.FindGameObjectWithTag("PhysDie").GetComponent<PhysicalDie>();

        
    }

    private void createPlayers()
    {
        for (int i = 0; i < PlayerCount; i++)
        {
            GameObject player = Instantiate(playerPrefab, _players);

            player.name = "Player" + (i+1).ToString();
            player.GetComponent<Player>().SetUpPlayer(i);
            player.SetActive(false);

            Players.Add(player);
        }
    }

    public void Buffer()
    {
        if (gameReady)
        {
            if (!win.activeSelf)
            buffer.SetActive(true);
            stackActions.SetActive(false);
        }
        else
        {
            NextPlayer();
        }
        
    }

    public void NextPlayer() 
    {
        buffer.SetActive(false);
        stackActions.SetActive(true);

        if (_currentPlayer < Players.Count - 1)
        {
            _currentPlayer++;
        }
        else
        {
            _currentPlayer = 0;
        }
        CurrentPlayer = Players[_currentPlayer].GetComponent<Player>();
        CurrentPlayer.StartOfTurn();
        UpdateGoldCards();
    }

    public int GetCurrentPlacedValue() { return _currentPlacedValue; }

    // Place cards from a player and update the _currentPlacedValue
    // Player must press to place until their value is high enough, otherwise they must skip
    public void PlaceCards(int value, List<GameObject> cards)
    {
        if (_currentPlacedValue == value)  // Reset pile
            _currentPlacedValue = 0;
        else if (_currentPlacedValue < value)
            _currentPlacedValue = value;

        _placedManager.PlaceCards(_currentPlacedValue, cards);

        UpdateGoldCards();
    }

    // Set the value of the player count (called when submit pressed)
    public void PlayerSetCount(Slider slider) { PlayerCount = (int)slider.value; }

    // Set the value of the dice (called when submit pressed)
    public void SetDice()
    {
        int[] die = new int[2];

        //if (dropdown.options[dropdown.value].text == "6, 6")
        //{
        //    die[0] = 6 ; die[1] = 6;
        //}
        //else if (dropdown.options[dropdown.value].text == "6, 4")
        //{
        //    die[0] = 6; die[1] = 4;
        //}
        die[0] = 6; die[1] = 6;

        diceRoll.GetComponent<DiceManager>().CreateDice(die);
    }

    public IEnumerator RollDice(System.Action<int> onResult)
    {
        diceRoll.gameObject.SetActive(true);

        // return _diceManager.RollDice(); OLD DICE ROLL

        _physDice.roll();

        yield return new WaitUntil(() => _physDice.rollFinished);

        int result = _physDice.GetResults();
        print("Dice result: " + result);
        _physDice.UpdateDiceUI();

        _physDice.rollFinished = false;

        onResult?.Invoke(result);
    }

    // When the player accepts their roll
    public void EndDiceRoll()
    {
        diceRoll.gameObject.SetActive(false);
        _physDice.ClearDice();
    }

    // When submit is pressed, create a new game based on number of players selected
    public void NewGame()
    {
        createPlayers();

        _currentPlayer = 0;

        _cardManager.GenerateCards();  // Generate pile of cards
        _cardManager.RandShuffle(800);  // Shuffle cards

        //SetDice();

        CurrentPlayer = Players[_currentPlayer].GetComponent<Player>();



        CurrentPlayer.StartOfTurn();

        //loops over all players
        for (int i = 0; i < PlayerCount; i++)
        {
            //grabs x amount of cards from the deck
            for (int j = 0; j < startingCards; j++)
            {
                _cardManager.ButtonPressedPull();
            }
            //incriments the player by 1
            CurrentPlayer.EndOfTurn();
        }


        stackActions.SetActive(true);
        diceRoll.SetActive(false);

        GameObject startingCard = _cardManager.PickStartingcard();
        startingCard.GetComponent<Card>().Flip();
        _placedManager.ResetPlaced(startingCard);

        UpdateGoldCards();
        
        gameReady = true;
    }

    public void UpdateGoldCards()
    {
        if (CurrentPlayer == null) return;

        foreach (GameObject cardObj in CurrentPlayer.Hand)
        {
            if (cardObj == null) continue;

            Card card = cardObj.GetComponent<Card>();
            if (card == null) continue;

            bool matchesTotal = card.GetNumber() == _currentPlacedValue;

            card.SetGold(matchesTotal);
        }
    }

    //if game is won
    public void Win()
    {
        win.SetActive(true);
        stackActions.SetActive(false);
        win.transform.GetChild(6).GetComponent<TMP_Text>().text = "Player " + (_currentPlayer + 1);
    }
}
