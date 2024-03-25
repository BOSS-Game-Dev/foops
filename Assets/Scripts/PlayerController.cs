using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform orientation;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    
    const float maxRaycastDist = 1.1f;
    [SerializeField] private LayerMask whatIsGround;

    private Vector2 input;
    private Vector3 direction;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        direction = orientation.forward * input.y + orientation.right * input.x;
        rb.AddForce(direction * moveSpeed, ForceMode.Force);
    }

    public void GetKeyboardInput(InputAction.CallbackContext callbackContext)
    {
        input = callbackContext.ReadValue<Vector2>().normalized;
    }
    public void Jump (InputAction.CallbackContext callbackContext) {
        if (callbackContext.performed) {
            bool isGrounded = Physics.Raycast(transform.position, Vector3.down, maxRaycastDist, whatIsGround);
            if (isGrounded) {
                rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
    private void SpeedControl() {
        //Only care about the player's velocity in the xz-plane
        Vector3 flatVelocity = new Vector3(rb.velocity.x,0,rb.velocity.z);

        if (flatVelocity.magnitude > moveSpeed) {
            //Limit the player's velocity to the same direction and at the max speed
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVelocity.x,rb.velocity.y,limitedVelocity.z);
        }
    }
}