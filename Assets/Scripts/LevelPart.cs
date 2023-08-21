using System.Collections.Generic;
using UnityEngine;

public class LevelPart : MonoBehaviour
{
    [SerializeField] private Transform _obstacles;
    [SerializeField] private Transform _obstaclesStart;
    [SerializeField] private Transform _obstaclesEnd;

    private List<Obstacle> _obstaclesList = new List<Obstacle>();

    public void Initialize(float? forcedStartZ = null)
    {
        float startZ = forcedStartZ ?? _obstaclesStart.position.z;
        float endZ = _obstaclesEnd.position.z;

        for (float z = startZ; z <= endZ; z += LevelGenerator.ObstaclesDistance)
        {
            TryPlaceObstacle(-Game.LANE_OFFSET, z);
            TryPlaceObstacle(0, z);
            TryPlaceObstacle(Game.LANE_OFFSET, z);
        }
    }

    private void TryPlaceObstacle(float x, float z)
    {
        System.Random random = new System.Random();

        if(random.Next(2) == 0)
        {
            return;
        }

        Obstacle obstacle = ObstaclesPool.Instance.GetObstacle((ObstacleType) random.Next(ObstaclesPool.Instance.PrefabsCount));
        if(obstacle == null)
        {
            Debug.LogError("obstacle from pool is null");
            return;
        }

        obstacle.transform.SetParent(_obstacles);
        obstacle.transform.SetPositionAndRotation(new Vector3(x, 0, z), Quaternion.identity);
        obstacle.gameObject.SetActive(true);

        _obstaclesList.Add(obstacle);
    }

    public void DestroyPart()
    {
        foreach(Obstacle obstacle in _obstaclesList)
        {
            ObstaclesPool.Instance.ReturnToPool(obstacle);
        }

        Destroy(gameObject);
    }
}
