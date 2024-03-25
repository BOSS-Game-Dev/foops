using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Transform orientation;

    private Vector2 mouseInput;
    private Vector2 camRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    // LateUpdate is called after Update and FixedUpdate
    void LateUpdate()
    {
        camRotation.x -= mouseInput.y;
        camRotation.y += mouseInput.x;

        camRotation.x = Mathf.Clamp(camRotation.x, -90f, 90f);

        transform.rotation = Quaternion.Euler(camRotation.x, camRotation.y, 0);
        orientation.rotation = Quaternion.Euler(0, camRotation.y, 0);
    }

    public void GetMouseInput(InputAction.CallbackContext callbackContext) 
    {
        mouseInput = callbackContext.ReadValue<Vector2>() * mouseSensitivity * 0.02f;
    }
}
