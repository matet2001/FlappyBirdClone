using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Code by Mate
public class ScoreUI : UIBase
{
    [SerializeField] TextMeshProUGUI currentScoreText, bestScoreText;
    [SerializeField] Image medalImage;
    [SerializeField] Sprite[] medalSprites;

    public override void Init()
    {
        ScoreController.OnSetScore += UpdateScore; 
    }
    private void UpdateScore(int point, int medalNumb, int bestScore)
    {
        SetCurrentScoreText(point);
        SetMedalImage(medalNumb);
        SetBestScoreText(bestScore);
    }
    private void SetCurrentScoreText(int numb) => currentScoreText.text = numb.ToString();    
    private void SetMedalImage(int numb)
    {
        if (numb == -1)
        {
            medalImage.enabled = false;
            return;
        }

        medalImage.enabled = true;
        medalImage.sprite = medalSprites[numb];
    }
    private void SetBestScoreText(int numb) => bestScoreText.text = numb.ToString();
}
