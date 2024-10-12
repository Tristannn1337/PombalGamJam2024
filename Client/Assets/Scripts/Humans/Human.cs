using Pombal;
using UnityEngine;

public class Human : MonoBehaviour
{

    [SerializeField] float maxHealth = 100;

    [SerializeField] Quadrant quadrant;
    [SerializeField, ReadOnly] float currentHealth;

    [field: SerializeField] public AIPathfinding Pathfinding { get; private set; }
    FishController fish;
    HidingSpots hidingSpots;

    public Transform FishTransform => fish != null ? fish.transform : null;
    public bool IsDead => currentHealth <= 0;

    private void Awake()
    {
        fish = FindObjectOfType<FishController>(true);
        hidingSpots = FindObjectOfType<HidingSpots>();
        currentHealth = maxHealth;

    }
    public void TakeDamage(float damageAmount)
    {
        currentHealth = Mathf.Max(0, currentHealth - damageAmount);
    }
    public Transform GetHidingSpot()
    {
        return hidingSpots.GetHidingSpot(quadrant);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out QuadrantTrigger quadrantTrigger))
        {
            quadrant = quadrantTrigger.Quadrant;
        }
    }
}
