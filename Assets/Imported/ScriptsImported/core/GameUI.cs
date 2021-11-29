using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private Text m_txtTimer;

    [SerializeField]
    private Text m_txtLevelNumber;

    [SerializeField]
    private Text m_txtBattleMsg;

    [SerializeField]
    private Text m_txtFoodEaten;

    private int m_foodEaten;

    private const float FADE_SPEED = 1.5f;
    private const float FADE_DELAY = 2f;

    private const string PREPARATION_FOR_BATTLE = "PREPARE TO FIGHT!";
    private const string BATTLE = "FIGHT!";

    public static GameUI Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }


    public void UpdateUITimer(int time)
    {
        m_txtTimer.text = "Time left: " + time.ToString();
    }


    public void UpdateUILevelNumber(int level)
    {
        m_txtLevelNumber.text = "Current level: " + level.ToString();
    }


    public void UpdateUIAddEatenFood()
    {
        m_foodEaten++;

        m_txtFoodEaten.text = m_foodEaten.ToString();
    }


    public void TurnOnFoodStageUI()
    {
        ResetUI();

        m_txtTimer.enabled = true;
        m_txtLevelNumber.enabled = true;
        m_txtFoodEaten.enabled = true;
    }


    private void ResetUI()
    {
        m_foodEaten = 0;
        m_txtFoodEaten.text = m_foodEaten.ToString();
    }


    private void TurnOffFoodStageUI()
    {
        m_txtTimer.enabled = false;
        m_txtFoodEaten.enabled = false;
        m_txtLevelNumber.enabled = false;
    }


    private void TurnOnBattleStageUI()
    {
        m_txtBattleMsg.enabled = true;
    }


    public void UpdateUIBattleMsg(LevelStage levelStage)
    {
        TurnOffFoodStageUI();
        TurnOnBattleStageUI();

        switch (levelStage)
        {
            case LevelStage.PreparationForBattle:
                m_txtBattleMsg.text = PREPARATION_FOR_BATTLE;
                break;
            case LevelStage.Battle:
                m_txtBattleMsg.text = BATTLE;
                m_txtBattleMsg.DOFade(0, FADE_SPEED).SetDelay(FADE_DELAY);
                StartCoroutine(TurnOffDelay(m_txtBattleMsg, FADE_SPEED + FADE_DELAY));
                break;
        }
    }


    private IEnumerator TurnOffDelay(Text textMsg, float delay)
    {
        yield return new WaitForSeconds(delay);

        textMsg.DOFade(1, 0); // turn on fade
        textMsg.enabled = false;
    }
}
