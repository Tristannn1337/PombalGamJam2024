using Pombal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] HudPuppet hudPuppet;
    [SerializeField] float levelTime = 120f;
    [SerializeField, ReadOnly] float timer = 0f;
    [Space]
    [SerializeField] int trashObjective = 100;
    [SerializeField, ReadOnly] int currentTrash;

    public static bool WON = false;
    private void Awake()
    {
        timer = levelTime;
        currentTrash = 0;
    }
    private void OnEnable()
    {
        TrashYummy.OnTrashPickup += IncrementTrash;
    }
    private void OnDisable()
    {
        TrashYummy.OnTrashPickup -= IncrementTrash;
    }
    private void Update()
    {
        timer = Mathf.Max(0, timer - Time.deltaTime);
        hudPuppet.SecondsRemaining = Mathf.CeilToInt(timer);

        if(timer <= 0)
        {
            WON = currentTrash >= trashObjective;
            //SceneManager.LoadScene();
        }
    }
    public void IncrementTrash()
    {
        currentTrash++;
    }
}
