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
        [SerializeField] private AnimationCurve _bounceOffCurve;
        [SerializeField] private float _flopApexSizeMultiplier = 1.1f;
        [SerializeField] private Vector2 _randomRenderingRotation = new Vector2(20f, 60f);
        [SerializeField] private Vector2 _randomMovementRotation = new Vector2(5f, 15f);


        [Header("Collision Settings")]
        [SerializeField] private float _collisionRadius = 2f;
        [SerializeField] private Vector2 _collisionOffset = Vector2.zero;
        [SerializeField] private LayerMask _collisionMask;


        [Header("Audio")]
        [SerializeField] private AudioSource _slimeAudioSource;
        [SerializeField] private AudioSource _poingAudioSource;
        [SerializeField] private AudioSource _flopAudioSource;
        [SerializeField] private AudioClip[] _flopAudioClips;
        [SerializeField] private Vector2 _flopPitchRange;
        [SerializeField] private Vector2 _slimePitchRange;
        [SerializeField] private Vector2 _poingPitchRange;


        [Header("References")]
        [SerializeField] private Transform _fishTransform;
        [SerializeField] private Transform _ramboHolderTransform;
        private Rigidbody2D _rb;
        public Rigidbody2D Rb => _rb = _rb ?? GetComponent<Rigidbody2D>();
        private TrashYummy _trashYummy;
        private TrashYummy TrashYummy => _trashYummy = _trashYummy ?? GetComponent<TrashYummy>();

        [Header("Debugging")]
        [SerializeField, ReadOnly] private FishMovementStates FishMovementState;

        private Vector2 _lastMoveInputDirection;
        private int _randomRotationPolarity = 1;
        private Vector3 _ramboHolderStartScale;

        private void OnEnable() {

            //Pombal.InputManager.OnFlop += OnFlopInput;
            Pombal.InputManager.OnMove += OnMoveInput;

        }

        private void OnDisable() {
            //Pombal.InputManager.OnFlop -= OnFlopInput;
            Pombal.InputManager.OnMove -= OnMoveInput;
        }

        private void Awake() {
            _ramboHolderStartScale = _ramboHolderTransform.localScale;
            Rb.freezeRotation = true;
        }

        private void Update() {

            if (IsPointedRight(-transform.right))
            {
                _ramboHolderTransform.localRotation = Quaternion.Euler(180, 0, 0);
                //_ramboHolderTransform.localScale = new Vector3(_ramboHolderStartScale.x, _ramboHolderStartScale.y, 1);
            } else {
                _ramboHolderTransform.localRotation = Quaternion.Euler(0, 0, 0);
                //_ramboHolderTransform.localScale = new Vector3(_ramboHolderStartScale.x, _ramboHolderStartScale.y, 1);
            }

            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
        }

        public void Hit(int damage) {
            TrashYummy.Hit(damage);
        }

        //private void OnFlopInput() {

        //    if (FishMovementState == FishMovementStates.Idling) {
        //        StartCoroutine(Flopping());
        //    }
        //}

        private void OnMoveInput(Vector2 input) {

            _lastMoveInputDirection = input.normalized;
            if (FishMovementState == FishMovementStates.Idling) {
                StartCoroutine(Flopping());
            }
        }

        private IEnumerator Flopping() {

            PlayPoingAudioClip();
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
            Vector3 startRenderingTransformScale = transform.localScale;
            Quaternion startRenderingTransformRotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);

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
            targetRotation = Quaternion.Euler(0, 0, targetRotation.eulerAngles.z);

            //bool flopInterrupted = false;

            //Flop

            float t = 0;
            float flopDuration = _flopDuration;
            bool flopInterrupted = false;
            while (t < 1) {

                t = (Time.time - flopStartTime) / flopDuration;
                AnimationCurve selectedAnimationCurve = flopInterrupted ? _bounceOffCurve : _flopCurve;
                float CurveT = selectedAnimationCurve.Evaluate(t);
                float scaleT = CurveT <= 0.5f ? CurveT * 2 : (1 - CurveT) * 2;



                Rb.position = Vector2.Lerp(startPosition, targetPosition, CurveT);//lerp rb movement
                transform.localScale = Vector2.Lerp(startRenderingTransformScale, startRenderingTransformScale * _flopApexSizeMultiplier, scaleT);//lerp rendering scale
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
                transform.rotation = Quaternion.Slerp(startRenderingTransformRotation, targetRotation, CurveT);// lerp rendering rotation

                //Check for collision bounce
                Vector2 collisionNormal;
                if (CheckForCollision((Vector2)transform.position + (_collisionOffset * -(Vector2)transform.right), _collisionRadius, -transform.right, _collisionMask, out collisionNormal)) {

                    flopInterrupted = true;
                    flopDuration *= 1f;
                    flopStartTime = Time.time;
                    Vector2 newDirection = collisionNormal;
                    startPosition = transform.position;
                    targetPosition = (Vector2)transform.position + newDirection.normalized * _flopDistance * .5f;
                    targetRotation = Quaternion.FromToRotation(transform.rotation * Vector3.right, -newDirection) * transform.rotation;
                    targetRotation *= randomAddedRotation;

                }
                yield return null;
            }

            Rb.position = targetPosition;
            _fishTransform.localScale = startRenderingTransformScale;
            _fishTransform.rotation = targetRotation;
            PlayRandomFlopAudioClip();
            PlaySlimeAudioClip();

            yield return new WaitForSeconds(_flopCooldown);
            FishMovementState = FishMovementStates.Idling;
        }

        private bool CheckForCollision(Vector2 origin, float radius, Vector2 direction, LayerMask layerMask, out Vector2 collisionNormal) {

            RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, radius, direction, 0f, layerMask);


            RaycastHit2D closestHit = new RaycastHit2D();
            float closestDistance = float.MaxValue;

            foreach (RaycastHit2D hit in hits) {
                float distance = hit.distance;

                if (distance < closestDistance) {
                    closestDistance = distance;
                    closestHit = hit;
                }
            }


            if (closestHit.collider != null) {
                collisionNormal = closestHit.normal;
                //Debug.Log("Collision: " + closestHit.transform.gameObject.name + " Found Among " + hits.Length + " hits");
                return true;
            } else {
                collisionNormal = Vector2.zero;
                return false;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos() {
            // Draw the circle at the origin
            Gizmos.color = Color.red;
            Vector2 position = (Vector2)transform.position + (_collisionOffset.x * -(Vector2)transform.right);
            Gizmos.DrawWireSphere(position, _collisionRadius);
        }
#endif

        private bool IsPointedRight(Vector2 direction) {
            direction = direction.normalized;
            return Vector2.Dot(direction, Vector2.right) > Vector2.Dot(direction, Vector2.left);
        }

        public void PlayRandomFlopAudioClip() {
            if (_flopAudioClips.Length == 0) {
                Debug.LogWarning("No audio clips assigned!");
                return;
            }
            int randomIndex = Random.Range(0, _flopAudioClips.Length);
            AudioClip selectedClip = _flopAudioClips[randomIndex];
            _flopAudioSource.pitch = Random.Range(_flopPitchRange.x, _flopPitchRange.y);
            _flopAudioSource.PlayOneShot(selectedClip);
        }

        public void PlaySlimeAudioClip() {

            _slimeAudioSource.pitch = Random.Range(_slimePitchRange.x, _slimePitchRange.y);
            _slimeAudioSource.Play();
        }
        public void PlayPoingAudioClip() {

            _poingAudioSource.pitch = Random.Range(_poingPitchRange.x, _poingPitchRange.y);
            _poingAudioSource.Play();
        }
    }


}
