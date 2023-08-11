using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Game : MonoBehaviour
{
    public const float LANE_OFFSET = 2f;
    public const float MIN_OBSTACLES_DISTANCE = 5f;

    [SerializeField] private UnityChanController _unityChan;
    [SerializeField] private InputAction _start;
    [SerializeField] private LevelGenerator _levelGenerator;
    [SerializeField] private UIController _uIController;

    private float _playerStartZ;

    private bool _gameStarted;

    protected void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        _start.performed += _ => { StartGame(); };
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
    }
}
