using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using System.Collections;
using static UnityEngine.Random;
using UnityEditor;

public class GameHUDController : MonoBehaviour
{
    private UIDocument _document;
    private VisualElement _gameOverVisualTree;
    private VisualElement _playerHUDVisualTree;
    private VisualElement _mainMenuHUDVisualTree;
    [SerializeField] private GameContoller _gameController;
    [SerializeField] private AudioClips _audioClips;

    private GameFSM _stateMachine;
    private GamePlayState _gamePlayState;

    private Button _gameOverRetryButton;
    private Button _gameOverMenuButton;
    private Button _mainMenuPlayButton;
    private Button _mainMenuResetButton;
    private Button _mainMenuQuitButton;

    private Label _elapsedTimeLabel;
    private Label _healthIconOne;
    private Label _healthIconTwo;
    private Label _healthIconThree;
    private Label _finalTimeLabel;
    private Label _tileScoreLabel;
    private Label _gradeLabel;
    private Label _highScoreLabel;
    private VisualElement _mainMenuHUD;

    private List<Button> _buttons = new List<Button>();
    private AudioSource _buttonClickAudio;

    private int _previousMissCount = 0;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _stateMachine = _gameController.GetComponent<GameFSM>();
        _buttonClickAudio = GetComponent<AudioSource>();

        // get menu sub objects
        _gameOverVisualTree = _document.rootVisualElement.Q("GameOverVisualTree");
        // assign button callbacks
        _gameOverRetryButton = _gameOverVisualTree.Q("RetryButton") as Button;
        _gameOverMenuButton = _gameOverVisualTree.Q("MenuButton") as Button;
        _finalTimeLabel = _gameOverVisualTree.Q("FinalTimeLabel") as Label;
        _tileScoreLabel = _gameOverVisualTree.Q("TileScoreLabel") as Label;
        _gradeLabel = _gameOverVisualTree.Q("GradeLabel") as Label;

        _playerHUDVisualTree = _document.rootVisualElement.Q("PlayerHUDVisualTree");

        _elapsedTimeLabel = _document.rootVisualElement.Q("TimerLabel") as Label;
        _healthIconOne = _document.rootVisualElement.Q("HealthIconOne") as Label;
        _healthIconTwo = _document.rootVisualElement.Q("HealthIconTwo") as Label;
        _healthIconThree = _document.rootVisualElement.Q("HealthIconThree") as Label;

        _mainMenuHUDVisualTree = _document.rootVisualElement.Q("MainMenuHUDVisualTree");
        // assign button callbacks
        _mainMenuPlayButton = _mainMenuHUDVisualTree.Q("PlayButton") as Button;
        _mainMenuResetButton = _mainMenuHUDVisualTree.Q("ResetButton") as Button;
        _mainMenuQuitButton = _mainMenuHUDVisualTree.Q("QuitButton") as Button;
        _highScoreLabel = _mainMenuHUDVisualTree.Q("CurrentHighScoreLable") as Label;
        _mainMenuHUD = _mainMenuHUDVisualTree.Q("MainMenuHUDVisualTree") as VisualElement;



        _buttons = _document.rootVisualElement.Query<Button>().ToList();
    }

    private void Start()
    {
        DisableAllDisplays();
        _mainMenuHUD.ClearClassList();
        _mainMenuHUD.AddToClassList("mainMenuEnd");
    }

    private void Update()
    {
        UpdateScoreLabel();
    }

    private void UpdateScoreLabel()
    {
        float elapsedTime = _gameController.ElapsedTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60) % 60;
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        //formating
        string textElapsedTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        //update UI
        _elapsedTimeLabel.text = textElapsedTime;
    }

    private void OnEnable()
    {
        // register callbacks to button
        _gameOverRetryButton.RegisterCallback<ClickEvent>(OnGameOverRetryButtonClick);
        _gameOverMenuButton.RegisterCallback<ClickEvent>(OnGameOverMenuButtonClick);
        _mainMenuPlayButton.RegisterCallback<ClickEvent>(OnMainMenuPlayButtonClick);
        _mainMenuQuitButton.RegisterCallback<ClickEvent>(OnMainMenuQuitButtonClick);
        _mainMenuResetButton.RegisterCallback<ClickEvent>(OnMainMenuResetButtonClick);

        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].RegisterCallback<ClickEvent>(OnButtonClick);
        }
    }

    private void OnDisable()
    {
        // unregister callbacks to buttons
        _gameOverRetryButton.UnregisterCallback<ClickEvent>(OnGameOverRetryButtonClick);
        _gameOverMenuButton.UnregisterCallback<ClickEvent>(OnGameOverMenuButtonClick);
        _mainMenuPlayButton.UnregisterCallback<ClickEvent>(OnMainMenuPlayButtonClick);
        _mainMenuQuitButton.UnregisterCallback<ClickEvent>(OnMainMenuQuitButtonClick);
        _mainMenuResetButton.UnregisterCallback<ClickEvent>(OnMainMenuResetButtonClick);

        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].UnregisterCallback<ClickEvent>(OnButtonClick);
        }
    }

    private void OnGameOverRetryButtonClick(ClickEvent evt)
    {
        _gameOverVisualTree.style.display = DisplayStyle.None;
        _gameController.ResetGame();
        _stateMachine.ChangeState(_stateMachine.PlayState);
    }

    private void OnGameOverMenuButtonClick(ClickEvent evt)
    {
        //animateMenuTransitionTo();
        StartCoroutine(DelayedStateChange(0.5f, _stateMachine.MenuState));


        //_stateMachine.ChangeState(_stateMachine.MenuState);

    }

    private void OnMainMenuPlayButtonClick(ClickEvent evt)
    {
        _gameController.ResetGame();
        //animateMenuTransitionFrom();
        StartCoroutine(DelayedStateChange(0.5f, _stateMachine.PlayState));
        Debug.Log("Play button clicked!");
    }

    private void OnMainMenuQuitButtonClick(ClickEvent evt)
    {
        #if (UNITY_EDITOR)
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void OnMainMenuResetButtonClick(ClickEvent evt)
    {
        SaveManager.Instance.ResetSave();
        DisplayHighScore();
    }

    private void OnButtonClick(ClickEvent evt)
    {
        _buttonClickAudio.Play();
    }

    public void DisableAllDisplays()
    {
        _gameOverVisualTree.style.display = DisplayStyle.None;
        _playerHUDVisualTree.style.display = DisplayStyle.None;
        _mainMenuHUDVisualTree.style.display = DisplayStyle.None;
    }

    public void DisplayGameOverMenu()
    {
        _gameOverVisualTree.style.display = DisplayStyle.Flex;
    }

    public void HideGameOverMenu()
    {
        _gameOverVisualTree.style.display = DisplayStyle.None;
    }

    public void DisplayPlayerHUD()
    {
        _playerHUDVisualTree.style.display = DisplayStyle.Flex;

    }

    public void HidePlayerHUD()
    {
        _playerHUDVisualTree.style.display = DisplayStyle.None;
    }

    public void DisplayMainMenuHUD()
    {
        _mainMenuHUDVisualTree.style.display = DisplayStyle.Flex;
    }

    public void HideMainMenuHUD()
    {
        _mainMenuHUDVisualTree.style.display = DisplayStyle.None;
    }

    public void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void UpdateHealthIcons(int activeMisses)
    {
        _healthIconOne.RemoveFromClassList("dimmed");
        _healthIconTwo.RemoveFromClassList("dimmed");
        _healthIconThree.RemoveFromClassList("dimmed");

        if (activeMisses < _previousMissCount)
        {
            int iconsUndimmed = _previousMissCount - activeMisses;

            if (_gamePlayState.IsInPlay == true)
            {
                for (int i = 0; i < iconsUndimmed; i++)
                {
                    AudioSource audioSource = AudioController.PlayClip2D(_audioClips._healthRespawn, 1);
                    audioSource.pitch = UnityEngine.Random.Range(.8f, 1.2f);
                }
            }
        }

        if (_gameController._activeMissCount >= 1) _healthIconOne.AddToClassList("dimmed");
        if (_gameController._activeMissCount >= 2) _healthIconTwo.AddToClassList("dimmed");
        if (_gameController._activeMissCount >= 3) _healthIconThree.AddToClassList("dimmed");

        _previousMissCount =  activeMisses;
    }

    private string CalculateGrade(int score, int misses)
    {
        int total = score + misses;

        float accuracy = (float)score / total;

        if (accuracy == 1.00f) return "S";
        if (accuracy >= 0.95f) return "A+";
        if (accuracy >= 0.85f) return "A";
        if (accuracy >= 0.75f) return "B";
        if (accuracy >= 0.65f) return "C";
        if (accuracy >= 0.50f) return "D";

        return "F";
    }

    public void DisplayFinalStats(bool isNewHighScore)
    {
        float elapsedTime = _gameController.ElapsedTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60) % 60;
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        //formating
        string textFinalTime = string.Format("Time: {0:0}:{1:00} |", minutes, seconds);
        _finalTimeLabel.text = textFinalTime;

        int finalTileCount = _gameController._score;
        if (isNewHighScore)
        {
            string textFinalTileCount = string.Format("New High Score!: {0}", finalTileCount);
            _tileScoreLabel.text = textFinalTileCount;
        }
        else
        {
            string textFinalTileCount = string.Format("Score: {0}", finalTileCount);
            _tileScoreLabel.text = textFinalTileCount;
        }
        string grade = CalculateGrade(_gameController._score, _gameController._totalMissCount);
        string textGrade = grade;
        _gradeLabel.text = textGrade;
    }

    public void DisplayHighScore()
    {
        int highScore = SaveManager.Instance.ActiveSaveData.Score;
        string textHighScore = string.Format("High Score: {0}", highScore);
        _highScoreLabel.text = textHighScore;
    }

    public void SetGamePlayState(GamePlayState gamePlayState)
    {
        _gamePlayState = gamePlayState;
    }

    public void animateMenuTransitionFrom()
    {
        StartCoroutine(DelayedAnimateMenuTransitionFrom());
    }

    private IEnumerator DelayedAnimateMenuTransitionFrom()
    {
        yield return null;
        _mainMenuHUD.RemoveFromClassList("mainMenuEnd");
        _mainMenuHUD.AddToClassList("mainMenuStart");
    }

    public void animateMenuTransitionTo()
    {
        StartCoroutine(DelayedAnimateMenuTransitionTo());
    }

    private IEnumerator DelayedAnimateMenuTransitionTo()
    {
        yield return null;
        _mainMenuHUD.RemoveFromClassList("mainMenuStart");
        _mainMenuHUD.AddToClassList("mainMenuEnd");
    }

    private IEnumerator DelayedStateChange(float delay, State state)
    {
        yield return new WaitForSeconds(delay);
        _mainMenuHUD.style.display = DisplayStyle.None;
        _stateMachine .ChangeState(state);
    }

}
