using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code by Mate
public class UIClassManager : MonoBehaviour
{
    [SerializeField] GameObject[] parentArray;

    private void Awake()
    {
        foreach (GameObject gameObject in parentArray)
        {
            gameObject.SetActive(true);

            foreach (UIBase uiBase in gameObject.GetComponentsInChildren<UIBase>(true))
                uiBase.Init();
        }
    }
}
