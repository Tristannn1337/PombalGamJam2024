using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadrantTrigger : MonoBehaviour
{
    [SerializeField] Quadrant quadrant = default;

    public Quadrant Quadrant => quadrant;
}
