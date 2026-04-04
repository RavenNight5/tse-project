using UnityEngine;
using UnityEngine.EventSystems;

public class DeckTap : MonoBehaviour
{
    [SerializeField] private int val = 12;
    [SerializeField] private string output = "pressed Deck";

    // Update is called once per frame
    void Update()
    {
        // Check if there's at least one touch input on the screen ....
        if (Input.touchCount > 0)
        {
            // Get the first touch event (useful if we're only using single-touch interactions).
            Touch touch = Input.GetTouch(0);
            // Check if the touch is on a UI element
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return; // Ignore the touch if it's on a UI element
            }
            // Check if the touch phase just began (indicating the player has just touched the screen).
            if (touch.phase == TouchPhase.Began)
            {
                print("OUT tapped");
                // Create a ray from the camera through the touch position on the screen.
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                // Perform a raycast to check if the touch intersects with any objects in the AR scene.
                if (Physics.Raycast(ray, out hit))
                {
                    // Get the GameObject that was hit by the raycast.
                    GameObject touchedObject = hit.collider.gameObject;

                    //if the touched object is the Deck
                    if (touchedObject.tag == "Stack")
                    {
                        //prints the output message
                        print(output);
                    }
                }
            }
        }
    }
}
