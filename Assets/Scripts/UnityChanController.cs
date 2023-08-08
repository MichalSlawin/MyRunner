using System.Collections.Generic;
using UnityEngine;

public class UnityChanController : MonoBehaviour
{
	private const string JUMP_TRIGGER = "Jump";
	private const string SLIDE_TRIGGER = "Slide";
	private const string LOSE_TRIGGER = "Lose";
	private const float LANE_OFFSET = 2f;

	[SerializeField] private float animSpeed = 1.5f;
	[SerializeField] private float jumpPower = 3.0f;
	[SerializeField] private float _moveSpeed = 7f;
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

	private Lane _occupiedLane = Lane.Middle;
	private Lane _newLane = Lane.Middle;
	private Direction _direction;

	private bool _lost;
	private float _initColHeight;
	private Vector3 _initColCenter;

	protected void Start()
	{
		anim.speed = animSpeed;
		anim.SetFloat("Speed", 1f);

		_initColHeight = col.height;
		_initColCenter = col.center;

		_lost = false;
	}

    protected void FixedUpdate()
    {
		if(_lost)
        {
			return;
        }

		Vector3 direction = new Vector3(GetNewDirectionX(), 0, 1);
		rb.MovePosition(transform.position + direction * Time.deltaTime * _moveSpeed);
	}

	private float GetNewDirectionX()
    {
		if(_newLane == _occupiedLane)
        {
			return 0;
        }

		if (_newLane == Lane.Left && transform.position.x > -LANE_OFFSET)
		{
			return -1;
		}

		if(_newLane == Lane.Right && transform.position.x < LANE_OFFSET)
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
			anim.SetTrigger(LOSE_TRIGGER);
			_lost = true;
        }
    }

	public void OnJump()
    {
		float jumpHeight = anim.GetFloat("JumpHeight");

		col.height = _initColHeight - jumpHeight;
		col.center = new Vector3(0, _initColCenter.y + jumpHeight, 0);
	}

	public void OnJumpFinish()
    {
		ResetCollider();
	}

	public void OnSlide(float heightMultiplier)
    {
		float newHeight = _initColHeight * heightMultiplier;
		float heightDiff = col.height - newHeight;

		col.height = newHeight;
		col.center = new Vector3(0, _initColCenter.y - heightDiff, 0);
	}

	public void OnSlideFinish()
    {
		ResetCollider();
	}

	private void ResetCollider()
	{
		col.height = _initColHeight;
		col.center = _initColCenter;
	}

	public void Move(Vector2 direction)
    {
		if(_lost)
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
		if (anim.IsInTransition(0) == false)
		{
			rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
			anim.SetTrigger(JUMP_TRIGGER);
		}
	}

	private void Slide()
    {
		if (anim.IsInTransition(0) == false)
		{
			anim.SetTrigger(SLIDE_TRIGGER);
		}
	}
}
