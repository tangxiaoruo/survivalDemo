using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour {
	public GameObject E;
	public BackPagManager backpackManager;
	Transform obj;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnTriggerEnter(Collider other){
		 obj = other.transform;
		if (obj.tag == "trigger") {
			E.gameObject.SetActive (true);
			E.transform.position = obj.transform.position + Vector3.up*0.5f;
		}
	}

	void OnTriggerStay(Collider other){
		obj = other.transform;
		if (obj.tag == "trigger") {
			ItemObj info;

			if (Input.GetKeyDown (KeyCode.E)) {
				if (obj.childCount == 0)
					info = obj.GetComponent<ItemObj> ();
				else
					info = obj.parent.GetComponent<ItemObj> ();

				int n = backpackManager.Increase (info.name, info.number);
				if (n ==0) {
					if (obj.transform.childCount == 0)
						Destroy (obj.gameObject);
					else
						Destroy (obj.parent.gameObject);
					E.gameObject.SetActive (false);
				} else {
					obj.parent.GetComponent<ItemObj> ().number = n;
					//StartCoroutine (backpackManager.AlpheAni());
				}
			}
		}
	}

	void OnTriggerExit(Collider other){	
		obj = other.transform;

		if (obj.tag == "trigger") {
			E.gameObject.SetActive (false);
		}
	}
}
