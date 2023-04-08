using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnims : MonoBehaviour
{
	public List<string> mAnimNames = new List<string>();
	private Animator mAnimator;

	void Start()
	{
		mAnimator = gameObject.GetComponent<Animator>();
	}

	public void OnButtonClick( int aIndex )
	{
		mAnimator.Play( mAnimNames[ aIndex ] );
	}

	public void OnButtonClickWithAnimName( string aAnimStateName )
	{
		mAnimator.Play( aAnimStateName );
	}

}
