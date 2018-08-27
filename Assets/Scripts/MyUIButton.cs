using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUIButton : UIButton{
	public Transform tips=null;
	protected override void OnHover (bool isOver)
	{
		
		
		base.OnHover (isOver);
		if (isOver) {
			tips.gameObject.SetActive (true);
			tips.GetComponent<ClickHover> ().OnMouseOver (transform);
		} else {
			tips.gameObject.SetActive (false);
		}

	}

}
