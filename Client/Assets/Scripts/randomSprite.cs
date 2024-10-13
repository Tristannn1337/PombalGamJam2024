using UnityEngine;

public class randomSprite : MonoBehaviour {
    [SerializeField] private SpriteRenderer _mainSR;
    public Sprite[] _sprites;
    Sprite _selectedSprite;
    void Awake() {
        //Assign Sprite
        if (_sprites.Length > 0) {
            int randomIndex = Random.Range(0, _sprites.Length);
            _selectedSprite = _sprites[randomIndex];
        } else { Debug.Log("Array is empty!"); }

        if (_mainSR == null) { Debug.Log(transform.gameObject.name + " has no MainSR"); }
        _mainSR.sprite = _selectedSprite;
    }
}
