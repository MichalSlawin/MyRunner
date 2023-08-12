using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public const float LANE_OFFSET = 2f;
    public const float MIN_OBSTACLES_DISTANCE = 5f;

    [SerializeField] private UnityChanController _unityChan;
    [SerializeField] private InputAction _start;
    [SerializeField] private LevelGenerator _levelGenerator;
    [SerializeField] private UIController _uIController;
    [SerializeField] private float _gameRestartTime = 1.5f;

    private float _playerStartZ;
    private bool _gameStarted;
    private int _currentScore;
    private int _bestScore;
    private DataManager _dataManager;
    private bool _gameLost;

    protected void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        InitDataManager();

        _start.performed += _ => { StartGame(); };
    }

    private void InitDataManager()
    {
        _dataManager = new DataManager();
        _dataManager.Load();

        _bestScore = _dataManager.GetGameData().score;
        _uIController.SetBestScoreText(_bestScore);
    }

    protected void Start()
    {
        _playerStartZ = _unityChan.transform.position.z;
        _levelGenerator.Initialize(_playerStartZ);
    }

    protected void OnEnable()
    {
        _start.Enable();    
    }

    protected void OnDisable()
    {
        _start.Disable();
    }

    protected void FixedUpdate()
    {
        if(_gameLost == false)
        {
            _currentScore = (int)((_unityChan.transform.position.z - _playerStartZ) * 10);
            _uIController.SetCurrentScoreText(_currentScore);
        }
    }

    private void StartGame()
    {
        if(_gameStarted)
        {
            return;
        }

        _uIController.SwitchPressAnyKeyText(false);
        _unityChan.Initialize();

        _start.Disable();
        _gameStarted = true;
        _gameLost = false;
    }

    public void OnGameLost()
    {
        _gameLost = true;
        UpdateBestScore();
        StartCoroutine(RestartGame());
    }

    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(_gameRestartTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateBestScore()
    {
        if (_currentScore > _bestScore)
        {
            _bestScore = _currentScore;
            _uIController.SetBestScoreText(_bestScore);

            _dataManager.SetBestScore(_bestScore);
            _dataManager.Save();
        }
    }
}
