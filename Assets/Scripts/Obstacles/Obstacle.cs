using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private ObstacleType _obstacleType;
    [SerializeField] private List<Renderer> _renderers;

    public ObstacleType ObstacleType => _obstacleType;

    public void ChangeMaterial(Material material)
    {
        foreach(Renderer renderer in _renderers)
        {
            renderer.material = material;
        }
    }
}
