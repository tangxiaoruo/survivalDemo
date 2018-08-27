using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WeaponType{
	AK,
	M16,
	KNIFE,
	SWORD,
	NULL
}

public class PlayerController : MonoBehaviour {

	public Animator animator;
	public WeaponType weaponType=WeaponType.NULL;
	public UIController uiController;
	public Transform E;
	public JumpSystem jumpSystem;

	public  Transform tagPos;
	[Range(0.23f,0.35f)]
	public float endFrame=0.29f;
	public float turnSpeed=15.0f;
	public float walkSpeed=1.0f;
	public float runSpeed=3.0f;

	public Transform collisionPos;
	public float hitDistance=1;

	private float x;
	private float z;
	private float speed=0f;
	private bool withGun=false;
	private bool aiming = false;
	[HideInInspector]
	public bool shooting = false;
	public bool canMove = true;
	private float timer=0f;

	private float climbTimer=0f;
	public bool climbing=false;
	private bool haveRig=true;
	private bool hitSomething=false;
	private bool matchStarted=false;

	private AnimatorClipInfo[] clipsInfo;
	private float targetHeight=0f;
	private Ray dir;
	// Use this for initialization
	void Start () {
		clipsInfo = animator.GetCurrentAnimatorClipInfo (0);
		Debug.Log ("1111111111:"+clipsInfo[0].clip.name);

	}

	void OnDisable(){
		animator.SetBool ("walk", false);
		animator.SetBool ("run", false);
	}
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
	
		x = Input.GetAxisRaw ("Horizontal");
		z = Input.GetAxisRaw ("Vertical");
		if (canMove) {

			if (Input.GetKey (KeyCode.LeftShift)) {
				speed = walkSpeed;
				if (Mathf.Abs (x) == 1 || Mathf.Abs (z) == 1) {
					animator.SetBool ("walk", true);
					animator.SetBool ("run", false);
				} else {
					animator.SetBool ("walk", false);
					animator.SetBool ("run", false);
				}
			} else {
				speed = runSpeed;
				if (Mathf.Abs (x) == 1 || Mathf.Abs (z) == 1) {
					animator.SetBool ("run", true);
					animator.SetBool ("walk", false);
				} else {
					animator.SetBool ("walk", false);
					animator.SetBool ("run", false);
				}
			}


		
			Turn ();
			if ((x != 0 || z != 0)&&!hitSomething) {
				transform.Translate (Vector3.forward*speed*Time.deltaTime,Space.Self);
			}
		}

	


		if (Input.GetKeyDown (KeyCode.Space)&&!climbing) {
			tagPos.position = jumpSystem.CheckHit ();
			Debug.Log ("....."+tagPos.position);

			if(tagPos.position!=Vector3.zero){
				targetHeight = (tagPos.position - transform.position).y;
				Debug.Log (".,.,.,.,.,,.,.,."+targetHeight);

				Destroy (transform.GetComponent<Rigidbody> ());
				climbing = true;
				canMove = false;
				climbTimer = 0f;
				haveRig = false;

				if (targetHeight >= jumpSystem.hitHeight)
					animator.SetTrigger ("climbHigh");
				else if (targetHeight >= jumpSystem.lowHitHeight)
					animator.SetTrigger ("climbMid");
				else {
					if (jumpSystem.type == JumpType.LOW)
						animator.SetTrigger ("jumpLow");
					else if (jumpSystem.type == JumpType.OVER)
						animator.SetTrigger ("overRailing");
				}


				//	animator.MatchTarget (tagPos.position, tagPos.rotation, AvatarTarget.RightHand, new MatchTargetWeightMask (Vector3.one, 1.0f), 0.3f, 1.0f);
			}
		}

		if (climbing) 
			climbTimer += Time.deltaTime;


		if (!matchStarted&&climbing&&!animator.IsInTransition(0)&&(!animator.GetCurrentAnimatorStateInfo (0).IsName ("BlendTree1")&&!animator.GetCurrentAnimatorStateInfo (0).IsName ("BlendTree2")&&!animator.GetCurrentAnimatorStateInfo (0).IsName ("BlendTree3"))) {
			Debug.Log ("........."+tagPos.position);
			matchStarted = true;

			Debug.Log ("match:"+animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
			clipsInfo = animator.GetCurrentAnimatorClipInfo(0);
			Debug.Log ("match:" + clipsInfo [0].clip.name);

			if (targetHeight >= jumpSystem.hitHeight) {
			Debug.Log("------"+tagPos.position);
				animator.MatchTarget (tagPos.position, tagPos.rotation, AvatarTarget.RightHand, new MatchTargetWeightMask (Vector3.one, 0f), 0.2f, endFrame);
				Debug.Log ("match1");
			} else if (targetHeight >= jumpSystem.lowHitHeight) {
				Debug.Log ("match2");

				animator.MatchTarget (tagPos.position, tagPos.rotation, AvatarTarget.RightHand, new MatchTargetWeightMask (Vector3.one, 0f), 0.26f, 0.44f);
			
			}else {
				if (jumpSystem.type == JumpType.LOW) {
					Debug.Log ("match3");

					animator.MatchTarget (tagPos.position, tagPos.rotation, AvatarTarget.RightFoot, new MatchTargetWeightMask (Vector3.one, 0f), 0.47f, 0.75f);

				} else if (jumpSystem.type == JumpType.OVER) {
					Debug.Log ("match4");

					animator.MatchTarget (tagPos.position, tagPos.rotation, AvatarTarget.LeftHand, new MatchTargetWeightMask (Vector3.one, 0f), 0.11f, 0.23f);
						
				}
			}
		}

		if (climbTimer>clipsInfo[0].clip.length-0.1f && animator.GetCurrentAnimatorStateInfo (0).IsName ("BlendTree1")) {
			Debug.Log ("add rigidbody:"+climbTimer+"clip.length:"+(clipsInfo[0].clip.length-0.1f)+"name:"+clipsInfo[0].clip.name);
			climbing = false;
			canMove = true;
			matchStarted = false;
			if (!haveRig) {
				Rigidbody rigidbody = transform.gameObject.AddComponent<Rigidbody> ();
				rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
				haveRig = true;
			}
			climbTimer = 0f;
			Debug.ClearDeveloperConsole ();
		}


		if (weaponType == WeaponType.KNIFE || weaponType == WeaponType.SWORD||weaponType==WeaponType.NULL) {
			if (withGun) {
				animator.SetFloat ("Blend",1.0f);
				withGun = false;
			}

			if (Input.GetMouseButtonDown (0)&&timer>1.06f&&(weaponType == WeaponType.KNIFE || weaponType == WeaponType.SWORD)) {
				canMove = false;
				animator.SetTrigger ("attack");
				FaceCursor ();
				timer = 0f;
				}
			if (timer > 1.07f)
				canMove = true;
		}


		if (weaponType == WeaponType.AK || weaponType == WeaponType.M16) {

			if (!withGun) {
				animator.SetFloat ("Blend", 0.0f);
				withGun = true;
			}

			if (aiming || shooting)
				FaceCursor ();

			if (Input.GetMouseButtonDown (0)) {
				canMove = false;
				animator.SetTrigger ("shoot");
				animator.SetBool ("shooting",true);
				shooting = true;
			}

			if (Input.GetMouseButtonUp (0)) {
				if (aiming)
					animator.SetTrigger ("aiming");
				else {
					canMove = true;
					animator.SetBool ("shooting", false);
				}
				shooting = false;
			}


			if (Input.GetMouseButtonDown (1)) {
				canMove = false;
				animator.SetTrigger ("aiming");
				animator.SetBool ("endAiming",false);
				aiming = true;
			}
			
			if (Input.GetMouseButtonUp (1)) {
				canMove = true;
				animator.SetBool("endAiming",true);
				aiming = false;
			}
		} 


		CheckCollision ();


	}




//	IEnumerator ClimbAni(){
//		yield return  new WaitForSeconds(0.2f);
//		animator.MatchTarget (tagPos.position, tagPos.rotation, AvatarTarget.RightHand, new MatchTargetWeightMask (Vector3.one,0f), 0.4f, 1.0f);
//	}


	void FaceCursor(){
		
		dir = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (dir, out hit)) {
			Vector3 d = new Vector3 (hit.point.x, transform.position.y, hit.point.z) - transform.position;
			float angle = 0;
			if (transform.InverseTransformPoint (new Vector3 (hit.point.x, transform.position.y, hit.point.z)).x < 0)
				angle = 360.0f - Vector3.Angle (transform.forward, d);
			else
				angle=Vector3.Angle (transform.forward, d);
			transform.Rotate (new Vector3(0,angle,0));
		}

	}

	void Turn(){

		if (z == 0 && x == 0)
			return;

		Vector3 dir = new Vector3 (x,0,z);

		transform.rotation = Quaternion.Slerp (transform.rotation,Quaternion.LookRotation(dir),Time.deltaTime*turnSpeed);
	}


	void CheckCollision(){
		RaycastHit hit;
		if(Physics.BoxCast(collisionPos.position,new Vector3(0.3f,0.9f,0.5f),transform.forward,out hit,transform.rotation,0.5f)){
			Debug.DrawLine (collisionPos.position,collisionPos.position+transform.forward,Color.green);
//		if (Physics.Raycast (collisionPos.position, transform.forward, out hit, hitDistance)) {
			Transform obj = hit.transform;

			if (obj.tag == "resource") {
				E.gameObject.SetActive (true);
				E.position = transform.position + Vector3.up*1.6f+transform.forward;
				if (Input.GetKeyDown (KeyCode.E)) {
					E.gameObject.SetActive (false);
					uiController.GenerateGoods (obj.GetComponent <GoodsInfo> ());
					uiController.ShowUI (true);
				}
				hitSomething = true;
			} else {
				if(obj.tag!="trigger"&&obj.tag!="terrain")
					hitSomething = true;
			}



		} else {
			Debug.DrawLine (collisionPos.position,collisionPos.position+transform.forward,Color.red);
			hitSomething = false;
			E.gameObject.SetActive (false);
		}
			
	}
}
