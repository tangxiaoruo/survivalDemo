using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeThrough : MonoBehaviour {
	public Transform targetPos;
	public Transform mask;

	List<GameObject> preObjList = new List<GameObject> ();
	RaycastHit[] hit;
	Vector3 dir;
	Vector3 beginPos;
	bool played = false;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		beginPos=targetPos.position+Vector3.up*1.5f+(transform.position-(targetPos.position+Vector3.up*1.5f))*2.0f;
		dir = targetPos.position+Vector3.up*1.5f-beginPos;
		Debug.DrawLine (beginPos,targetPos.position+Vector3.up*1.5f,Color.red);
		if (Physics.Raycast (new Ray (beginPos, dir), dir.magnitude - 0.3f, ~(1 << 8))) {
			//hit = Physics.RaycastAll (new Ray (beginPos, dir), dir.magnitude - 0.3f, ~(1 << 8));
			hit = Physics.CapsuleCastAll (beginPos, beginPos + Vector3.down * 1.11f, 0.4f, dir, dir.magnitude - 0.3f, ~(1 << 8));
		}
		else
			hit = new RaycastHit[0];

		if (hit.Length == 0 && played) {
			mask.GetComponent<Animator> ().SetTrigger ("back");
			played = false;
		} else if (hit.Length != 0&&!played) {
			mask.GetComponent<Animator> ().SetTrigger ("forward");
			played = true;
		}



			foreach (GameObject obj in preObjList) {
				bool b = false;
				for (int i = 0; i < hit.Length; i++) {
					if (hit [i].transform.name == obj.name)
						b = true;
				}
				if (b == false) {
					obj.GetComponent<MeshRenderer> ().material.renderQueue = 2000;
					preObjList.Remove (obj);
				}
			}

			foreach (RaycastHit h in hit) {
				bool b = false;
				foreach (GameObject obj in preObjList) {
					if (obj.name == h.transform.name)
						b = true;
				}
				if (b == false) {
					h.transform.GetComponent<MeshRenderer> ().material.renderQueue = 3100;
					preObjList.Add (h.transform.gameObject);
				}
			}
	}
}
