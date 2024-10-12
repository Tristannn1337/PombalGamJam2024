using UnityEngine;

public class Human : MonoBehaviour
{
    [field: SerializeField] public AIPathfinding Pathfinding { get; private set; }
    [field: SerializeField] public Transform Target { get; set; }
    [field: SerializeField] public float MovementSpeed { get; private set; }
}
