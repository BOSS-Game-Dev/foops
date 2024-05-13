using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform orientation;

    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private float maxSlope = 40f;

    [SerializeField]
    private float groundDrag = 3f;

    private readonly float maxRaycastDist = 1.1f;

    private Vector2 input;
    private Vector3 direction;

    private Rigidbody rb;

    private bool isGrounded;
    private RaycastHit slopeHit;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() { }

    private void FixedUpdate()
    {
        var onSlope = IsOnSlope();
        direction = orientation.forward * input.y + orientation.right * input.x;

        isGrounded =
            Physics.Raycast(transform.position, Vector3.down, maxRaycastDist, whatIsGround)
            || onSlope;

        if (onSlope)
        {
            var slopeDirection = Vector3.ProjectOnPlane(direction, slopeHit.normal);
            rb.AddForce(10f * movementSpeed * slopeDirection);
        }
        else
        {
            rb.AddForce(10f * movementSpeed * direction);
        }

        if (isGrounded)
            rb.drag = groundDrag;
        else 
            rb.drag = 0;

        SpeedControl();

        print(onSlope);
        rb.useGravity = !onSlope;
    }

    public void GetInput(InputAction.CallbackContext callbackContext)
    {
        input = callbackContext.ReadValue<Vector2>().normalized;
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            if (isGrounded)
                // Add jump force
                rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void SpeedControl()
    {
        //Only care about the player's velocity in the x-z plane (flat)
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (flatVelocity.magnitude > movementSpeed)
        {
            //Limit the player's velocity to the same direction, but at the max speed magnitude
            Vector3 limitedVelocity = flatVelocity.normalized * movementSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }

    private bool IsOnSlope()
    {
        if (
            Physics.Raycast(
                transform.position,
                Vector3.down,
                out slopeHit,
                maxRaycastDist + 0.1f,
                whatIsGround
            )
        )
        {
            var angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlope && angle != 0;
        }
        return false;
    }
}



