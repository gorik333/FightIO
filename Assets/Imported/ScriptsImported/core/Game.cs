using UnityEngine;
using UnityEngine.EventSystems;

public class Game : MonoBehaviour
{
    [SerializeField]
    private LevelData[] m_level;

    [SerializeField]
    private UIProc m_UI;

    [SerializeField]
    private GameUI m_gameUI;

    [SerializeField]
    private LevelStageController m_levelStageController;

    [SerializeField]
    private GameObject m_playField;

    [SerializeField]
    private GameObject m_previewPlayerPrefab;

    private GameObject m_currentPreviewPlayer;

    private LevelData m_currentLevelData;
    private MovementJoystick m_movementJoystick;
    private Player m_player;

    static Game _instance;

    public static Game Instance { get { return _instance; } }

    Game() { _instance = this; }


    void Start()
    {
        Application.targetFrameRate = 60;

        ADs._AdShown += AdShown;
        ADs._AdHidden += AdHidden;

        Restart();
    }


    public void Restart()
    {
        int currentLevel = Stats.Instance.level - 1;

        m_currentLevelData = m_level[currentLevel % m_level.Length];

        m_gameUI.UpdateUILevelNumber(currentLevel + 1);

        SetPlayFieldSize();

        TroopSpawner.Instance.CleanMapFromTroops();

        m_levelStageController.SetMenuStage();

        Spawner.Instance.SetUp(m_currentLevelData);

        m_currentPreviewPlayer = Instantiate(m_previewPlayerPrefab, transform.position, m_previewPlayerPrefab.transform.rotation, transform);

        m_currentPreviewPlayer.transform.position = Vector3.zero;

        CinemachineExtension.Instance.ChangeOffset(GameOffset.MenuOffset);
        CinemachineExtension.Instance.SetFollow(m_currentPreviewPlayer.transform, GameOffset.MenuOffset);
    }


    private void StartGame()
    {
        m_gameUI.gameObject.SetActive(true);

        Destroy(m_currentPreviewPlayer);

        TroopSpawner.Instance.SetUp(m_currentLevelData);
        TroopSpawner.Instance.SpawnTroops();

        LevelStageController.Instance.StartLevel(m_currentLevelData);
    }


    private void SetPlayFieldSize()
    {
        m_playField.transform.localScale = Vector3.one;

        m_playField.transform.localScale *= m_currentLevelData.GetFieldSize;
    }


    public void Continue()
    {

    }


    public void InitializePlayerMovement(Player player)
    {
        m_movementJoystick = new MovementJoystick();

        m_movementJoystick.Initialize();

        m_player = player;
    }


    public void UpdateUITimer(int time)
    {
        m_gameUI.UpdateUITimer(time);
    }


    // EVENTS


    public void onPointerDown()
    {
        JoystickPointerDown();
    }


    private void JoystickPointerDown()
    {
        if (m_movementJoystick != null)
        {
            m_movementJoystick.PointerDown();
        }
    }


    public void onPointerUp()
    {
        JoystickPointerUp();
    }


    private void JoystickPointerUp()
    {
        if (m_movementJoystick != null)
        {
            m_movementJoystick.PointerUp();

            if (m_player != null)
                m_player.ResetMovement();
        }
    }


    public void onDrag(BaseEventData eventData)
    {
        JoystickDrag(eventData);
    }


    private void JoystickDrag(BaseEventData eventData)
    {
        if (m_movementJoystick != null)
        {
            m_movementJoystick.Drag(eventData);

            Vector2 direction = m_movementJoystick.GetJoystickDirection;
            float distanceMultiplier = m_movementJoystick.MultiplierByDistanceFromCenter;

            m_player.Move(direction, distanceMultiplier);
        }
    }


    public void onClick(bool pressed)
    {
        if (pressed && m_UI.IsMainMenuShown)
        {
            StartGame();
            JoystickPointerDown();

            m_UI.ShowMainMenu(false);
        }
    }


    public void onGameOver()
    {
        m_UI.ShowGameOver(false, Stats.Instance.scores);

        m_gameUI.gameObject.SetActive(false);

        SoundsMan.Play("4pok");
    }


    public void onFinish()
    {
        m_UI.ShowWin(Stats.Instance.level);

        m_gameUI.gameObject.SetActive(false);

        SoundsMan.Play("4pok");
    }


    void AdShown(bool rewarded)
    {
        UIProc.Instance.DebugOut("AD SHOWN: " + rewarded);

        Vibro.Mute(true);
        SoundsMan.MuteSFX(true);
    }


    void AdHidden(bool rewarded)
    {
        UIProc.Instance.DebugOut("AD HIDDEN: " + rewarded);

        Vibro.Mute(Stats.Instance.muteVibro);
        SoundsMan.MuteSFX(Stats.Instance.muteSFX);
    }


    public LevelData GetCurrentLevelData => m_currentLevelData;
}
