using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JumpType{
	HIGH,
	LOW,
	OVER,
	NONE
}

public class JumpSystem : MonoBehaviour {

	public Transform player;
	public Animator animator;
	public float lowHitHeight=1.2f;
	public float hitHeight = 2f;
	public float maxHitHeight=3.0f;

	public float turnOverWidth = 0.5f;
	public float lineDurationTime=5.0f;
	private Vector3 matchPoint ;

	[Range(0f,0.2f)]
	public float targetOffsetZ=0.08f;
	[Range(0f,0.2f)]
	public float targetOffsetY=0.08f;

	[Range(1f,2f)]
	public float fwdHitDistance=1.5f;
	public float playerHeight = 2.0f;

	public JumpType type = JumpType.NONE;

	RaycastHit hit1;
	RaycastHit hit2;
	// Use this for initialization
	void Start () {
		matchPoint = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){
		//CheckHit ();
	
	}


	public Vector3 CheckHit(){
		matchPoint = Vector3.zero;

		//Debug.DrawLine (transform.position,transform.position + transform.up * hitHeight,Color.yellow,lineDurationTime);
		//Debug.DrawLine(transform.position + transform.up * hitHeight,transform.position + transform.up * hitHeight+transform.forward*fwdHitDistance,Color.red,lineDurationTime);
		Debug.DrawLine(transform.position + transform.up * lowHitHeight,transform.position + transform.up * lowHitHeight+transform.forward*fwdHitDistance,Color.red,lineDurationTime/2);
		if (Physics.Raycast (transform.position + transform.up * lowHitHeight, transform.forward, out hit1,fwdHitDistance)) {
			ResetPlayerPosition (hit1.point);
			ClimbHighCheck ();
		} else {
//			if (Physics.Raycast (transform.position + transform.up * lowHitHeight, transform.forward, out hit2, fwdHitDistance))
//				JumpMidCheck ();
//			else
			JumpLowCheck ();
		}

		return matchPoint;
	}

	void ResetPlayerPosition(Vector3 hitPoint){
		if((hit1.point - (transform.position + transform.up * hitHeight)).magnitude<1.0f)
			player.position=player.position-player.forward*(1.0f-(hit1.point - (transform.position + transform.up * hitHeight)).magnitude+0.02f);
	}


//	void JumpMidCheck (){
//		RaycastHit hit;
//		Debug.DrawLine (transform.position ,transform.position + transform.up * lowHitHeight,Color.yellow,lineDurationTime);
//		Debug.DrawLine (transform.position + transform.up * lowHitHeight,transform.position + transform.up * lowHitHeight+transform.forward*fwdHitDistance,Color.red,lineDurationTime);
//	
//		Vector3 start = transform.position + transform.up * hitHeight + transform.forward * fwdHitDistance;
//
//		if (Physics.Raycast (start, Vector3.down, out hit, 20.0f)) {
//			Debug.DrawLine (start, hit.point, Color.green,lineDurationTime);
//			//如果平台上方的空间尺寸小于游戏角色高度，结束方法
//			if (Physics.Raycast (hit.point + Vector3.up * targetOffsetY, Vector3.up, playerHeight))
//				return;
//
//			RaycastHit hit2;
//			if (!Physics.Raycast (hit.point + Vector3.up * targetOffsetY, -transform.forward, out hit2, fwdHitDistance,~(1<<8))) {
//				float dis = (hit1.point - (transform.position + transform.up * hitHeight)).magnitude;
//				ResetPlayerPosition (hit1.point);
//				matchPoint= hit.point + Vector3.up * targetOffsetY - transform.forward * (fwdHitDistance - dis+targetOffsetZ);
//				Debug.DrawLine (hit.point + Vector3.up * targetOffsetY, matchPoint, Color.blue,lineDurationTime);
//			} 
//
//		}
//	}



	void ClimbHighCheck(){
		
		RaycastHit hit;
		Debug.DrawLine (transform.position + transform.up * hitHeight,transform.position + transform.up * maxHitHeight,Color.yellow,lineDurationTime);
		Debug.DrawLine (transform.position + transform.up * maxHitHeight,transform.position + transform.up * maxHitHeight+transform.forward*fwdHitDistance,Color.red,lineDurationTime);
		if (Physics.Raycast (transform.position + transform.up * maxHitHeight, transform.forward, fwdHitDistance))
			return ;
		
		Vector3 start = transform.position + transform.up * maxHitHeight + transform.forward * fwdHitDistance;
		if (Physics.Raycast (start, Vector3.down, out hit, 20.0f)) {
			Debug.DrawLine (start, hit.point, Color.green,lineDurationTime);
			//如果平台上方的空间尺寸小于游戏角色高度，结束方法
			Debug.DrawLine (hit.point + Vector3.up * targetOffsetY, hit.point + Vector3.up * targetOffsetY + Vector3.up * playerHeight,Color.black,lineDurationTime);
			if (Physics.Raycast (hit.point + Vector3.up * targetOffsetY, Vector3.up, playerHeight)) {
				return;
			}

			RaycastHit hit2;
			if (!Physics.Raycast (hit.point + Vector3.up * targetOffsetY, -transform.forward, out hit2, fwdHitDistance,~(1<<8))) {
				float dis = (hit1.point - (transform.position + transform.up * hitHeight)).magnitude;
				type = JumpType.HIGH;
				matchPoint= hit.point + Vector3.up * targetOffsetY - transform.forward * (fwdHitDistance - dis+targetOffsetZ)+transform.right*0.3f;
				Debug.DrawLine (hit.point + Vector3.up * targetOffsetY, matchPoint, Color.blue,lineDurationTime);
				Debug.Log ("...JumpHigh"+matchPoint);
			} 

		} 

		
	}

	void JumpLowCheck(){
		RaycastHit hitTest;
		Debug.DrawLine (transform.position + transform.up * 0.1f,transform.position + transform.up * 0.1f+transform.forward*fwdHitDistance,Color.white,lineDurationTime);

		if (!Physics.Raycast (transform.position + transform.up * 0.1f, transform.forward, out hitTest, fwdHitDistance))
			return;
		//ResetPlayerPosition (hitTest.point);

		RaycastHit hit;
		Debug.DrawLine (transform.position,transform.position + transform.up * lowHitHeight,Color.yellow,lineDurationTime);
		Debug.DrawLine (transform.position + transform.up * lowHitHeight,transform.position + transform.up * lowHitHeight+transform.forward*fwdHitDistance*0.5f,Color.red,lineDurationTime);

		Vector3 start = transform.position + transform.up * lowHitHeight + transform.forward * fwdHitDistance*0.9f;
		float dis = (hitTest.point - (transform.position + transform.up * 0.1f)).magnitude;
		if (Physics.Raycast (start, Vector3.down, out hit, lowHitHeight-0.01f)) {
			Debug.DrawLine (start, hit.point, Color.red,lineDurationTime);
			//如果平台上方的空间尺寸小于游戏角色高度，结束方法
			Debug.DrawLine (hit.point + Vector3.up * targetOffsetY, hit.point + Vector3.up * targetOffsetY + Vector3.up * playerHeight,Color.black,lineDurationTime);
			if (Physics.Raycast (hit.point + Vector3.up * targetOffsetY, Vector3.up, playerHeight))
				return;

			RaycastHit hit2;
			if (!Physics.Raycast (hit.point , -transform.forward, out hit2, fwdHitDistance*0.5f, ~(1 << 8))) {
				type = JumpType.LOW;
				matchPoint = hit.point + Vector3.up * 0.1f - transform.forward * (fwdHitDistance*0.5f - dis)/2f;
				Debug.DrawLine (hit.point + Vector3.up * 0.1f, matchPoint, Color.blue, lineDurationTime);
				Debug.Log ("...JumpLow"+matchPoint);

			} 

		} else if (Physics.Raycast (transform.position + transform.up * lowHitHeight + transform.forward * (dis + 0.01f), Vector3.down, out hit, lowHitHeight-0.01f)) {
			Debug.DrawLine(transform.position + transform.up * lowHitHeight + transform.forward * (dis + 0.01f),hit.point,Color.green,lineDurationTime);
			type = JumpType.OVER;
			matchPoint = hit.point+Vector3.up*0.1f-transform.forward*0.3f;
			Debug.DrawLine (hit.point+Vector3.up*targetOffsetY,matchPoint,Color.blue,lineDurationTime);
			Debug.Log ("...OverRailing"+matchPoint);

		} else
			return;
	
	}
}
