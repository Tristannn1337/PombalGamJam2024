namespace Pombal {
    using UnityEngine;

    public class TrashManager : MonoBehaviour {

        [SerializeField] HudPuppet _hudPuppet;
        private void Awake() {
            int childCount = transform.childCount;
            _hudPuppet.PollutantsRemaining = childCount;
        }

    }
}
