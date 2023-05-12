using System.Collections;
using UnityEngine;

public class UnityChanController : MonoBehaviour
{
    private const int ROTATION_ANGLE = 45;

    [SerializeField] private float animSpeed = 1.5f;
	[SerializeField] private float jumpPower = 3.0f;
	[SerializeField] private float _moveSpeed = 7f;
	[SerializeField] private float _rotationSpeed = 5f;
	[SerializeField] private float _runRotatedTime = 1f;
	[SerializeField] private CapsuleCollider col;
	[SerializeField] private Rigidbody rb;
	[SerializeField] private Animator anim;

	private enum Lane
    {
		Left,
		Middle,
        Right
    }

    private enum Direction
    {
		Left,
		Right
	}

    private Quaternion _startRotation;
	private Quaternion _targetRotation;
	private Lane _occupiedLane = Lane.Middle;
	private Coroutine _faceFrontCoroutine;
	private float _timeCount;
	private bool _rotating;

	protected void Start()
	{
		anim.speed = animSpeed;
		anim.SetFloat("Speed", 1f);
		_startRotation = transform.rotation;
		_targetRotation = transform.rotation;
	}

    protected void FixedUpdate()
    {
		transform.position += transform.forward * _moveSpeed * Time.deltaTime;

		if(_startRotation != _targetRotation)
        {
			transform.rotation = Quaternion.Lerp(_startRotation, _targetRotation, _rotationSpeed * _timeCount);
			_timeCount += Time.deltaTime;
		}

		if(transform.rotation.y == 0 && _targetRotation.y == 0)
        {
			Vector3 current = transform.position;
			_rotating = false;

			if (_occupiedLane == Lane.Middle && current.x != 0)
			{
				transform.position = new Vector3(0, current.y, current.z);
			}
		}
	}

	public void Move(Vector2 direction)
    {
		float horizontal = direction.x;
		float vertical = direction.y;

		if (horizontal > 0)
        {
			ChangeLane(Direction.Right);
        }
		else if (horizontal < 0)
        {
			ChangeLane(Direction.Left);
		}

		if (vertical > 0)
        {
			Jump();
		}
		else if (vertical < 0)
        {
			Slide();
        }
	}

	private void ChangeLane(Direction direction)
    {
		if(_rotating)
        {
			return;
        }

		if(_faceFrontCoroutine != null)
        {
			StopCoroutine(_faceFrontCoroutine);
		}

		float angle = 0;
		Lane newLane = _occupiedLane;

		if (direction == Direction.Right && _occupiedLane != Lane.Right)
        {
			angle = ROTATION_ANGLE;
			newLane = _occupiedLane == Lane.Left ? Lane.Middle : Lane.Right;
        }
		else if (direction == Direction.Left && _occupiedLane != Lane.Left)
        {
			angle = -ROTATION_ANGLE;
			newLane = _occupiedLane == Lane.Right ? Lane.Middle : Lane.Left;
		}

		if (angle != 0)
        {
			ChangeRotation(angle);
			_faceFrontCoroutine = StartCoroutine(FaceFront(newLane));
		}
	}

	private IEnumerator FaceFront(Lane newLane)
    {
		yield return new WaitForSeconds(_runRotatedTime);

		ChangeRotation(0);
		_occupiedLane = newLane;
	}

	private void ChangeRotation(float angle)
    {
		_startRotation = transform.rotation;
		_targetRotation = Quaternion.AngleAxis(angle, transform.up);
		_timeCount = 0;
		_rotating = true;
	}

	private void Jump()
    {
		if (anim.IsInTransition(0) == false)
		{
			rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
			anim.SetTrigger("Jump");
		}
	}

	private void Slide()
    {

    }
}
