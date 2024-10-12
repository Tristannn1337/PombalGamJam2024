using Pombal;
using UnityEngine;

public class Human : MonoBehaviour
{
    [SerializeField] float maxHealth = 100;

    float currentHealth;

    [field: SerializeField] public AIPathfinding Pathfinding { get; private set; }
    FishController fish;

    public Transform FishTransform => fish != null ? fish.transform : null;
    public bool IsDead => currentHealth <= 0;

    private void Awake()
    {
        fish = FindObjectOfType<FishController>();
        currentHealth = maxHealth;
    }
    public void TakeDamage(float damageAmount)
    {
        currentHealth = Mathf.Max(0, currentHealth - damageAmount);
    }
}
