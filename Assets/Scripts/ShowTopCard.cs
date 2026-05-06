using TMPro;
using UnityEngine;

public class ShowTopCard : MonoBehaviour
{
    private TMP_Text text;
    private PlacedManager placedManager;

    private void Start()
    {
        placedManager = GameObject.FindGameObjectWithTag("PlacedManager").GetComponent<PlacedManager>();
        text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        text.text = "Top Card: " + placedManager.GetTopCard();
    }
}
