using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHover : MonoBehaviour {
	public BuildSystem buildSys;
	public int width=958;
	public int height=504;
	Vector2 pos;

	void Update(){
		//transform.localPosition = new Vector3 (0f,0f,0f);
		//Debug.Log ("--:"+uiCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,0f)));

	}
	public void OnMouseOver(Transform obj) {
//		Debug.Log (Screen.width+"---"+Screen.height+"---"+Input.mousePosition);
		pos.x = (Input.mousePosition.x - (Screen.width/2))* ((float)width / (float)Screen.width);
//		pos.y = (Input.mousePosition.y - (Screen.height/2))* ((float)height / (float)Screen.height);
//
//		if (Input.mousePosition.x - (Screen.width/2) < 0f)
//			pos.x = -pos.x;
//		if (Input.mousePosition.y - (Screen.height/2) < 0f)
//			pos.y = -pos.y;
		ComponentInfo info =buildSys.GetComponentInfo(obj.name);
		UILabel label = GetComponentInChildren<UILabel> ();
		label.text = "";
		for (int i = 0; i < info.requiredGoods.Length; i++) {
			label.text += info.requiredGoods [i] + ":" + info.requiredGoodsNum [i];
			if(i!=info.requiredGoods.Length-1)
				label.text+="\n";
		}

		Debug.Log (pos);
		transform.localPosition = new Vector3 (pos.x,transform.localPosition.y,0f);
	}
}
