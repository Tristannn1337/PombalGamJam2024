namespace Pombal {
    using UnityEngine;

    public enum FishMovementStates {
        Idling,
        Flopping
    }

    [RequireComponent(typeof(Rigidbody2D))]
    public class FishController : MonoBehaviour {

        [Header("Debugging")]
        [SerializeField, ReadOnly] private FishMovementStates FishMovementState;


        private Rigidbody2D _rb;
        private Rigidbody2D Rb => _rb = _rb ?? GetComponent<Rigidbody2D>();


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

            Debug.Log("Flop");
        }

        private void OnMoveInput(Vector2 input) {

            Debug.Log("Move: " + input);
        }
    }
}
