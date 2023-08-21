using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesPool : MonoBehaviour
{
    [SerializeField] private Transform _poolTransform;
    [SerializeField] private List<Obstacle> _obstaclesPrefabs;
    [SerializeField] private List<Material> _obstacleMaterials;

    public static ObstaclesPool Instance;

    public int PrefabsCount => _obstaclesPrefabs.Count;

    private Dictionary<ObstacleType, List<Obstacle>> _pooledObstaclesByType;

    protected void Awake()
    {
        Instance = this;
    }

    public Material GetRandomMaterial()
    {
        System.Random random = new System.Random();

        int materialIdx = random.Next(_obstacleMaterials.Count);

        return _obstacleMaterials[materialIdx];
    }

    protected void Start()
    {
        _pooledObstaclesByType = new Dictionary<ObstacleType, List<Obstacle>>();

        for(int i = 0; i < PrefabsCount; i++)
        {
            _pooledObstaclesByType[(ObstacleType) i] = new List<Obstacle>();
        }
    }

    public Obstacle GetObstacle(ObstacleType type)
    {
        if(_pooledObstaclesByType.TryGetValue(type, out List<Obstacle> pooledObstacles))
        {
            int pooledCount = pooledObstacles.Count;

            if (pooledCount == 0)
            {
                Obstacle newObstacle = Instantiate(_obstaclesPrefabs[(int)type]);
                newObstacle.ChangeMaterial(GetRandomMaterial());
                return newObstacle;
            }

            Obstacle obstacle = pooledObstacles[pooledCount - 1];
            pooledObstacles.RemoveAt(pooledCount - 1);
            return obstacle;
        }

        return null;
    }

    public void ReturnToPool(Obstacle obstacle)
    {
        if (_pooledObstaclesByType.TryGetValue(obstacle.ObstacleType, out List<Obstacle> pooledObstacles))
        {
            obstacle.gameObject.SetActive(false);
            pooledObstacles.Add(obstacle);
            obstacle.transform.SetParent(_poolTransform);
        }
    }
}
