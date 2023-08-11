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

    private bool _gameStarted;

    protected void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        _start.performed += _ => { StartGame(); };
    }

    protected void Start()
    {
        _levelGenerator.Initialize(_unityChan.transform.position);
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

        _unityChan.Initialize();

        _start.Disable();
        _gameStarted = true;
    }
}
