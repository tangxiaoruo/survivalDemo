using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {

	Camera cam;
	Vector3 dis;
	// Use this for initialization
	void OnEnable(){
		cam = Camera.main;
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		dis = cam.transform.position - transform.position;
		dis.x = 0f;
		transform.rotation = Quaternion.LookRotation (-dis);
	}
}
