namespace Pombal {
    using System.Collections;
    using UnityEngine;

    public class TrashYummy : MonoBehaviour {

        [SerializeField] private int _maxTrash = 15;
        [SerializeField, ReadOnly] private int _tummyTrash = 0;
        [SerializeField] private float _vomitingDuration = 3;
        [SerializeField] private float _vomitingWaitTime = .3f;
        [SerializeField] private LayerMask _trashLayerMask;
        [SerializeField] private RamboFishPuppet _fishPuppet;
        [SerializeField] private HudPuppet _hudPuppet;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private GameObject _PukeParticles;
        [SerializeField] private AudioSource _pukeSound;
        [SerializeField] private VomitMeterPuppet _vomitMeter;
        private bool _vomiting;


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
            _cameraController.SwitchToHitCamera(.2f);
        }

        //public void Die() {
        //    _fishPuppet.Dead = true;
        //}

        private void StartVomit() {
            _fishPuppet.Vomiting = true;
            _PukeParticles.SetActive(true);
        }
        private void StopVomit() {
            _fishPuppet.Vomiting = false;
            _PukeParticles.SetActive(false);
            _pukeSound.Stop();
        }

        private void SetFullness(float percent) {
            _fishPuppet.Fullness = percent;
            _vomitMeter.FillAmount = percent;
        }


        private void OnTriggerEnter2D(Collider2D collision) {

            if (!_vomiting) {
                if (((1 << collision.gameObject.layer) & _trashLayerMask) != 0) {
                    Destroy(collision.gameObject);
                    Eat();
                }
            }
        }

        private void AddTrash(int value) {
            _tummyTrash = Mathf.Clamp(_tummyTrash += value, 0, _maxTrash);
            SetFullness((float)_tummyTrash / (float)_maxTrash);

            if (!_vomiting) {
                if (_tummyTrash >= _maxTrash) {
                    StartCoroutine(Vomiting());
                    _vomiting = true;
                }
            }
        }

        private IEnumerator Vomiting() {

            _vomitMeter.ShowScrollingVomit = true;
            _cameraController.SwitchToVomitCamera(Mathf.Clamp(_vomitingDuration - 1f, 0, 100f));
            yield return new WaitForSeconds(_vomitingWaitTime);

            StartVomit();
            _pukeSound.Play();
            float tummyTrashfloat = _tummyTrash;

            while (_tummyTrash > 0) {
                tummyTrashfloat -= (_maxTrash / _vomitingDuration * Time.deltaTime);
                _tummyTrash = (int)tummyTrashfloat;
                SetFullness((float)_tummyTrash / (float)_maxTrash);
                yield return null;
            }
            _tummyTrash = 0;
            SetFullness(0);
            _vomitMeter.ShowScrollingVomit = false;
            StopVomit();
            _vomiting = false;
        }
    }
}
