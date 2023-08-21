using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UnityChanController : MonoBehaviour, IDifficultyUpdater
{
	private const string JUMP_TRIGGER = "Jump";
	private const string SLIDE_TRIGGER = "Slide";
	private const string LOSE_TRIGGER = "Lose";

	[SerializeField] private float _animSpeed = 1.5f;
	[SerializeField] private float _jumpPower = 5f;
	[SerializeField] private float _fallPower = 3f;
	[SerializeField] private PlayerAudio _playerAudio;
	[Header("Move speed")]
	[SerializeField] private float _moveSpeedStart = 5f;
	[SerializeField] private float _moveSpeedEnd = 7f;
	[SerializeField] private float _moveSpeedStep = 0.5f;
	[Header("Components")]
	[SerializeField] private CapsuleCollider _col;
	[SerializeField] private Rigidbody _rb;
	[SerializeField] private Animator _anim;
	[Header("Events")]
	[SerializeField] private UnityEvent _nextPartEvent;
	[SerializeField] private UnityEvent _lostEvent;

	private Coroutine _jumpCoroutine;
	private Coroutine _slideCoroutine;

	private float _moveSpeed;

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
	private bool _canJump = true;
	private bool _jumped;

	public void Initialize()
    {
		_moveSpeed = _moveSpeedStart;
		_anim.speed = _animSpeed;
		_anim.SetFloat("Speed", 1f);

		_initColHeight = _col.height;
		_initColCenter = _col.center;

		_lost = false;
		_initialized = true;

		PlayAudio(AudioClipType.Start);
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

	public void PlayAudio(AudioClipType type)
    {
		_playerAudio.PlayClip(type);
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
		if (collision.gameObject.CompareTag("Obstacle"))
		{
			_anim.SetTrigger(LOSE_TRIGGER);
			_lost = true;
			_lostEvent.Invoke();
		}
		else if (collision.gameObject.CompareTag("Ground"))
        {
			_canJump = true;
        }
    }

    protected void OnCollisionExit(Collision collision)
    {
		if (_jumped && collision.gameObject.CompareTag("Ground"))
		{
			_canJump = false;
			_jumped = false;
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
		Lane _newLanePrev = _newLane;

		if (direction == Direction.Right && _occupiedLane != Lane.Right)
        {
			_newLane = _occupiedLane == Lane.Left ? Lane.Middle : Lane.Right;
			if(_newLane == _newLanePrev)
            {
				_newLane = Lane.Right;
            }
        }
		else if (direction == Direction.Left && _occupiedLane != Lane.Left)
        {
			_newLane = _occupiedLane == Lane.Right ? Lane.Middle : Lane.Left;
			if (_newLane == _newLanePrev)
			{
				_newLane = Lane.Left;
			}
		}
		else
        {
			_newLane = _occupiedLane;
        }
	}

	private void Jump()
    {
		if (_anim.IsInTransition(0) == false && _canJump)
		{
			_rb.AddForce(Vector3.up * _jumpPower, ForceMode.VelocityChange);
			_anim.SetTrigger(JUMP_TRIGGER);
			_jumpCoroutine = null;
			_jumped = true;

			PlayAudio(AudioClipType.Jump);
		}
		else if(_jumpCoroutine == null)
        {
			_jumpCoroutine = StartCoroutine(JumpWhenPossible());
        }
	}

	private IEnumerator JumpWhenPossible()
    {
		yield return new WaitUntil(() => _anim.IsInTransition(0) == false && _canJump);
		Jump();
    }

	private void Slide()
    {
		if (_anim.IsInTransition(0) == false)
		{
			if(_canJump)
            {
				_anim.SetTrigger(SLIDE_TRIGGER);

				PlayAudio(AudioClipType.Slide);
			}
			else
            {
				_rb.AddForce(Vector3.down * _fallPower, ForceMode.VelocityChange);
			}

			_slideCoroutine = null;
		}
		else if(_slideCoroutine == null)
        {
			_slideCoroutine = StartCoroutine(SlideWhenPossible());
        }
	}

	private IEnumerator SlideWhenPossible()
    {
		yield return new WaitUntil(() => _anim.IsInTransition(0) == false && _canJump);
		Slide();
	}

    public void IncreaseDifficulty()
    {
        if(_moveSpeed < _moveSpeedEnd)
        {
			_moveSpeed += _moveSpeedStep;
        }
    }
}
