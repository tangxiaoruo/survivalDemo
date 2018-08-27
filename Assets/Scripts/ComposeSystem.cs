using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComposeSystem : MonoBehaviour {


	public List<ComponentInfo> components=new List<ComponentInfo>();
	public BackPagManager backPackManager;
	public Transform equipment;
	public Transform composeUI;
	public Camera uiCamera;
	public UIController uiController;

	public Transform grid;
	public GameObject itemPrefab;
	public GameObject needsPrefab;


	ComponentInfo comp1;
	ComponentInfo comp2;
	ComponentInfo comp3;

	private bool uiOpened = false;

	// Use this for initialization
	void Start () {
		comp1 = new ComponentInfo ("ammo",new string[2]{"shellCase","powder"},new int[2]{1,1},null);
		comp2 = new ComponentInfo ("board",new string[1]{"wood"},new int[1]{2},new string[1]{"axe"});
		comp3 = new ComponentInfo ("friedMeat",new string[1]{"freshMeat"},new int[1]{1},new string[1]{"campFire"});


		components.Add (comp1);
		components.Add (comp2);
		components.Add (comp3);


		//InitComposingSys ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

//	void OnDisable(){
//		for (int i = 0; i < grid.childCount; i++)
//			Destroy (grid.GetChild(i).gameObject);
//
//	}
//
//	void OnEnable(){
//		InitComposingSys ();
//	}

	ComponentInfo GetComponentInfo(string name){
	
		foreach (ComponentInfo info in components) {
			if (info.name == name)
				return  info;
		}

		Debug.Log ("can't find ComponentInfo in list!");
		return null;
	
	}


	public void InitComposingSys(){
		Transform itemUI=null;
		Transform needListUI=null;
		Transform temp=null;
		Transform needsGrid=null;
		Transform button=null;
		GameObject item;

		foreach (ComponentInfo componentInfo in components) {
			item = Instantiate (itemPrefab)as GameObject;
			item.name = componentInfo.name;
			item.transform.parent = grid;
			item.transform.localScale = Vector3.one;

			for (int n = 0; n < item.transform.childCount; n++) {
				temp=item.transform.GetChild(n);
				if (temp.name == "item")
					itemUI = temp;
				if (temp.name == "button")
					button = temp;
				if (temp.name == "needList")
					needListUI = temp;
			}
			EventDelegate delegate1=new EventDelegate(this,"OnBuildClick") ;
			delegate1.parameters[0] = new EventDelegate.Parameter (item.transform,"");
			button.GetComponent<UIButton> ().onClick.Add(delegate1);
			if (itemUI != null) {
				itemUI.GetComponent<UISprite> ().spriteName = componentInfo.name;
				itemUI.GetComponentInChildren<UILabel> ().text = componentInfo.name;
			}else
				Debug.Log ("can't find \"item\" transform!");

			for (int i = 0; i < componentInfo.requiredGoods.Length; i++) {
				GameObject need = Instantiate (needsPrefab)as GameObject;
				need.GetComponentInChildren<UILabel> ().text = componentInfo.requiredGoodsNum [i] + "";
				need.GetComponent<UISprite> ().spriteName = componentInfo.requiredGoods [i];
				need.name=componentInfo.requiredGoods [i];

				if (!CheckGoodsNumber (componentInfo.requiredGoods [i], componentInfo.requiredGoodsNum [i])) {
					need.GetComponentInChildren<UILabel> ().color = Color.red;
					//button.GetComponent<BoxCollider> ().enabled = false;
				}
				
				//获取needList下的grid子物体
				if (needListUI != null) {
					needsGrid = needListUI.GetChild (0).GetChild (0);
					need.transform.parent = needsGrid;
					need.transform.localScale = Vector3.one;

				} else
					Debug.Log ("can't find \"needList\" transform!");

			}
			needsGrid.GetComponent<UIGrid> ().Reposition ();
		}

		grid.GetComponent<UIGrid> ().Reposition ();
	}


	bool CheckGoodsNumber(string name,int num){
		int totalNum = 0;
		for (int i = 0; i < backPackManager.cellNum; i++) {
			if (backPackManager.itemBars [i].childCount != 0) {
				Transform obj = backPackManager.itemBars [i].GetChild (0);

				if (obj != null) {
					if (obj.GetComponent<ItemObj> ().name == name)
						totalNum += int.Parse (obj.GetComponentInChildren<UILabel> ().text);

					if (totalNum >= num)
						return true;
				}

			}
		}
		return false;
	}


	public void OnBuildClick(Transform obj){
		Transform button = null;
		Transform grid=null;
		Transform numUI = null;
		Transform temp;
		bool canBuild = true;

		for (int i = 0; i < obj.childCount; i++) {
			temp = obj.GetChild (i);
			if (temp.name == "num")
				numUI = temp;
			if (temp.name == "button")
				button = temp;
			if (temp.name == "needList")
				grid = temp.GetChild (0).GetChild (0);
		}

		int num = int.Parse (numUI.GetComponentInChildren<UILabel> ().text);
		Debug.Log ("nnnnum");
		ComponentInfo componentInfo = GetComponentInfo (obj.name);

		for(int n=0;n<componentInfo.requiredGoods.Length;n++){
			Transform need=null;
			need = GetChildWithName (grid,componentInfo.requiredGoods[n]);

			if (!CheckGoodsNumber (componentInfo.requiredGoods [n], componentInfo.requiredGoodsNum [n] * num)) {
				need.GetComponentInChildren<UILabel> ().color = Color.red;
				canBuild = false;
			}
			else 
				need.GetComponentInChildren<UILabel> ().color = Color.black;
			

			need.GetComponentInChildren<UILabel> ().text = componentInfo.requiredGoodsNum [n] * num + "";

		}

		if (canBuild) {
			
			StartCoroutine (BuildItem(grid,obj.name,num));
		}

	}

	IEnumerator BuildItem(Transform grid,string name,int num){
		for (int m = 0; m < grid.childCount; m++) {
			backPackManager.Reduce (grid.GetChild (m).name, int.Parse (grid.GetChild (0).GetComponentInChildren<UILabel> ().text));
		}

		yield return new WaitForSeconds (0.3f);

		int n=backPackManager.Increase (name, num);


		if (n !=0){
			StartCoroutine (backPackManager.AlpheAni());
			uiController.ShowBuildPanelUI (false);
			StartCoroutine (uiController.InstantiateItemModel (name, n));
		}
	}

	Transform GetChildWithName(Transform parent,string name){
		Transform temp = null;
		for (int i = 0; i < parent.childCount; i++) {
			temp = parent.GetChild (i);
			if (temp.name == name)
				return temp;
		}

		return null;
	}
}
