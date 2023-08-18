using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenAnimationContainer : MonoBehaviour
{
    public RectTransform RectTransform {
        private set { }
        get 
        {
            if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
            return rectTransform;
        }
    }
    private RectTransform rectTransform; // { set; get; }
    public Vector3 direction;
    public Vector3 path { set; get; }

    //Debug
    public Vector3 startPosition { set; private get; }
    [SerializeField] bool shouldDebug;

    private void OnValidate()
    {
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
    }
    //Debug
    private void OnDrawGizmos()
    {
        if (!shouldDebug) return;
        Gizmos.color = Color.red;
        Vector3 relativePath = startPosition + path;
        Gizmos.DrawLine(rectTransform.position, relativePath);
    }
    public void SetStartPosition()
    {
        startPosition = rectTransform.position;
    }
}
