using Pombal;
using UnityEngine;

public class Human : MonoBehaviour
{

    [SerializeField] float maxHealth = 100;

    [SerializeField] Quadrant quadrant;
    [SerializeField, ReadOnly] float currentHealth;
    [SerializeField] ParticleSystem deathParticles;

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
    public void TurnTowardsFish()
    {
        Vector2 fishDirection = (FishTransform.position - transform.position).normalized;
        transform.right = fishDirection;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out QuadrantTrigger quadrantTrigger))
        {
            quadrant = quadrantTrigger.Quadrant;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        Instantiate(deathParticles, transform.position, Quaternion.identity);
    }
}
