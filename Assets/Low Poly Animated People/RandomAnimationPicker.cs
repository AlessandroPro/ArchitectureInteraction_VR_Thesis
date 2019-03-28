using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimationPicker : MonoBehaviour {

	Animator anim;
	int AnimChoose;

	float speed ;
	
	// Update is called once per frame
	void Start () {
		
		anim = GetComponent<Animator> ();
		anim.speed = speed = 1 * (Random.Range (.5f, 1.5f));

		AnimChoose = Random.Range(1,3);
		anim.SetInteger("Idle", AnimChoose);
	}
}
