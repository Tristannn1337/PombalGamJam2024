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

        [Header("Collision Settings")]
        [SerializeField] private float _collisionRadius = 2f;
        [SerializeField] private Vector2 _collisionOffset = Vector2.zero;
        [SerializeField] private LayerMask _collisionMask;



        [Header("References")]
        [SerializeField] private Transform _fishTransform;
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
            Vector2 startRenderingTransformScale = transform.localScale;
            Quaternion startRenderingTransformRotation = transform.rotation;

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

            //bool flopInterrupted = false;

            //Flop

            float t = 0;
            float flopDuration = _flopDuration;
            while (t < 1) {

                t = (Time.time - flopStartTime) / flopDuration;

                float flopCurveT = _flopCurve.Evaluate(t);
                float scaleT = flopCurveT <= 0.5f ? flopCurveT * 2 : (1 - flopCurveT) * 2;


                Rb.position = Vector2.Lerp(startPosition, targetPosition, flopCurveT);//lerp rb movement
                transform.localScale = Vector2.Lerp(startRenderingTransformScale, startRenderingTransformScale * _flopApexSizeMultiplier, scaleT);//lerp rendering scale
                transform.rotation = Quaternion.Slerp(startRenderingTransformRotation, targetRotation, flopCurveT);// lerp rendering rotation

                //Check for collision bounce
                Vector2 collisionNormal;
                if (CheckForCollision((Vector2)transform.position + (_collisionOffset * -(Vector2)transform.right), _collisionRadius, -transform.right, _collisionMask, out collisionNormal)) {



                    float remainingT = Mathf.Max(0, 1 - t);
                    Debug.Log("Collided, remaining T: " + remainingT);
                    //ResetT
                    flopDuration *= remainingT;
                    flopStartTime = Time.time;

                    //ResetDirection
                    Vector2 newDirection = collisionNormal;
                    targetPosition = (Vector2)transform.position + newDirection.normalized * _flopDistance * remainingT;

                    transform.right = -newDirection;

                    targetRotation = Quaternion.FromToRotation(transform.rotation * Vector3.right, -newDirection) * transform.rotation;
                    targetRotation *= randomAddedRotation;

                    //flopInterrupted = true;

                }
                yield return null;
            }

            Rb.position = targetPosition;
            _fishTransform.localScale = startRenderingTransformScale;
            _fishTransform.rotation = targetRotation;


            yield return new WaitForSeconds(_flopCooldown);
            FishMovementState = FishMovementStates.Idling;
        }

        private bool CheckForCollision(Vector2 origin, float radius, Vector2 direction, LayerMask layerMask, out Vector2 collisionNormal) {

            RaycastHit2D hit = Physics2D.CircleCast(origin, radius, direction, 0f, layerMask);

            if (hit.collider != null) {
                collisionNormal = hit.normal;
                Debug.Log(hit.transform.gameObject.name);
                return true;
            } else {
                collisionNormal = Vector2.zero;
                return false;
            }
        }

        private void OnDrawGizmos() {
            // Draw the circle at the origin
            Gizmos.color = Color.red;
            Vector2 position = (Vector2)transform.position + (_collisionOffset.x * -(Vector2)transform.right);
            Gizmos.DrawWireSphere(position, _collisionRadius);
        }
    }
}
