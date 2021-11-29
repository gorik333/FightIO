using UnityEngine;
using TMPro;
using System;
using DG.Tweening;


public class LevelStageUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _collectFoodTimerText;

    [SerializeField]
    private LevelStageController _levelStageController;

    private const string PREPARE_TEXT = "PREPARE FOR BATTLE!";
    private const string BATTLE_TEXT = "FIGHT!";


    public void UpdateCollectFoodTimer(float time)
    {
        _collectFoodTimerText.text = Convert.ToInt32(time).ToString();
    }


    public void SetPreparationStageText()
    {
        _collectFoodTimerText.text = PREPARE_TEXT;
        _collectFoodTimerText.alpha = 1f;

        _collectFoodTimerText.DOFade(0f, 0.8f).SetDelay(3f);
    }


    public void SetBattleStageText()
    {
        _collectFoodTimerText.text = BATTLE_TEXT;
        _collectFoodTimerText.alpha = 1f;

        _collectFoodTimerText.DOFade(0f, 0.8f).SetDelay(1f);
    }
}
