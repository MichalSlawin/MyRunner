using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Game : MonoBehaviour
{
    public const float LANE_OFFSET = 2f;

    [SerializeField] private UnityChanController _unityChan;
    [SerializeField] private InputAction _start;

    private bool _gameStarted;

    protected void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        _start.performed += _ => { StartGame(); };
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
