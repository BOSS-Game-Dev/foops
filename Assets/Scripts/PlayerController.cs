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
        rb.AddForce(direction * movementSpeed);
    }

    public void GetInput(InputAction.CallbackContext callbackContext) 
    {
        input = callbackContext.ReadValue<Vector2>().normalized;
    }
}
