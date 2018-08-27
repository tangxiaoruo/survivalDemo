using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinGenerateTest : MonoBehaviour {

	public SkinGenerater generater;
	// Use this for initialization
	int i=1;
	int j=1;

	void Start () {
		generater = GetComponent<SkinGenerater> ();
	}

	string[] s={"Backpack1","Backpack2","Backpack3"};
	string[] l = { "Pants1","Pants2","Pants3"};
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			generater.ChangeSkin (s[i],SkinType.BACKPACK);
			i++;
			i = i%3;
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			generater.ChangeSkin (l[j],SkinType.PAINTS);
			j++;
			j = j%3;
		}
	}
}
