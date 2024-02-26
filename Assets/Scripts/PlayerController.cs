using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private Transform orientation;

    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private float jumpForce;

    private Vector2 input;
    private Rigidbody rb;

    private bool grounded;
    private float maxDist;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        maxDist = GetComponent<CapsuleCollider>().height * 0.5f + 0.2f;
    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, maxDist, whatIsGround);
        Debug.Log(grounded);
    }

    void FixedUpdate()
    {
        Move();
        SpeedControl();
    }

    // Method is called by the "Player Input" Component
    public void GetInput(InputAction.CallbackContext callbackContext)
    {
        input = callbackContext.ReadValue<Vector2>();
    }

    // Method is called by the "Player Input" Component
    public void Jump(InputAction.CallbackContext callbackContext)
    {
        // When the action is just performed (downstroke of the key)
        if (callbackContext.performed)
        {
            if (!grounded) return;

            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void Move()
    {
        // Input on y-axis is how much we move on our relative z-axis (forward & backwards)
        // Input on x-axis is how much we move on relative x-axis (right & left)
        Vector3 moveDirection = orientation.forward * input.y + orientation.right * input.x;
        rb.AddForce(moveDirection * moveSpeed, ForceMode.Acceleration);
    }

    // This method is necessary, otherwise the player will keep on accelerating
    public void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }

}
