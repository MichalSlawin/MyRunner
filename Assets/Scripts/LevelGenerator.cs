using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private const float PART_Z_OFFSET = 150f;

    [SerializeField] private LevelPart _currentPart;
    [SerializeField] private LevelPart _nextPart;
    [SerializeField] private LevelPart _levelPartPrefab;

    public void Initialize(Vector3 playerInitialPosition)
    {
        _currentPart.Initialize(playerInitialPosition.z + Game.MIN_OBSTACLES_DISTANCE);
        _nextPart.Initialize();
    }

    public void OnNextPartEnter()
    {
        Destroy(_currentPart.gameObject);

        Vector3 position = _nextPart.transform.position;
        LevelPart newPart = Instantiate(_levelPartPrefab, 
            new Vector3(position.x, position.y, position.z + PART_Z_OFFSET), Quaternion.identity, transform);
        newPart.Initialize();

        _currentPart = _nextPart;
        _nextPart = newPart;
    }
}
