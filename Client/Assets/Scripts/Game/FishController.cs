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
        [SerializeField] private Vector2 _randomRenderingRotation = new Vector2(20f, 60f);
        [SerializeField] private Vector2 _randomMovementRotation = new Vector2(5f, 15f);

        [Header("References")]
        [SerializeField] private Transform _renderingTransform;
        private Rigidbody2D _rb;
        private Rigidbody2D Rb => _rb = _rb ?? GetComponent<Rigidbody2D>();

        [Header("Debugging")]
        [SerializeField, ReadOnly] private FishMovementStates FishMovementState;

        private Vector2 _lastMoveInputDirection;
        private int _randomRotationPolarity = 1;

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

            _lastMoveInputDirection = input.normalized;
        }

        private IEnumerator Flopping() {

            FishMovementState = FishMovementStates.Flopping;

            float flopStartTime = Time.time;
            Vector2 flopDirection = _lastMoveInputDirection;

            //Get Random Rotation
            float randomRotationAddedInDeg = Random.Range(Mathf.Abs(_randomRenderingRotation.x), Mathf.Abs(_randomRenderingRotation.y));
            Quaternion randomAddedRotation = Quaternion.Euler(new Vector3(0, 0, randomRotationAddedInDeg * _randomRotationPolarity));

            //Get Random Direction
            float randomDirectionRotationAddedInDeg = Random.Range(_randomMovementRotation.x, _randomMovementRotation.y);
            randomDirectionRotationAddedInDeg *= _randomRotationPolarity;

            _randomRotationPolarity *= -1;
            //Save Transform data
            Vector2 startRenderingTransformScale = _renderingTransform.localScale;
            Quaternion startRenderingTransformRotation = _renderingTransform.rotation;

            //Add Random DIrection
            float flopDirectionAngle = Mathf.Atan2(flopDirection.y, flopDirection.x) * Mathf.Rad2Deg;
            float newDirectionAngle = flopDirectionAngle + randomDirectionRotationAddedInDeg;
            Vector2 adjustedFlopDirection = new Vector2(Mathf.Cos(newDirectionAngle * Mathf.Deg2Rad), Mathf.Sin(newDirectionAngle * Mathf.Deg2Rad)).normalized;

            //Set Target Position
            Vector2 startPosition = Rb.position;
            Vector2 targetPosition = startPosition + adjustedFlopDirection.normalized * _flopDistance;

            //Set Target Rotation
            Quaternion targetRotation = Quaternion.FromToRotation(startRenderingTransformRotation * Vector3.right, -flopDirection) * startRenderingTransformRotation;
            targetRotation *= randomAddedRotation;


            //Flop
            while (Time.time < flopStartTime + _flopDuration) {

                float t = (Time.time - flopStartTime) / _flopDuration;
                float flopCurveT = _flopCurve.Evaluate(t);
                float scaleT = flopCurveT <= 0.5f ? flopCurveT * 2 : (1 - flopCurveT) * 2;

                Rb.position = Vector2.Lerp(startPosition, targetPosition, flopCurveT);//lerp rb movement
                _renderingTransform.localScale = Vector2.Lerp(startRenderingTransformScale, startRenderingTransformScale * _flopApexSizeMultiplier, scaleT);//lerp rendering scale
                _renderingTransform.rotation = Quaternion.Slerp(startRenderingTransformRotation, targetRotation, flopCurveT);// lerp rendering rotation
                yield return null;
            }

            Rb.position = targetPosition;
            _renderingTransform.localScale = startRenderingTransformScale;
            _renderingTransform.rotation = targetRotation;

            yield return new WaitForSeconds(_flopCooldown);
            FishMovementState = FishMovementStates.Idling;
        }
    }
}
