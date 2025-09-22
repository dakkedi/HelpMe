using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInputManager : MonoBehaviour
{
    private Camera mainCamera;
    public Vector3 mousePosition { get; private set; }
    public bool leftClickDown { get; private set; }

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        Vector2 mouseScreenPosition = context.ReadValue<Vector2>();

        // Create a 2D ray from the camera
        RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(mouseScreenPosition), Vector2.zero);

        if (hit.collider != null)
        {
            // Get the world coordinates from the hit point
            mousePosition = hit.point;
        }
        else
        {
            // If the raycast doesn't hit anything, return a non-valid position
            mousePosition = new Vector3(0, -9999f, 0); 
        }
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        // Debug.Log("OnLeftClick");
        // Den här metoden anropas när vänster musknapp klickas
        leftClickDown = context.performed;
    }
}