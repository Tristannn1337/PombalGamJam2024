using Pombal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplaySceneManager : MonoBehaviour
{
    [SerializeField] GameObject winScreen = null;
    [SerializeField] GameObject loseScreen = null;
     InputManager inputManager;

    float lockTimer = 0;
    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
    }
    private void Start()
    {
        if (LevelManager.WON)
        {
            winScreen.SetActive(true);
            loseScreen.SetActive(false);
        }
        else
        {
            loseScreen.SetActive(true);
            winScreen.SetActive(false);
        }
    }
    private void Update()
    {
        lockTimer += Time.deltaTime;
        if(lockTimer >= 2f)
        {
            if (inputManager.PressAnyButton())
            {
                SceneManager.LoadScene("Game");
            }
        }
    }
}
