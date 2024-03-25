using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion;

    [SerializeField]
    private float explosionForce;

    [SerializeField]
    private float explosionRadius;

    private bool collided;

    void OnCollisionEnter(Collision col) 
    {
        if (!collided && !col.gameObject.CompareTag("Projectile")) {

            GameObject explosionObj = Instantiate(
                explosion, col.GetContact(0).point, Quaternion.identity
            );

            collided = true;

            DoExplosionForce();
            Destroy(explosionObj, 2f);
            Destroy(gameObject);
        }

    }

    void DoExplosionForce() 
    {
        Collider[] colliders = new Collider[10];

        int count = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, colliders);

        foreach (Collider c in colliders) 
        {
            if (c != null && c.TryGetComponent<Rigidbody>(out var colliderRb)) 
            {

            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
