using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public  enum StateType{
	ATTACK,
	IDLE,
	DEAD
}

public class ZombieAI : MonoBehaviour {
	
	public NavMeshAgent agent;
	public Animator animator;
	private StateType state = StateType.IDLE;
	public float alertDistance=8.0f;
	public float loseDistance = 16.0f;
	public float attackDistance=1.0f;
	private bool attackState = false;
	public bool isAttacking=false;
	public float repeatTime=8.0f;
	private Vector3 dis;
	private bool idleWalking=false;
	private bool invoking = false;
	Transform player=null;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("player").transform;
	}
	
	// Update is called once per frame
	void Update () {

		if (state == StateType.ATTACK) {
			CancelInvoke ();
			invoking = false;
			AtatckState ();
		}

		if (state == StateType.IDLE) {
			IdleState ();
		}

		if (player != null) {
			dis = player.position - transform.position;
			if (dis.magnitude < alertDistance) {
				state = StateType.ATTACK;
				attackState = true;
			}


			if (dis.magnitude > loseDistance && attackState) {
				state = StateType.IDLE;
				attackState = false;

			}

			if (state == StateType.IDLE) {
				Debug.DrawLine (transform.position + Vector3.up, transform.position + Vector3.up + transform.forward * 1.0f, Color.red);
				if (Physics.Raycast (transform.position + Vector3.up, transform.forward, 1.0f) && idleWalking) {
					animator.SetBool ("idle2walk", false);
					idleWalking = false;
				}
			}
		}
	}

	void AtatckState(){
		if (dis.magnitude > attackDistance) {
			if (dis.magnitude < attackDistance + 0.5f)
				;
			else {
				agent.enabled = true;
				agent.destination = player.position;
				if (isAttacking) {
					animator.SetTrigger ("idleTri");
					isAttacking = false;
				}
				animator.SetBool ("idle2walk", true);
			}
		}
		else {
			transform.LookAt (player.position);
			if (!isAttacking) {
				agent.enabled=false;
				animator.SetTrigger ("attack");
				isAttacking = true;
			}
		}
	
	}


	void IdleState(){
		
		if (!invoking) {
			agent.enabled = false;
			animator.SetTrigger ("idleTri");
			animator.SetBool ("idle2walk",false);
			InvokeRepeating ("RandomStateIdle", repeatTime, repeatTime);
			invoking = true;
		} 

	}

	void RandomStateIdle(){
		if (Random.value > 0.5f) {
			transform.Rotate (new Vector3 (0, Random.Range (-180.0f, 180.0f), 0), Space.Self);
			animator.SetBool ("idle2walk", false);
			idleWalking = false;
		} else {
			transform.Rotate (new Vector3 (0, Random.Range (-180.0f, 180.0f), 0), Space.Self);
			animator.SetBool ("idle2walk", true);
			idleWalking = true;
		}

	}


}
