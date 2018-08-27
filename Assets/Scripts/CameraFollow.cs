using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform player;
	public Transform camPos;
	[Range(0,100)]
	public int swing = 30;
	public float multiplier = 0.003f;

	public bool roller = true;

	[Range(0.0f,3.0f)]
	public float scrollSensitivity = 1.5f;
	Vector3 orgPos;
	Vector3 dir;
	Vector3 dis;
	Vector3 tag2;
	Vector3 dir2;
	// Use this for initialization
	void Start () {
		transform.position = camPos.position;
		orgPos = player.position;
		dir = Vector3.zero;
		dis = player.position - transform.position;
		tag2 = transform.position;
		transform.LookAt (player.position);
	}
	
	// Update is called once per frame
	void Update () {
		dir = player.position-orgPos;

		Vector3 camTagPos = transform.position + dir;
				
		orgPos = Vector3.Lerp (orgPos,player.position,Mathf.Clamp(swing,0,100)*multiplier);
		Vector3 temp = transform.position;
		transform.position = Vector3.Lerp (transform.position,camTagPos,Mathf.Clamp(swing,0,100)*multiplier);
		dir2 = transform.position - temp;

		if (roller)
		if (Input.GetAxisRaw ("Mouse ScrollWheel") > 0) {
			tag2 = transform.position + transform.forward * scrollSensitivity;
			transform.position = Vector3.Lerp (transform.position, tag2, 0.03f);
		} else if (Input.GetAxisRaw ("Mouse ScrollWheel") < 0) {
			tag2 = transform.position - transform.forward * scrollSensitivity;
			transform.position = Vector3.Lerp (transform.position, tag2,  0.03f);
		} else if (Input.GetAxisRaw ("Mouse ScrollWheel") == 0) {
			transform.position = Vector3.Lerp (transform.position, tag2 + dir2,  0.03f);
			tag2 = tag2 + dir2;
		}
	}

	void FixedUpdate(){
		
	}
}
