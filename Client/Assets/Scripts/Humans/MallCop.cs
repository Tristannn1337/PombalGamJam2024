using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MallCop : Human
{
    [SerializeField] LayerMask fishLayer = default;
    [SerializeField] Transform damageCenter = null;
    [SerializeField] Vector2 damageSize;
    [SerializeField] float taserForce = 1000f;
    [Space]
    [SerializeField] UnityEvent OnTase;

    public void TaseFish()
    {
        OnTase?.Invoke();

        RaycastHit2D hit = Physics2D.BoxCast(damageCenter.position, damageSize, 0f, Vector2.up, 0f, fishLayer);
        if(hit.transform != null)
        {
            Vector2 forceDirection = (FishTransform.position - transform.position).normalized;
            fish.Rb.AddForceAtPosition(forceDirection * taserForce, fish.transform.position, ForceMode2D.Force);
        }
    }
    private void OnDrawGizmos()
    {
        if(damageCenter != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(damageCenter.position, damageSize);
        }
    }
}
