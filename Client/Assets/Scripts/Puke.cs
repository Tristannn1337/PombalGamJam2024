using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puke : MonoBehaviour
{
    [SerializeField] float damage = 0.1f;

    private void OnParticleCollision(GameObject other)
    {
        if(other.TryGetComponent(out Human human))
        {
            human.TakeDamage(damage);
        }
    }
}
