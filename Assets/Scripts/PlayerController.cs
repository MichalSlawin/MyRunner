using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private SwipeDetection _swipeDetection;
    [SerializeField] private InputAction _move;

    protected void Awake()
    {
        _move.performed += context => Move(context.ReadValue<Vector2>());
        _swipeDetection.SwipePerformed += context => Move(context);
    }

    protected void OnEnable()
    {
        _move.Enable();
    }

    protected void OnDisable()
    {
        _move.Disable();
    }

    private void Move(Vector2 direction)
    {
        Debug.Log(direction);
    }
}
