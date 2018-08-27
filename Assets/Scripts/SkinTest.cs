using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinTest : MonoBehaviour {

	public SkinnedMeshRenderer tp;
	public SkinnedMeshRenderer src;
	public SkinnedMeshRenderer des;
	//public Transform rootbone;
	public Material[] m;
	Mesh target;
	Mesh temp;
	Transform[] bones;
	// Use this for initialization
	void Start () {
		
		bones = GetComponentsInChildren<Transform>();
//		Debug.Log ("..................");
//		Debug.Log (des.bones.Length);
//		for (int i = 0; i < des.bones.Length; i++) {
//			Debug.Log (des.bones[i].position+".."+des.bones[i].name);
//		}
	}
	
	// Update is called once per frame
	void Update () {
		

		if (Input.GetKeyDown(KeyCode.Space)) {
			List<Transform> bs=new List<Transform>();
			src.sharedMesh = des.sharedMesh;

			Debug.Log ("before bones transform changed..................");
			Debug.Log (src.bones.Length);
			for (int i = 0; i < src.bones.Length; i++) {
				Debug.Log (src.bones[i].position+".."+src.bones[i].name);
			}

			for (int i = 0; i < des.bones.Length; i++) {
			
				for (int j = 0; j < bones.Length; j++) {
				
				
					if (bones [j].name.Equals (des.bones [i].name)) {
						bs.Add (bones [j]);
						break;
					}
				}
			
			}
			src.bones = bs.ToArray();
			//Debug.Log (des.materials.Length);
			src.materials = des.materials;
			Debug.Log ("after bones transform changed..................");
			Debug.Log (src.bones.Length);
			for (int i = 0; i < src.bones.Length; i++) {
				Debug.Log (src.bones[i].position+".."+src.bones[i].name);
			}
		}
	}
}
