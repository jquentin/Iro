using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBodyController : MonoBehaviour {

	Animator _animator;
	Animator animator
	{
		get
		{
			if (_animator == null)
				_animator = GetComponent<Animator>();
			return _animator;
		}
	}

	public void Shoot()
	{
		animator.SetTrigger("Shoot");
	}
}
