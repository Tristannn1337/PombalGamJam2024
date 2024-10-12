namespace Pombal {
    using Rewired;
    using UnityEngine;

    public class InputManager : Singleton<InputManager> {

        private int playerId = 0;
        private Rewired.Player player;

        // Actions
        private const string FlopAction = "Flop";
        private const string MoveHorizontalAction = "MoveHorizontal";
        private const string MoveVerticallAction = "MoveVertical";

        public delegate void InputButtonAction();
        public static event InputButtonAction OnFlop;

        public delegate void InputAxisAction(Vector2 input);
        public static event InputAxisAction OnMove;

        protected override void Awake() {
            base.Awake();
            player = ReInput.players.GetPlayer(playerId);
        }

        private void Update() {
            HandleInput();
        }

        private void HandleInput() {
            //Flop Input
            if (player.GetButtonDown(FlopAction)) { OnFlop?.Invoke(); Debug.Log("Flop"); }//Called on first button press 
            //Movement Input
            Vector2 movementInput = new Vector2(player.GetAxis(MoveHorizontalAction), player.GetAxis(MoveVerticallAction));
            if (movementInput != Vector2.zero) { OnMove?.Invoke(movementInput); Debug.Log("Move"); }//Called when not 0
        }
    }
}
