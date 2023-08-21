using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour, IDifficultyUpdater
{
    private const float PART_Z_OFFSET = 150f;

    [SerializeField] private LevelPart _currentPart;
    [SerializeField] private LevelPart _nextPart;
    [SerializeField] private LevelPart _levelPartPrefab;
    [Header("Obstacles distance")]
    [SerializeField] private float _obstaclesDistanceStart = 8f;
    [SerializeField] private float _obstaclesDistanceEnd = 4f;
    [SerializeField] private float _obstaclesDistanceStep = 0.5f;

    public static float ObstaclesDistance { get; private set; }

    public void IncreaseDifficulty()
    {
        if(ObstaclesDistance > _obstaclesDistanceEnd)
        {
            ObstaclesDistance -= _obstaclesDistanceStep;
        }
    }

    public void Initialize(float playerStartZ)
    {
        ObstaclesDistance = _obstaclesDistanceStart;
        _currentPart.Initialize(playerStartZ + ObstaclesDistance);
        _nextPart.Initialize();
    }

    public void OnNextPartEnter()
    {
        _currentPart.DestroyPart();

        Vector3 position = _nextPart.transform.position;
        LevelPart newPart = Instantiate(_levelPartPrefab, 
            new Vector3(position.x, position.y, position.z + PART_Z_OFFSET), Quaternion.identity, transform);
        newPart.Initialize();

        _currentPart = _nextPart;
        _nextPart = newPart;
    }
}
