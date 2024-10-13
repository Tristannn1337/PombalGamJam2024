namespace Pombal {
    using UnityEngine;
    using Cinemachine;
    using System.Collections;

    public class CameraController : MonoBehaviour {

        [SerializeField] private Transform target;
        [SerializeField] private CinemachineVirtualCamera _camIdle, _camHit, _camVomit;
        private CinemachineVirtualCamera[] _cameras;

        private void Awake() {

            _cameras = new CinemachineVirtualCamera[] { _camIdle, _camHit, _camVomit };
            foreach (var cam in _cameras) {
                cam.Follow = target;
                cam.LookAt = target;
            }

            SwitchToIdleCamera();
        }

        private void SetActiveCamera(CinemachineVirtualCamera cameraToActivate) {
            foreach (var cam in _cameras) {
                cam.gameObject.SetActive(cam == cameraToActivate);
            }
        }

        public void SwitchToIdleCamera() {
            StopAllCoroutines();
            SetActiveCamera(_camIdle);
        }

        public void SwitchToHitCamera(float duration) {
            StopAllCoroutines();
            SetActiveCamera(_camHit);
            StartCoroutine(SetBackToIdle(duration));
        }

        public void SwitchToVomitCamera(float duration) {
            StopAllCoroutines();
            SetActiveCamera(_camVomit);
            StartCoroutine(SetBackToIdle(duration));
        }

        private IEnumerator SetBackToIdle(float duration) {
            yield return new WaitForSeconds(duration);
            SwitchToIdleCamera();
        }
    }

}
