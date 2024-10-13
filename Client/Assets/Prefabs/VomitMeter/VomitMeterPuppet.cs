using UnityEngine;
using UnityEngine.UI;

public class VomitMeterPuppet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image _fillImage;
    [SerializeField] private Image _scrollingImage;

    [Header("Settings")]
    [Range(0, 1)] public float FillAmount;
    public bool ShowScrollingVomit;
    public Color LowColor;
    public Color MidColor;
    public Color FullColor;
    
    private void Update()
    {
        _fillImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, FillAmount * 0.825f);
        _scrollingImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, FillAmount * 1.31f);
        
        _fillImage.color = Color.Lerp(Color.Lerp(LowColor, MidColor, Mathf.Clamp01(FillAmount / 0.5f)), FullColor, Mathf.Clamp01(FillAmount - 0.5f) / 0.5f);
        _scrollingImage.enabled = ShowScrollingVomit;
    }
}
