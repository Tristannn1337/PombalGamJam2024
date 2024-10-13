using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBox : MonoBehaviour
{
    [SerializeField] float destroyTime;
    [SerializeField] TextMesh textMesh;
    [SerializeField] float yOffset = 1f;

    Transform humanTransform;
    public void ActivateText(string text, Transform humanTransform)
    {
        this.humanTransform = humanTransform;
        textMesh.text = text;
        Destroy(gameObject, destroyTime);
    }
    private void Update()
    {
        if(humanTransform != null)
        {
            transform.position = (Vector2)humanTransform.position + (Vector2.up * yOffset);
        }
    }
}
