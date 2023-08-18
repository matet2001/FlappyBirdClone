using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using NaughtyAttributes;

public class CanvasAnimationManager : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    private const float rectBoundMultiplier = 0.75f;

    private void Awake()
    {     
        SetUpElements();      
    }
    [Button]
    private void DebugPaths()
    {
        foreach (TweenAnimationContainer animationContainer in GetComponentsInChildren<TweenAnimationContainer>(true))
        {
            SetRelativePath(animationContainer);
            animationContainer.SetStartPosition();
        }
    }
    private void SetUpElements()
    {
        foreach (TweenAnimationContainer animationContainer in GetComponentsInChildren<TweenAnimationContainer>(true))
        {
            SetRelativePath(animationContainer);
            SetElementPosition(animationContainer);
            animationContainer.SetStartPosition();//Debug
        }
    }
    private void SetRelativePath(TweenAnimationContainer animationContainer)
    {    
        Vector3 path = GetRelativePath(animationContainer);
        animationContainer.path = path;
    }
    private Vector3 GetRelativePath(TweenAnimationContainer animationContainer)
    {
        RectTransform rectTransform = animationContainer.RectTransform;
        
        Vector2 direction = animationContainer.direction;
        Vector2 directionAbs = new Vector2(Mathf.Abs(direction.x), Mathf.Abs(direction.y));

        Vector2 rectBound = new Vector2(rectTransform.rect.width, rectTransform.rect.height) * rectBoundMultiplier;

        Vector2 canvasPos = Vector2.zero;
        Vector2 canvasBound = new Vector2(canvas.pixelRect.width, canvas.pixelRect.height) / canvas.scaleFactor;

        Vector2 distanceFromCanvas = ((Vector2)rectTransform.localPosition - canvasPos) * directionAbs;
        Vector2 distanceFromCanvasToEdge = (canvasPos - canvasBound / 2f) * -direction;
        Vector2 distanceFromRectToCanvasEdge = distanceFromCanvasToEdge - distanceFromCanvas;

        Vector3 path = distanceFromRectToCanvasEdge + rectBound * direction;
        return path;
    }
    private static void SetElementPosition(TweenAnimationContainer animationContainer)
    {
        RectTransform rectTransform = animationContainer.RectTransform;
        rectTransform.localPosition += animationContainer.path;
    }
    public static void Open(GameObject gameObject) 
    {
        gameObject.SetActive(true);
        Move(gameObject, -1);
    }
    public static void Close(GameObject gameObject)
    {
        Move(gameObject, 1);
    }
    private static void Move(GameObject gameObject, float direction)
    {
        foreach (RectTransform rectTransform in gameObject.transform)       
            MoveRectTransformOnPath(rectTransform, direction);                   
    }
    private static void MoveRectTransformOnPath(RectTransform rectTransform, float direction)
    {        
        if (!rectTransform.transform.TryGetComponent(out TweenAnimationContainer animationContainer)) return;

        Vector3 toPos = animationContainer.RectTransform.localPosition + 
            animationContainer.path * direction;        

        LeanTween.moveLocal(animationContainer.gameObject , toPos, 1f);
    }
    public static bool IsFinishedAnimating(GameObject gameObject)
    {
        foreach (RectTransform rectTransform in gameObject.transform)
            if (LeanTween.isTweening(rectTransform)) return false;

        return true;
    }
}
