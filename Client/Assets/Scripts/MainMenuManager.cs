using Pombal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] IntroPuppet introPuppet = null;
    [SerializeField] InputManager inputManager = null;
    [SerializeField] GameObject pressAnyKeyMessage = null;

    private void OnEnable()
    {
        introPuppet.OnIntroComplete += LoadMainScene;
    }
    private void OnDisable()
    {
        introPuppet.OnIntroComplete -= LoadMainScene;
    }
    private void Update()
    {
        if (pressAnyKeyMessage.activeSelf)
        {
            if (inputManager.PressAnyButton())
            {
                introPuppet.KeyPressed = true;
            }
        }
    }
    private void LoadMainScene()
    {
        SceneManager.LoadScene("Game");
    }
}
