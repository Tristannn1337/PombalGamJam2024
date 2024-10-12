using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomSprite : MonoBehaviour
{
    public Sprite[] trash;
    Sprite randomTrash;

    // Start is called before the first frame update
    void Start()
    {
        if (trash.Length > 0)
        {
            // Select a random index
            int randomIndex = Random.Range(0, trash.Length);
            // Get the random item
            randomTrash = trash[randomIndex];
            // Output the random item
        }
        else
        {
            Debug.Log("Array is empty!");
        }

        this.GetComponent<SpriteRenderer>().sprite = randomTrash;
    }
}
