using System.Collections.Generic;
using UnityEngine;

public class LevelPart : MonoBehaviour
{
    private const int SPAWN_OBSTACLE_CHANCE = 3;

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
            GetRandomPrefabNumbers(out int prefab1, out int prefab2, out int prefab3);

            TryPlaceObstacle(-Game.LANE_OFFSET, z, prefab1);
            TryPlaceObstacle(0, z, prefab2);
            TryPlaceObstacle(Game.LANE_OFFSET, z, prefab3);
        }
    }

    public void GetRandomPrefabNumbers(out int prefab1, out int prefab2, out int prefab3)
    {
        System.Random random = new System.Random();

        do
        {
            prefab1 = random.Next(ObstaclesPool.Instance.PrefabsCount);
            prefab2 = random.Next(ObstaclesPool.Instance.PrefabsCount);
            prefab3 = random.Next(ObstaclesPool.Instance.PrefabsCount);
        }
        while (prefab1 == prefab2 && prefab2 == prefab3);
    }

    private void TryPlaceObstacle(float x, float z, int prefabNumber)
    {
        System.Random random = new System.Random();

        if(random.Next(SPAWN_OBSTACLE_CHANCE) == 0)
        {
            return;
        }

        Obstacle obstacle = ObstaclesPool.Instance.GetObstacle((ObstacleType) prefabNumber);
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
