namespace Pombal {
    using System.Collections;
    using UnityEngine;

    public enum FishMovementStates {
        Idling,
        Flopping
    }

    [RequireComponent(typeof(Rigidbody2D))]
    public class FishController : MonoBehaviour {

        [Header("Flop Settings")]
        [SerializeField] private float _flopDistance = 2f;
        [SerializeField] private float _flopDuration = .3f;
        [SerializeField] private float _flopCooldown = .1f;
        [SerializeField] private AnimationCurve _flopCurve;
        [SerializeField] private float _flopApexSizeMultiplier = 1.1f;


        [Header("References")]
        [SerializeField] private Transform _renderingTransform;
        private Rigidbody2D _rb;
        private Rigidbody2D Rb => _rb = _rb ?? GetComponent<Rigidbody2D>();

        [Header("Debugging")]
        [SerializeField, ReadOnly] private FishMovementStates FishMovementState;

        private Vector2 lastMoveInputDirection;

        private void OnEnable() {

            Pombal.InputManager.OnFlop += OnFlopInput;
            Pombal.InputManager.OnMove += OnMoveInput;

        }

        private void OnDisable() {
            Pombal.InputManager.OnFlop -= OnFlopInput;
            Pombal.InputManager.OnMove -= OnMoveInput;
        }

        private void Awake() {
        }

        private void Update() {
        }

        private void OnFlopInput() {

            if (FishMovementState == FishMovementStates.Idling) {
                StartCoroutine(Flopping());
            }
        }

        private void OnMoveInput(Vector2 input) {

            lastMoveInputDirection = input.normalized;
        }

        private IEnumerator Flopping() {

            FishMovementState = FishMovementStates.Flopping;

            float flopStartTime = Time.time;
            Vector2 flopDirection = lastMoveInputDirection;

            Vector2 startPosition = Rb.position;
            Vector2 targetPosition = startPosition + flopDirection * _flopDistance;
            Vector2 startRenderingTransformScale = _renderingTransform.localScale;
            //Vector2 startRenderingTransformRight = _renderingTransform.right;
            Quaternion startRenderingTransformRotation = _renderingTransform.rotation;
            Quaternion targetRotation = Quaternion.FromToRotation(startRenderingTransformRotation * Vector3.right, -flopDirection) * startRenderingTransformRotation;

            while (Time.time < flopStartTime + _flopDuration) {

                float t = (Time.time - flopStartTime) / _flopDuration;
                float flopCurveT = _flopCurve.Evaluate(t);
                float scaleT = flopCurveT <= 0.5f ? flopCurveT * 2 : (1 - flopCurveT) * 2;

                Rb.position = Vector2.Lerp(startPosition, targetPosition, flopCurveT);//lerp rb movement
                _renderingTransform.localScale = Vector2.Lerp(startRenderingTransformScale, startRenderingTransformScale * _flopApexSizeMultiplier, scaleT);//lerp rendering scale
                //_renderingTransform.right = Vector2.Lerp(startRenderingTransformRight, -flopDirection, flopCurveT);//Lerp rendering direction
                _renderingTransform.rotation = Quaternion.Slerp(startRenderingTransformRotation, targetRotation, flopCurveT);// lerp rendering rotation
                yield return null;
            }

            Rb.position = targetPosition;
            _renderingTransform.localScale = startRenderingTransformScale;
            //_renderingTransform.right = -flopDirection;
            _renderingTransform.rotation = targetRotation;

            yield return new WaitForSeconds(_flopCooldown);

            FishMovementState = FishMovementStates.Idling;
        }
    }
}
