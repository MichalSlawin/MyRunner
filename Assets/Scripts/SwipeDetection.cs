using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeDetection : MonoBehaviour
{
    public delegate void Swipe(Vector2 direction);
    public event Swipe SwipePerformed;

    [SerializeField] private InputAction _position;
    [SerializeField] private InputAction _press;
    [SerializeField] private float _swipeResistance = 100;

    private Vector2 _initialPosition;
    private Vector2 _currentPosition => _position.ReadValue<Vector2>();

    protected void Awake()
    {
        _press.performed += _ => { _initialPosition = _currentPosition; };
        _press.canceled += _ => DetectSwpie();
    }

    protected void OnEnable()
    {
        _position.Enable();
        _press.Enable();
    }

    protected void OnDisable()
    {
        _position.Disable();
        _press.Disable();
    }

    private void DetectSwpie()
    {
        Vector2 positionDelta = _currentPosition - _initialPosition;
        Vector2 direction = Vector2.zero;

        if(Mathf.Abs(positionDelta.x) > _swipeResistance)
        {
            direction.x = Mathf.Clamp(positionDelta.x, -1, 1);
        }

        if (Mathf.Abs(positionDelta.y) > _swipeResistance)
        {
            direction.y = Mathf.Clamp(positionDelta.y, -1, 1);
        }

        if(direction != Vector2.zero && SwipePerformed != null)
        {
            SwipePerformed(direction);
        }
    }
}
