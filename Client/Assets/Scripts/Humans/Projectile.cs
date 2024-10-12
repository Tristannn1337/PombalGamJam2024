using Pombal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed;
    [SerializeField] Rigidbody2D rb;

    Vector2 direction;
    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
    }
    private void FixedUpdate()
    {
        rb.AddForce(direction * projectileSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out FishController fish))
        {
            Debug.Log("Deal Damage to Fish");
        }
        Destroy(gameObject);
    }

}
