using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private const float PART_Z_OFFSET = 150f;

    [SerializeField] LevelPart _currentPart;
    [SerializeField] LevelPart _nextPart;
    [SerializeField] LevelPart _levelPartPrefab;

    public void Initialize()
    {
        _currentPart.Initialize();
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
