using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using UnityEngine.EventSystems;

// The ObjectManager class manages the placement and removal of objects in an AR scene.
// It allows players to place an object by touching a detected plane and remove an object by touching it again.
public class ObjectManager : MonoBehaviour
{
    // The prefab of the object we want to place in the AR scene.
    public GameObject objectToPlace;

    // Reference to the ARRaycastManager component, which handles raycasting onto AR planes.
    public ARRaycastManager raycastManager;

    // A list that stores all the objects currently placed in the AR scene.
    private List<GameObject> spawnedObjects = new List<GameObject>();

    // Update is called once per frame.
    // Here we detect touch inputs and handle object placement and removal based on touch interactions.
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
                // Create a ray from the camera through the touch position on the screen.
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                // Perform a raycast to check if the touch intersects with any objects in the AR scene.
                if (Physics.Raycast(ray, out hit))
                {
                    // Get the GameObject that was hit by the raycast.
                    GameObject touchedObject = hit.collider.gameObject;

                    // Check if the touched object is in our list of spawned objects.
                    if (spawnedObjects.Contains(touchedObject))
                    {
                        // If it is, remove it from the scene and from our list.
                        spawnedObjects.Remove(touchedObject);
                        Destroy(touchedObject);
                        return; // Exit to prevent placing a new object in the same frame.
                    }
                }

                // If the touch phase just began and no existing object was hit, place a new object.
                // Create a list to store any AR raycast hits (points where the ray hits an AR plane).
                List<ARRaycastHit> hits = new List<ARRaycastHit>();

                // Perform an AR raycast from the touch position. This will detect if the touch is on a plane.
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    // If the raycast hit a plane, get the pose (position and rotation) of the hit.
                    Pose hitPose = hits[0].pose;

                    // Instantiate the object at the hit location on the plane.
                    GameObject newObject = Instantiate(objectToPlace, hitPose.position, hitPose.rotation);

                    // Add the newly placed object to our list of spawned objects.
                    spawnedObjects.Add(newObject);

                }
            }
        }
    }

    // This method allows you to switch the prefab to be placed.
    public void SwitchPrefab(GameObject go)
    {
        objectToPlace = go;
    }
}





