using Pombal;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{

    [SerializeField] float maxHealth = 100;

    [SerializeField] Quadrant quadrant;
    [SerializeField, ReadOnly] float currentHealth;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem bloodOnTheGroundVFX;
    [SerializeField] BubbleBox bubbleBoxPrefab;
    [SerializeField] List<string> runAwayLines;
    [SerializeField] List<AudioClip> audioClips;

    [field: SerializeField] public AIPathfinding Pathfinding { get; private set; }
    protected FishController fish;
    HidingSpots hidingSpots;
    AudioSource audioSource;

    public Transform FishTransform => fish != null ? fish.transform : null;
    public bool IsDead { get; private set; }

    private void Awake()
    {
        fish = FindObjectOfType<FishController>(true);
        hidingSpots = FindObjectOfType<HidingSpots>();
        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();

    }
    public void TakeDamage(float damageAmount)
    {
        currentHealth = Mathf.Max(0, currentHealth - damageAmount);
        if(currentHealth == 0)
        {
            if(deathParticles != null)
            {
                Instantiate(deathParticles, transform.position, Quaternion.identity);
            }
            if(bloodOnTheGroundVFX != null)
            {
                Instantiate(bloodOnTheGroundVFX, transform.position, Quaternion.identity);
            }
            IsDead = true;
        }
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
    private string GetShoutLine()
    {
        return runAwayLines[Random.Range(0, runAwayLines.Count)];
    }
    public void HumanReaction()
    {
        BubbleBox bubbleBox = Instantiate(bubbleBoxPrefab, transform.position, Quaternion.identity);
        bubbleBox.ActivateText(GetShoutLine(), transform);

        if(audioClips.Count > 0)
        {
            audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Count)]);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out QuadrantTrigger quadrantTrigger))
        {
            quadrant = quadrantTrigger.Quadrant;
        }
    }
}
