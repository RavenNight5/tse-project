using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : NetworkBehaviour
{
    [SerializeField] private float speed = 1f;
    private Vector3 moveDir;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        moveDir = new Vector3(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        transform.position += moveDir * speed * Time.deltaTime;
    }
}
