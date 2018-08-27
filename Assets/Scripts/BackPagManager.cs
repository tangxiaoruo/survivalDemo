using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BackPagType{
	NONE,
	SMALL,
	MID,
	BIG,
	HUGE
}
public class BackPagManager : MonoBehaviour {

	public BackPagType bagType=BackPagType.NONE;
	public Transform[] itemBars;
	public int cellNum = 9;
	public Transform goodsUI;
	public float totalWeight=90.0f;
	private float goodsWeight=0.0f;
	public  UIController uiController;
	public UILabel warningLabel;
	Color color;
	// Use this for initialization
	void Start () {
		color.a = 0;
		warningLabel.color = color;
		InitItemBars ();
	}

	public IEnumerator AlpheAni(){
		for (int i = 0; i < 10; i++) {
			color.a += 0.1f;
			warningLabel.color = color;
			yield return new WaitForSeconds (0.025f);
		}
		for (int i = 0; i < 40; i++) {
			color.a -= 0.025f;
			warningLabel.color = color;
			yield return new WaitForSeconds (0.025f);
		}
		color.a = 0f;
		warningLabel.color = color;
	}


	// Update is called once per frame
	void Update () {
//		if (Input.GetKeyDown (KeyCode.Space))
//			StartCoroutine (AlpheAni());
	}

	public int Reduce(string name,int num){
		int itemNum = 0;
		for (int i = 0; i < cellNum; i++) {
			if (itemBars [i].childCount != 0) {
				if(itemBars[i].GetChild(0).name==name){
					itemNum=int.Parse(itemBars[i].GetChild(0).GetComponentInChildren<UILabel>().text);
					if (itemNum > num) {
						itemBars [i].GetChild (0).GetComponentInChildren<UILabel> ().text = itemNum - num + "";
						return 0;
					} else if (itemNum == num) {
						Destroy (itemBars[i].GetChild(0).gameObject);
						return 0;
					} else if(itemNum<num){
						Destroy (itemBars[i].GetChild(0).gameObject);
						num = num - itemNum;
					}

				}
			}
			
		}


		return num;
	}

	public int  Increase(string name,int num){
		int itemNum=0;

		for (int i = 0; i < cellNum; i++) {
			if (itemBars [i].childCount != 0) {
				if (itemBars [i].GetChild (0).name == name) {
					itemNum=int.Parse(itemBars[i].GetChild(0).GetComponentInChildren<UILabel>().text);
					if (itemNum + num > 100) {
						num = itemNum + num - 100;
						itemBars [i].GetChild (0).GetComponentInChildren<UILabel> ().text = "100";
					} else{
						itemBars [i].GetChild (0).GetComponentInChildren<UILabel> ().text = itemNum+num+"";
						return  0;
					} 
				}
			}
			
		}

		Transform cell=null;
		while (true) {
			cell = GetFristEmptyCell ();
			if (cell == null)
				return num;
			
			Debug.Log (cell.name);
			if (num > 100) {
				if (GenerateItem (cell, name, 100))
					num -= 100;
			} else {
				GenerateItem (cell, name, num);
				return 0;
			}
		}
	}

	public bool GenerateItem(Transform cell,string name,int num){

		if (cell == null || cell.childCount != 0||num>100) {
			Debug.Log ("cell is not empty or cell is null or num>100!");
			return false;
		}

		GameObject obj = uiController.GetPrefab (name);
		GameObject item = Instantiate (obj)as GameObject;

		item.name = name;
		item.transform.parent = cell;
		item.transform.localScale = Vector3.one;
		item.transform.localPosition = Vector3.zero;
		item.GetComponentInChildren<UILabel> ().text = num + "";
		item.AddComponent<DragItemController> ();

		return true;
	}


	void ChangeGoodsWeight(Transform org){
		
	}

//	int CalculateWeight(Transform org){
//		float weight;
//		int num;
//		num =int.Parse(org.GetComponentInChildren<UILabel>().text);
//		weight = num * org.GetComponent<ItemObj> ().weight;
//
//		if (weight + goodsWeight < totalWeight)
//			return num;
//		else {
//			float n = (weight + goodsWeight - totalWeight) / org.GetComponent<ItemObj> ().weight;
//			Debug.Log(n+"...");
//
//			if ((n * 100) % 100 > 0)
//				n = n + 1;
//			Debug.Log(n);
//
//			return num - (int)n;
//		}
//	}

	public int GetItemNum(string name){
		int n = 0;
		for (int i = 0; i < cellNum; i++) {
			if (itemBars [i].childCount!=0) {
				if (itemBars [i].GetChild (0).name == name)
					n += int.Parse(itemBars [i].GetChild (0).GetComponentInChildren<UILabel> ().text);
			}
		}

		return n;
	}

	public  void CombineItemsInside(Transform org,Transform tag){
		int orgNum;
		int tagNum;

		orgNum = int.Parse (org.GetComponentInChildren<UILabel>().text);
		tagNum = int.Parse (tag.GetComponentInChildren<UILabel>().text);

		if (orgNum + tagNum < 100) {
			Destroy (org.gameObject);
			tag.GetComponentInChildren<UILabel> ().text = orgNum + tagNum + "";
		} else {
			tag.GetComponentInChildren<UILabel> ().text = "100";
			org.GetComponentInChildren<UILabel> ().text = orgNum + tagNum - 100+"";

			if (org.IsChildOf (goodsUI)) {
				Transform temp;
				temp = GetFristEmptyCell ();
				if (temp != null) {
					org.parent = temp;
				} else
					StartCoroutine (AlpheAni ());
			}
			
			org.localPosition = Vector3.zero;
		
		}
			
	}

	public Transform GetFristEmptyCell(){
	
		for (int i = 0; i < cellNum; i++) {
			if (itemBars [i].childCount == 0) {
				return itemBars [i];
			}
		}

		return null;
	}

	void InitItemBars(){

		UISprite cellSprite;
		BoxCollider collider;

		if (bagType == BackPagType.NONE)
			cellNum = 9;
		if (bagType == BackPagType.SMALL)
			cellNum = 14;
		if (bagType == BackPagType.MID)
			cellNum = 21;
		if (bagType == BackPagType.BIG||bagType==BackPagType.HUGE)
			cellNum = 28;

		totalWeight = cellNum * 10.0f;

		for (int i = cellNum; i < itemBars.Length; i++) {
		
			cellSprite = itemBars [i].GetComponent<UISprite> ();
			collider = itemBars [i].GetComponent<BoxCollider> ();
			collider.enabled = false;
			cellSprite.color = Color.black;
		}
	
	}
}
