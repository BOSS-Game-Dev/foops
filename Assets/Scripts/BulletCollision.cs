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

    // OnCollisionStay because we want to count for collisions while the bullet is IN another collider as well
    void OnCollisionEnter(Collision col)
    {
        if (!collided && !col.gameObject.CompareTag("Projectile"))
        {
            GameObject explosionObj = Instantiate(
                explosion,
                col.GetContact(0).point,
                Quaternion.identity
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
                // 1f represents the upwards modifier; how much should the explosion act on objects upwards
                colliderRb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1f, ForceMode.Impulse);
            }
        }
    }
}
