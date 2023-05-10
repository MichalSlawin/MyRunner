using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private SwipeDetection _swipeDetection;
    [SerializeField] private InputAction _move;
    [SerializeField] private UnityChanController _unityChan;

    protected void Awake()
    {
        _move.performed += context => _unityChan.Move(context.ReadValue<Vector2>());
        _swipeDetection.SwipePerformed += context => _unityChan.Move(context);
    }

    protected void OnEnable()
    {
        _move.Enable();
    }

    protected void OnDisable()
    {
        _move.Disable();
    }
}
