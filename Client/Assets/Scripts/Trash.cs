namespace Pombal {
    using UnityEngine;

    public class Trash : MonoBehaviour {
        [SerializeField] private SpriteRenderer _mainSR, _outlineSR;
        [SerializeField] private ParticleSystem thrashEffect = null;
        public Sprite[] trash;
        Sprite randomTrash;
        void Awake() {
            //Assign Sprite
            if (trash.Length > 0) {
                int randomIndex = Random.Range(0, trash.Length);
                randomTrash = trash[randomIndex];
            } else { Debug.Log("Array is empty!"); }
            //this.GetComponent<SpriteRenderer>().sprite = randomTrash;

            if (_mainSR == null) { Debug.Log(transform.gameObject.name + " has no MainSR"); }
            _mainSR.sprite = randomTrash;
            _mainSR.color = GenerateVibrantColor();
            _outlineSR.sprite = randomTrash;

            //Rotate Z Randomly
            float randomRotation = Random.Range(0f, 360f);
            transform.localRotation = Quaternion.Euler(0, 0, randomRotation);
        }


        //private Color GenerateBrightColor() {
        //    // Generate random values with a minimum threshold to avoid dark colors
        //    float r = Random.Range(0.6f, 1f);
        //    float g = Random.Range(0.6f, 1f);
        //    float b = Random.Range(0.6f, 1f);
        //    return new Color(r, g, b, 1f); // Full alpha
        //}

        private Color GenerateVibrantColor() {
            float hue = Random.Range(0f, 1f);
            float saturation = Random.Range(0.3f, .7f);
            float value = Random.Range(0.9f, 1f);
            Color color = Color.HSVToRGB(hue, saturation, value);
            return color; 
        }
        private void OnBecameVisible()
        {
            thrashEffect.gameObject.SetActive(true);
        }
        private void OnBecameInvisible()
        {
            thrashEffect.gameObject.SetActive(false);
        }
    }
}