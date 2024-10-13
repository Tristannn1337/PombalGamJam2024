using UnityEngine;
using UnityEngine.UI;

public class VomitMeterPuppet : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Image _fillImage;
    [SerializeField] private Image _scrollingImage;
    [SerializeField] private Transform _fishTransform;
    [SerializeField] private Vector2 _offsetPosition;
    [SerializeField] private float rotationLimit;
    private Quaternion _initialRotation;

    [Header("Settings")]
    [Range(0, 1)] public float FillAmount;
    public bool ShowScrollingVomit;
    public Color LowColor;
    public Color MidColor;
    public Color FullColor;

    private void Update() {
        _fillImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, FillAmount * 0.825f);
        _scrollingImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, FillAmount * 1.31f);

        _fillImage.color = Color.Lerp(Color.Lerp(LowColor, MidColor, Mathf.Clamp01(FillAmount / 0.5f)), FullColor, Mathf.Clamp01(FillAmount - 0.5f) / 0.5f);
        _scrollingImage.enabled = ShowScrollingVomit;


        HandlePosition();
        HandleRotation();
    }

    private void HandlePosition() {

        Vector2 targetOffset;
        transform.position = (Vector2)_fishTransform.position + _offsetPosition;

        if (IsPointingDownMoreThanOtherDirections(-_fishTransform.right)) { targetOffset = new Vector2(_offsetPosition.x, -_offsetPosition.y); } else { targetOffset = _offsetPosition; }

        transform.position = (Vector2)_fishTransform.position + targetOffset;

    }

    private void HandleRotation() {
        Quaternion currentRotation = transform.rotation;

        Quaternion minRotation = Quaternion.Euler(
            _initialRotation.eulerAngles.x,
            _initialRotation.eulerAngles.y,
            _initialRotation.eulerAngles.z - rotationLimit
        );

        Quaternion maxRotation = Quaternion.Euler(
            _initialRotation.eulerAngles.x,
            _initialRotation.eulerAngles.y,
            _initialRotation.eulerAngles.z + rotationLimit
        );

        float clampedZRotation = Mathf.Clamp(currentRotation.eulerAngles.z, minRotation.eulerAngles.z, maxRotation.eulerAngles.z);
        transform.rotation = Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y, clampedZRotation);
    }


    private bool IsPointingDownMoreThanOtherDirections(Vector2 vector) {
        // Check if the vector is pointing down more than left, right, or up
        //return vector.y < 0 && Mathf.Abs(vector.y) > Mathf.Abs(vector.x) && Mathf.Abs(vector.y) > 0;
        return vector.y < -.4f;
    }
}
