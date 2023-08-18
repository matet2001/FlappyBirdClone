using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Code by Mate
public class PointCounterUI : UIBase
{
    [SerializeField]
    TextMeshProUGUI pointCounterText;
        
    public override void Init()
    {       
        GameEventController.OnGameStart += OnGameStart;
        PointController.OnSetPoint += OnSetPoint;
    }
    private void OnGameStart()
    {
        SetCounterText(0);
    }
    private void OnSetPoint(int point)
    {
        SetCounterText(point);
    }
    public void SetCounterText(int numb) => pointCounterText.text = numb.ToString();
}
