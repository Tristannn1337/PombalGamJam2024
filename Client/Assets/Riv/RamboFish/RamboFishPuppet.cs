using UnityEngine;
using Rive;

public class RamboFishPuppet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RiveTexture _riveTexture;
    
    [Header("State")]
    [Range(0, 1)] public float Fullness;
    public bool Vomiting;

    private SMINumber _riveFullness;
    private SMIBool _riveVomiting;

    private void Start()
    {
        var stateMachine = _riveTexture.stateMachine;
        _riveFullness = stateMachine.GetNumber("Fullness");
        _riveVomiting = stateMachine.GetBool("Vomiting");
    }
    
    private void Update()
    {
        _riveFullness.Value = Fullness * 100;
        _riveVomiting.Value = Vomiting;
    }

}
