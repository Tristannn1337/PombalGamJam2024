using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MallCop : Human
{
    [SerializeField] LayerMask fishLayer = default;
    [SerializeField] float damage = 10f;
    [SerializeField] Transform damageCenter = null;
    [SerializeField] Vector2 damageSize;
    public void TaseFish()
    {
        RaycastHit2D hit = Physics2D.BoxCast(damageCenter.position, damageSize, 0f, Vector2.up, 0f, fishLayer);
        if(hit.transform != null)
        {
            Debug.Log("Tase Fish");
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
