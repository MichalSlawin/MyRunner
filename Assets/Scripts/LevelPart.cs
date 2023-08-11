using System.Collections.Generic;
using UnityEngine;

public class LevelPart : MonoBehaviour
{
    private const float MIN_OBSTACLES_DISTANCE = 5f;

    [SerializeField] private List<GameObject> _obstaclesPrefabs;
    [SerializeField] private Transform _obstacles;
    [SerializeField] private Transform _obstaclesStart;
    [SerializeField] private Transform _obstaclesEnd;

    public void Initialize()
    {
        float startZ = _obstaclesStart.position.z;
        float endZ = _obstaclesEnd.position.z;

        for(float z = startZ; z <= endZ; z += MIN_OBSTACLES_DISTANCE)
        {
            TryInstantiateObstacle(-Game.LANE_OFFSET, z);
            TryInstantiateObstacle(0, z);
            TryInstantiateObstacle(Game.LANE_OFFSET, z);
        }
    }

    private void TryInstantiateObstacle(float x, float z)
    {
        System.Random random = new System.Random();

        if (random.Next(2) == 0)
        {
            GameObject prefab = _obstaclesPrefabs[random.Next(2)];
            float y = prefab.transform.position.y + _obstacles.position.y;
            Instantiate(prefab, new Vector3(x, y, z),
                Quaternion.identity, _obstacles);
        }
    }
}
