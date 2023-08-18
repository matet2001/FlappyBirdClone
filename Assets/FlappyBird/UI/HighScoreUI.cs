using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HighScoreUI : MonoBehaviour
{
    [SerializeField] GameObject highScoreUI, setNamePopUpUI, noInternetUI;

    [SerializeField] TMP_InputField newNameInputField;
    [SerializeField] List<HighScoreElementContainer> highScoreElementList;

    [SerializeField] Color playerColor;
    
    public void OpenHighScorePanel() => OpenPanel(highScoreUI);
    public bool TryToCloseHighScorePanel() => ClosePanel(highScoreUI);
    public void OpenSetNamePanel() => OpenPanel(setNamePopUpUI);
    public bool TryToCloseSetNamePanel() => ClosePanel(setNamePopUpUI);
    public void OpenNoInternetPanel() => OpenPanel(noInternetUI);
    public bool TryToCloseNoInternetPanel() => ClosePanel(noInternetUI);
    private void OpenPanel(GameObject panel)
    {
        CanvasAnimationManager.Open(panel);
    }
    private bool ClosePanel(GameObject panel)
    {
        if (!CanvasAnimationManager.IsFinishedAnimating(panel)) return false;

        CanvasAnimationManager.Close(panel);
        return true;
    }
    
    public void UpdateHighScoreUI(List<RecordStruct> highScoresList, string playerName)
    {
        for (int i = 0; i < highScoreElementList.Count;i++)
        {
            if (i >= highScoresList.Count)
            {
                ClearElementText(i);
                continue;
            }

            SetElementText(highScoresList[i], i);
        }
    }
    private void SetElementText(RecordStruct record, int i)
    {
        GetData(record, 
            i, 
            out string placement, 
            out string name, 
            out string point, 
            out bool isCurrentPlayer
            );

        SetTexts(i, placement, name, point);
        SetTextColor(i, isCurrentPlayer);
    }
    private void GetData(RecordStruct record, int i, out string placement, out string name, out string point, out bool isCurrentPlayer)
    {
        placement = (i + 1).ToString();        
        point = record.playerPoint.ToString();

        name = record.playerName;
        int idTextIndex = name.IndexOf("$");

        isCurrentPlayer = IsCurrentPlayer(name, idTextIndex);

        if (idTextIndex != -1)
            name = name.Remove(idTextIndex);
    }
    private void SetTexts(int i, string placement, string name, string point)
    {
        highScoreElementList[i].placementText.text = placement + ".";
        highScoreElementList[i].nameText.text = name;
        highScoreElementList[i].pointText.text = point;
    }
    private void SetTextColor(int i, bool isCurrentPlayer)
    {
        Color textColor = (isCurrentPlayer) ? playerColor : Color.white;
        highScoreElementList[i].placementText.color = textColor;
        highScoreElementList[i].nameText.color = textColor;
        highScoreElementList[i].pointText.color = textColor;
    }
    private bool IsCurrentPlayer(string name, int idTextIndex)
    {
        if (idTextIndex == -1) return false;
        
        string idText = name.Substring(idTextIndex);
        return idText == "$" + SystemInfo.deviceUniqueIdentifier;
    }
    private void ClearElementText(int i)
    {
        highScoreElementList[i].placementText.text = "";
        highScoreElementList[i].nameText.text = "";
        highScoreElementList[i].pointText.text = "";
    }
    public string GetNameInputFieldText() => newNameInputField.text;
    public int GetHighScoreElementListLength() => highScoreElementList.Count;
}