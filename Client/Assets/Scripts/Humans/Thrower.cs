using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : Human
{
    [field: SerializeField] public float FireRate { get; private set; }
    [SerializeField] Projectile projectile = null;
    [SerializeField] Transform firePoint = null;

    public void ShootAtPlayer()
    {
        Vector2 shootDirection = (FishTransform.position - transform.position).normalized;
        transform.right = shootDirection;
        Instantiate(projectile, firePoint.position, Quaternion.identity).SetDirection(shootDirection);
    }
}
