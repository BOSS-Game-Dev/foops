using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform orientation;

    [SerializeField]
    private float mouseSens;

    private Vector2 mouseInput;
    private Vector2 camRot;
    // Start is called before the first frame update
    void Start()
    {
        // Hide the cursor and lock it to the center of the screen
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void LateUpdate()
    {
        // Rotating on x-axis -> down and up
        camRot.x -= mouseInput.y;
        // Rotating on y-axis -> right and left
        camRot.y += mouseInput.x;

        camRot.x = Mathf.Clamp(camRot.x, -90f, 90f);

        transform.rotation = Quaternion.Euler(camRot.x, camRot.y, 0);
        
        // Rotate the player's orientation
        orientation.rotation = Quaternion.Euler(0, camRot.y, 0);
    }

    // Method is called by the "Player Input" Component
    public void GetMouseInput(InputAction.CallbackContext callbackContext)
    {
        mouseInput = callbackContext.ReadValue<Vector2>() * 0.02f * mouseSens;
    }
}
