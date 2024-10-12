using Pombal;
using UnityEngine;

public class Human : MonoBehaviour
{
    [field: SerializeField] public FishController Fish { get; private set; }
    [field: SerializeField] public AIPathfinding Pathfinding { get; private set; }

    private void Awake()
    {
        Fish = FindObjectOfType<FishController>();
    }
}
