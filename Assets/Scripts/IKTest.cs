using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKTest : MonoBehaviour {
	public Transform tagPos;
	public Animator animator;
	public bool climbing = false;
	// Use this for initialization
	void Start () {
		//animator.MatchTarget (tagPos.position,tagPos.rotation,AvatarTarget.LeftHand,new MatchTargetWeightMask(Vector3.one,1.0f),0.1f,1.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)&&!climbing) {
			animator.SetTrigger ("climbHigh");
			climbing = true;
			//	animator.MatchTarget (tagPos.position, tagPos.rotation, AvatarTarget.RightHand, new MatchTargetWeightMask (Vector3.one, 1.0f), 0.3f, 1.0f);

		}
		if (climbing&&animator.GetCurrentAnimatorStateInfo(0).IsName("Idle_ToJumpUpHigh")&&!animator.IsInTransition(0)) {
			animator.MatchTarget (tagPos.position, tagPos.rotation, AvatarTarget.RightHand, new MatchTargetWeightMask (Vector3.one,0f), 0.21f,0.26f);
			climbing = false;
		}
	}

	void OnAnimatorIK(){
		if (Input.GetKey (KeyCode.Mouse1)) {
			Debug.Log ("ppppp");
			animator.SetIKPositionWeight (AvatarIKGoal.RightHand,0.8f);
			animator.SetIKPosition (AvatarIKGoal.RightHand,tagPos.position);
			animator.SetIKRotationWeight (AvatarIKGoal.RightHand,0.8f);
			animator.SetIKRotation (AvatarIKGoal.RightHand,tagPos.rotation);


		}
	}
}
