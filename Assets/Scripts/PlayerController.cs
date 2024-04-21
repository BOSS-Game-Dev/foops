using System.Collections;
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
    private float groundDrag = 3f;

    [SerializeField]
    private float maxSlope = 40;

    private readonly float maxRaycastDist = 1.1f;

    private Vector2 input;

    private Rigidbody rb;

    private bool isGrounded;

    private RaycastHit slopeHit;
    private bool moveToSlope;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded =
            Physics.Raycast(transform.position, Vector3.down, maxRaycastDist, whatIsGround)
            || OnSlope();
    }

    private void FixedUpdate()
    {
        Move();

        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        SpeedControl();
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
            {
                // Add jump force
                rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
                StartCoroutine(FallToGround());
            }
        }
    }

    // In the future if we want to add falling sfx
    private IEnumerator FallToGround()
    {
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => isGrounded);
    }

    private void Move()
    {
        Vector3 direction = orientation.forward * input.y + orientation.right * input.x;

        if (OnSlope())
        {
            Vector3 slopeDirection = Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
            rb.AddForce(10f * movementSpeed * slopeDirection, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(direction * movementSpeed * 10f, ForceMode.Acceleration);
        }

        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        if (OnSlope())
        {
            if (rb.velocity.sqrMagnitude > movementSpeed * movementSpeed)
            {
                rb.velocity = rb.velocity.normalized * movementSpeed;
            }
        }
        else
        {
            // Only care about the player's velocity in the x-z plane (flat)
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            if (flatVelocity.magnitude > movementSpeed)
            {
                // Limit the player's velocity to the same direction, but at the max speed magnitude
                Vector3 limitedVelocity = flatVelocity.normalized * movementSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, maxRaycastDist + 0.1f))
        {
            // The angle between two vectors: a vector that points straight up & a vector that points perpendicular to the surface of the raycast
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlope && angle != 0;
        }

        return false;
    }
}
