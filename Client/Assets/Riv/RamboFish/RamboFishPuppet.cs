using DG.Tweening;
using UnityEngine;
using Rive;
using Color = UnityEngine.Color;

public class RamboFishPuppet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RiveTexture _riveTexture;
    
    [Header("State")]
    [Range(0, 1)] public float Fullness;
    public bool Vomiting;
    public bool Eat;
    public bool Dead;
    public bool Hit;

    private SMINumber _riveFullness;
    private SMIBool _riveVomiting;
    private SMITrigger _riveEat;
    private SMIBool _riveDead;
    private SMITrigger _riveHit;

    private void Start()
    {
        var stateMachine = _riveTexture.stateMachine;
        _riveFullness = stateMachine.GetNumber("Fullness");
        _riveVomiting = stateMachine.GetBool("Vomiting");
        _riveEat = stateMachine.GetTrigger("Eat");
        _riveDead = stateMachine.GetBool("Dead");
        _riveHit = stateMachine.GetTrigger("Hit");
    }
    
    private void Update()
    {
        _riveFullness.Value = Fullness * 100;
        _riveVomiting.Value = Vomiting;
        _riveDead.Value = Dead;
        
        if (Eat)
        {
            Eat = false;
            _riveEat.Fire();
            var mat = _riveTexture.gameObject.GetComponent<MeshRenderer>().material;
            mat.color = Color.white;
            mat.DOColor(Color.black, 0.1f);
        }
        
        if (Hit)
        {
            Hit = false;
            //_riveHit.Fire();
            var mat = _riveTexture.gameObject.GetComponent<MeshRenderer>().material;
            mat.color = Color.red;
            mat.DOColor(Color.black, 0.2f);
        }
    }

}
