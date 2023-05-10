using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityChanController : MonoBehaviour
{
	[SerializeField] private float animSpeed = 1.5f;
	[SerializeField] private float lookSmoother = 3.0f;
	[SerializeField] private bool useCurves = true;
	[SerializeField] private float useCurvesHeight = 0.5f;
	[SerializeField] private float jumpPower = 3.0f;
	[SerializeField] private CapsuleCollider col;
	[SerializeField] private Rigidbody rb;
	[SerializeField] private Animator anim;

	private Vector3 velocity;
	private float orgColHight;
	private Vector3 orgVectColCenter;
	private AnimatorStateInfo currentBaseState;
	private float _moveSpeed = 7.0f;

	private static int idleState = Animator.StringToHash("Base Layer.Idle");
	private static int locoState = Animator.StringToHash("Base Layer.Locomotion");
	private static int jumpState = Animator.StringToHash("Base Layer.Jump");
	private static int restState = Animator.StringToHash("Base Layer.Rest");

	protected void Start()
	{
		orgColHight = col.height;
		orgVectColCenter = col.center;
		anim.speed = animSpeed;
		anim.SetFloat("Speed", 1f);
	}

    protected void FixedUpdate()
    {
		transform.position += transform.forward * _moveSpeed * Time.deltaTime;
	}

    private void ResetCollider()
	{
		col.height = orgColHight;
		col.center = orgVectColCenter;
	}

	public void Move(Vector2 direction)
    {
		float horizontal = direction.x;
		float vertical = direction.y;
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);

		if (horizontal > 0)
        {

        }
		else if (horizontal < 0)
        {

        }

		if (vertical > 0)
        {
			if (anim.IsInTransition(0) == false)
			{
				rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
				anim.SetTrigger("Jump");
			}
		}
		else if (vertical < 0)
        {

        }
	}
}
