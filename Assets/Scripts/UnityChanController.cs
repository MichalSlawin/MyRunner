using UnityEngine;
using UnityEngine.Events;

public class UnityChanController : MonoBehaviour
{
	private const string JUMP_TRIGGER = "Jump";
	private const string SLIDE_TRIGGER = "Slide";
	private const string LOSE_TRIGGER = "Lose";

	[SerializeField] private float animSpeed = 1.5f;
	[SerializeField] private float jumpPower = 3.0f;
	[SerializeField] private float _moveSpeed = 7f;
	[SerializeField] private CapsuleCollider _col;
	[SerializeField] private Rigidbody _rb;
	[SerializeField] private Animator _anim;
	[SerializeField] private UnityEvent _nextPartEvent;
	[SerializeField] private UnityEvent _lostEvent;

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

	private Lane _occupiedLane = Lane.Middle;
	private Lane _newLane = Lane.Middle;
	private Direction _direction;

	private bool _lost;
	private bool _initialized;
	private float _initColHeight;
	private Vector3 _initColCenter;

	private bool _canPlay => _lost == false && _initialized;

	public void Initialize()
    {
		_anim.speed = animSpeed;
		_anim.SetFloat("Speed", 1f);

		_initColHeight = _col.height;
		_initColCenter = _col.center;

		_lost = false;
		_initialized = true;
	}

    protected void FixedUpdate()
    {
		if(_canPlay == false)
        {
			return;
        }

		Vector3 direction = new Vector3(GetNewDirectionX(), 0, 1);
		_rb.MovePosition(transform.position + direction * Time.deltaTime * _moveSpeed);
	}

	private float GetNewDirectionX()
    {
		if(_newLane == _occupiedLane)
        {
			return 0;
        }

		if (_newLane == Lane.Left && transform.position.x > -Game.LANE_OFFSET)
		{
			return -1;
		}

		if(_newLane == Lane.Right && transform.position.x < Game.LANE_OFFSET)
        {
			return 1;
        }

		if(_newLane == Lane.Middle)
        {
			if(_direction == Direction.Left && transform.position.x > 0)
            {
				return -1;
            }
			else if(_direction == Direction.Right && transform.position.x < 0)
            {
				return 1;
            }
        }

		_occupiedLane = _newLane;
		return 0;
	}

	protected void OnCollisionEnter(Collision collision)
    {
		if(collision.gameObject.CompareTag("Obstacle"))
        {
			_anim.SetTrigger(LOSE_TRIGGER);
			_lost = true;
			_lostEvent.Invoke();
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
		if(other.CompareTag("NextPart"))
        {
			_nextPartEvent.Invoke();
		}
    }

    public void OnJump()
    {
		float jumpHeight = _anim.GetFloat("JumpHeight");

		_col.height = _initColHeight - jumpHeight;
		_col.center = new Vector3(0, _initColCenter.y + jumpHeight, 0);
	}

	public void OnJumpFinish()
    {
		ResetCollider();
	}

	public void OnSlide(float heightMultiplier)
    {
		float newHeight = _initColHeight * heightMultiplier;
		float heightDiff = _initColHeight - newHeight;

		_col.height = newHeight;
		_col.center = new Vector3(0, _initColCenter.y - (heightDiff / 2), 0);
	}

	public void OnSlideFinish()
    {
		ResetCollider();
	}

	private void ResetCollider()
	{
		_col.height = _initColHeight;
		_col.center = _initColCenter;
	}

	public void Move(Vector2 direction)
    {
		if(_canPlay == false)
        {
			return;
        }

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
		_direction = direction;
		if (direction == Direction.Right && _occupiedLane != Lane.Right)
        {
			_newLane = _occupiedLane == Lane.Left ? Lane.Middle : Lane.Right;
        }
		else if (direction == Direction.Left && _occupiedLane != Lane.Left)
        {
			_newLane = _occupiedLane == Lane.Right ? Lane.Middle : Lane.Left;
		}
		else
        {
			_newLane = _occupiedLane;
        }
	}

	private void Jump()
    {
		if (_anim.IsInTransition(0) == false)
		{
			_rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
			_anim.SetTrigger(JUMP_TRIGGER);
		}
	}

	private void Slide()
    {
		if (_anim.IsInTransition(0) == false)
		{
			_anim.SetTrigger(SLIDE_TRIGGER);
		}
	}
}
