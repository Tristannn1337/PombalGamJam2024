using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class HudPuppet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _secondsRemainingRoot;   
    [SerializeField] private Transform _pollutantsRemainingRoot;   
    
    [Header("Settings")]
    public float SecondsRemaining;
    public int PollutantsRemaining;

    private List<TextMeshProUGUI> _secondsRemainingLabels = new();
    private List<TextMeshProUGUI> _pollutantsRemainingLabels = new();

    private float _lastSecondsRemaining;
    private float _lastPollutantsRemaining;
    
    private void Start()
    {
        _secondsRemainingLabels = _secondsRemainingRoot.GetComponentsInChildren<TextMeshProUGUI>().ToList();
        _pollutantsRemainingLabels = _pollutantsRemainingRoot.GetComponentsInChildren<TextMeshProUGUI>().ToList();
    }

    private void Update()
    {
        if (_lastSecondsRemaining != SecondsRemaining)
        {
            _lastSecondsRemaining = SecondsRemaining;
            var minutes = (int)SecondsRemaining / 60;
            var seconds = (int)SecondsRemaining % 60;
            var strLabel = $"{minutes:D2}:{seconds:D2}";
            foreach (var label in _secondsRemainingLabels)
                label.text = strLabel;
        }
        
        if (_lastPollutantsRemaining != PollutantsRemaining)
        {
            _lastPollutantsRemaining = PollutantsRemaining;
            var strLabel = $"{PollutantsRemaining}";
            foreach (var label in _pollutantsRemainingLabels)
                label.text = strLabel;
        }
    }
}
