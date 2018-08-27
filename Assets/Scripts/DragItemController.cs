using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragItemController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		InvokeRepeating ("CheckBuildPanel",1.0f,0.1f);
	}

	// Update is called once per frame
	void Update () {
		
	}

	void CheckBuildPanel(){
		if (Camera.main.GetComponent<UIController> ().buildPanelOpen)
			GetComponent<DragDropItem> ().enabled = false;
		else
			GetComponent<DragDropItem> ().enabled = true;
	}
}
