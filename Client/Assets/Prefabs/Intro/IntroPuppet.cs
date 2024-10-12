using System;
using UnityEngine;
using UnityEngine.Playables;

public class IntroPuppet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayableDirector _playableDirector;
    
    [Header("Settings")]
    public bool KeyPressed;
    public Action OnIntroComplete;
    
    public void OnComplete()
    {
        Debug.Log("Intro Complete");
        OnIntroComplete?.Invoke();
    }
}
