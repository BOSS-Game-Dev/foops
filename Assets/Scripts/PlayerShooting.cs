using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField]
    private Transform shootPosition;    

    [SerializeField]

    private GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(InputAction.CallbackContext callbackContext) {
        if (callbackContext.performed) {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            
            Vector3 destination; 
            if (Physics.Raycast(ray, out var hit)) {
                destination = hit.point;
            } else {
                destination = ray.GetPoint(1000);
            }

            GameObject bulletObject = Instantiate(bullet, shootPosition.position, Quaternion.identity);
            Vector3 shootDirection = (destination - shootPosition.position).normalized;

            bulletObject.GetComponent<Rigidbody>().velocity = shootDirection * 100f;

            Destroy(bulletObject, 2f);            
        }
    }
}
