using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNumber;

    // Set the value of the text label (called on slider value changed)
    public void PlayerCountChange(Slider slider) { playerNumber.SetText(((int)slider.value).ToString()); }
}
