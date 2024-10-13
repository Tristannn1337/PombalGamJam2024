namespace Pombal {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using static Rewired.Controller;

    public class TrashYummy : MonoBehaviour {

        [SerializeField] private int _maxTrash = 15;
        [SerializeField, ReadOnly] private int _tummyTrash = 0;
        [SerializeField] private float _vomitingDuration = 3;
        [SerializeField] private float _vomitingWaitTime = .5f;
        [SerializeField] private LayerMask _trashLayerMask;
        [SerializeField] private RamboFishPuppet _fishPuppet;
        [SerializeField] private HudPuppet _hudPuppet;

        void Start() {

        }

        private void Eat() {
            _fishPuppet.Eat = true;
            AddTrash(1);
            _hudPuppet.PollutantsRemaining -= 1;
        }

        public void Hit(int damage) {
            _fishPuppet.Hit = true;
            AddTrash(-Mathf.Abs(damage));
        }

        //public void Die() {
        //    _fishPuppet.Dead = true;
        //}

        private void StartVomit() {
            _fishPuppet.Vomiting = true;
        }
        private void StopVomit() {
            _fishPuppet.Vomiting = false;
        }

        private void SetFullness(float percent) {
            _fishPuppet.Fullness = percent;
        }


        private void OnTriggerEnter2D(Collider2D collision) {
            if (((1 << collision.gameObject.layer) & _trashLayerMask) != 0) {
                Destroy(collision.gameObject);
                Eat();
            }
        }

        private void AddTrash(int value) {
            _tummyTrash = Mathf.Clamp(_tummyTrash += value, 0, _maxTrash);
            SetFullness((float)_tummyTrash / (float)_maxTrash);

            if (_tummyTrash >= _maxTrash) {
                StartCoroutine(Vomiting());
            }
        }

        private IEnumerator Vomiting() {

            yield return new WaitForSeconds(_vomitingWaitTime);
            StartVomit();
            float tummyTrashfloat = _tummyTrash;

            while (_tummyTrash > 0) {
                tummyTrashfloat -= (_maxTrash / _vomitingDuration * Time.deltaTime);
                _tummyTrash = (int)tummyTrashfloat;
                yield return null;
            }
            _tummyTrash = 0;
            SetFullness(0);
            StopVomit();
        }
    }
}
