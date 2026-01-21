using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] public List<GameObject> Hand = new List<GameObject>();  // Holds the card objects
    [SerializeField] private Transform playerHand;  // Cards will be parented to this transform

    [SerializeField] private TMP_Text playerNumber;
    [SerializeField] private TMP_Text selectedTotal;
    [SerializeField] private Button rollButton;
    [SerializeField] private GameObject reRollButton;

    [SerializeField] private List<GameObject> selectedCards;  // List of the cards the player has selected in their hand

    private GameManager _gameManager;
    private int _selectedTotal;  // Total value of the cards (and dice if been rolled) the player has selected
    private int _totalRollValue;

    private int _rollsRemaining = 2;
    private bool _canRoll = true;

    private void Start()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void SetUpPlayer(int number)
    {
        playerNumber.SetText("Player " +  (number + 1).ToString());
    }

    public void AddCardToHand(GameObject card)
    {
        card.transform.SetParent(playerHand);
        Hand.Add(card);
    }

    public string CheckIfCanSelect(int value)
    {
        // If a large number is selected, don't allow more selections
        // If the value of the card is higher than the merging maximum and one or more cards are already selected return false
        // If the number of selected cards is at the limit, return false
        if (selectedCards.Count == 1) { if (selectedCards[0].GetComponent<Card>().GetNumber() > GameManager.MaxMergeValue) { return $"High value card already selected! Can only merge values 2 - {GameManager.MaxMergeValue}"; } }
        if (value > GameManager.MaxMergeValue && selectedCards.Count >= 1) { return $"High value cards cannot be merged! Merge cards of values 2 - {GameManager.MaxMergeValue}"; }
        if (selectedCards.Count >= GameManager.MaxCardMerges) { return $"A maximum of {GameManager.MaxCardMerges} cards can be merged"; }

        return null;
    }

    public void SelectedCard(int value, GameObject card)
    {
        _selectedTotal += value;
        selectedCards.Add(card);
        updateSelectedTotalText();
    }
    public void DeselectCard(int value, GameObject card)
    {
        if (value != -1) { _selectedTotal -= value; selectedCards.Remove(card); }
        else { _selectedTotal = 0; }  // Deselect all cards

        updateSelectedTotalText();
    }

    // Rolling
    public void RollDice()
    {
        if (_canRoll)
        {
            if (_rollsRemaining > 1) { _totalRollValue = _gameManager.RollDice(); }
            else if (_rollsRemaining == 1) { reRollButton.SetActive(false); }

            _rollsRemaining--;
        }
        else { Debug.Log("No more rolls left"); reRollButton.SetActive(false); }
    }
    public void AcceptRoll()
    {
        _canRoll = false;
        _selectedTotal += _totalRollValue;
        reRollButton.SetActive(false);

        _gameManager.EndDiceRoll();
        updateSelectedTotalText();
    }

    // Placing
    public void Place()
    {
        // Check hand can be placed

        if (_gameManager.GetCurrentPlacedValue() <= _selectedTotal)
        {
            _gameManager.PlaceCards(_selectedTotal, selectedCards);

            // Remove the selected cards from the player's hand
            foreach (var card in selectedCards)
            {
                Hand.Remove(card);
            }

            EndOfTurn();
        }
        else
        {
            Debug.Log($"Total is too small! Must be equal to {_gameManager.GetCurrentPlacedValue()} or higher");
        }
    }


    // Skipping/end of turn
    public void EndOfTurn()
    {
        selectedCards.Clear();

        _totalRollValue = 0;
        _selectedTotal = 0;
        updateSelectedTotalText();

        gameObject.SetActive(false);

        _gameManager.NextPlayer();
    }

    public void StartOfTurn()
    {
        gameObject.SetActive(true);
        rollButton.interactable = true;
        _rollsRemaining = 2;
        _canRoll = true;
    }


    private void updateSelectedTotalText()
    {
        selectedTotal.SetText(_selectedTotal.ToString());
    }
}
