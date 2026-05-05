using TMPro;
using UnityEngine;

public class PullNumber : MonoBehaviour
{
    private TMP_Text text;
    [SerializeField] private PlacedManager placedManager;

    private void Start()
    {
        placedManager = GameObject.FindGameObjectWithTag("PlacedManager").GetComponent<PlacedManager>();
        text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (placedManager != null)
        {
            text.text = placedManager.placedValue.ToString();
        }
    }
}
