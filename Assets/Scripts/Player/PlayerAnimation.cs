using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
	private Animator mAnimator;

	void Start()
	{
		mAnimator = gameObject.GetComponent<Animator>();
	}
    private void Update()
    {
		MovingAnim();
		ShootingAnim();
		IdleAnim();
    }

    public void MovingAnim()
    {
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
			mAnimator.SetBool("Walking", true);
			mAnimator.SetBool("Shooting", false);
			mAnimator.SetBool("Idle", false);
        }
    }
	public void ShootingAnim()
    {
		if (Input.GetKeyDown(KeyCode.Mouse0))
        {
			mAnimator.SetBool("Shooting", true);
			mAnimator.SetBool("Walking", false);
			mAnimator.SetBool("Idle", false);
        }
    }
	public void IdleAnim()
    {
		if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.Mouse0)))
		{
			mAnimator.SetBool("Shooting", false);
			mAnimator.SetBool("Walking", false);
			mAnimator.SetBool("Idle", true);
		}
	}
}
