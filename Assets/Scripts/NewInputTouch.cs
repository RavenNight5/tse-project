using UnityEngine;
using UnityEngine.InputSystem;

public class NewInputTouch : MonoBehaviour
{
    bool justPressed = false;
    [SerializeField] private CardManager stackManager;


    // Update is called once per frame
    void Update()
    {
        if (Touchscreen.current != null)
        {
            foreach (var touch in Touchscreen.current.touches)
            {
                if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began && !justPressed)
                {
                    justPressed = true;
                    Debug.Log("Just Touched");
                }
                if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended && justPressed)
                {
                    justPressed = false;
                    Debug.Log("Just Released");

                    Ray ray = Camera.main.ScreenPointToRay(touch.ReadValue().position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        GameObject obj = hit.collider.gameObject;
                        if (obj != null)
                        {
                            print("hit something");

                            if (obj.tag == "ARStack")
                            {
                                print("hit stack");
                                stackManager.ButtonPressedPull();
                            }
                        }
                    }


                }
            }
        }
    }
}
