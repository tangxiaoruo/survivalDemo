using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

	public Transform player;
	public Camera UIPlayerCam;
	public Camera UICam;
	public Transform buildPanelUI;
	public Transform buildPanelBG;
	//public Transform buildPanelMask;
	public Transform bagPanel;
	public Transform buildPanel2;
	public Transform tips;
	public Transform bg;
	public GameObject buildingUI;
	public Transform healthBar;

	public PlayerController moveController;
	public ComposeSystem composeSys;
	public BuildSystem buildSys;
	public GameObject buildingsPrefab;

	public GameObject[] baseItemPrefabs;
	public GameObject[] itemPrefabs;
	public Transform[] goodCells;

	public GameObject modelPrefab;
	[Range(0,10)]
	public int randomMultiplier=5;

	public  bool buildPanelOpen=false;
	bool UIShowed=false;
	bool buildPanel2Open=false;
	bool EOpened=false;
	GoodsInfo goodsInfo=null;

	Vector2 tipsPos;
	private List<GameObject> generateList=new List<GameObject>();
	// Use this for initialization
	void Start () {
		UIPlayerCam.enabled = false;
		UICam.enabled = false;
		UIShowed = false;

		//有可能buildSys的start方法再次之前运行，所以在这里调用InitBuildings（）（该方法里需要buildSys里的某些值）方法有可能出现bug；
		//InitBuildings ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.C)) {
			if (!buildPanelOpen) {
				ShowBuildPanelUI (true);
			} else {
				ShowBuildPanelUI (false);
			}
		}


		if (Input.GetKeyDown (KeyCode.Tab)) {
			if (!UIShowed) {
				ShowUI (true);
			} else {
				if (buildPanelOpen)
					ShowBuildPanelUI (false);

				ShowUI (false);
				if (EOpened) {
					ClearAndCopyItems ();
					EOpened = false;
				}
			}

		}
		if (UIShowed && Input.GetKeyDown (KeyCode.E)) {
			ShowUI (false);
			ClearAndCopyItems ();
			EOpened = false;
		}

		if (Input.GetKeyDown(KeyCode.B)) {
			if (buildPanel2Open) {
				ShowBuildingsUI (false);
			}else{
				ShowBuildingsUI(true);
			}
		}
	}

	public void InitBuildings(){
		GameObject obj = null;
		Transform grid = null;
	
		for (int n = 0; n < buildPanel2.childCount; n++) {
			if (buildPanel2.GetChild (n).name == "grid")
				grid = buildPanel2.GetChild (n);
		}

		foreach (ComponentInfo info in buildSys.buildingList) {
			obj = Instantiate (buildingUI)as GameObject;

			obj.name = info.name;
			obj.transform.parent = grid;
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = Vector3.zero;
			obj.AddComponent<MyUIButton> ();
			obj.GetComponent<UISprite> ().spriteName = info.name;
			obj.GetComponent<MyUIButton> ().normalSprite = info.name;

			EventDelegate delegate1=new EventDelegate(buildSys,"OnBuildClick") ;
			delegate1.parameters[0] = new EventDelegate.Parameter (obj,"");
			obj.GetComponent<MyUIButton> ().onClick.Add(delegate1);
			obj.GetComponent<MyUIButton> ().tips = tips;
			Debug.Log ("111");
		}

		grid.GetComponent<UIGrid> ().Reposition ();

	}

//	public void OnMouseHover(){
//
//		tipsPos = Input.mousePosition-new Vector3(Screen.width/2,Screen.height/2,0f);
//		Debug.Log (Screen.width+"--"+Screen.height);
//		tips.localPosition = new Vector3 (tipsPos.x, tipsPos.y, 0f);
//	}

	public void ShowBuildingsUI(bool show){
		if(show){
			UICam.enabled = true;
			if (!UIShowed) {
				bagPanel.GetComponent<UIPanel> ().alpha = 0f;
			//	UIPlayerCam.enabled = false;
			}
			buildPanel2.GetComponent<UIPanel> ().alpha = 1.0f;
			bg.gameObject.SetActive (false);
		}else{
			if (!UIShowed) {
				bagPanel.GetComponent<UIPanel>().alpha=1.0f;
				UICam.enabled = false;

			} 
			buildPanel2.GetComponent<UIPanel> ().alpha =0f;
			bg.gameObject.SetActive (true);
		}
		buildPanel2Open = show;
	}

	public IEnumerator InstantiateItemModel(string name,int itemNum){
		ItemObj info;

		while (true) {
			GameObject obj = Instantiate (modelPrefab)as GameObject;
			obj.transform.position = player.transform.position + player.transform.forward + Vector3.up;
			info = obj.GetComponent<ItemObj> ();
			info.name = name;

			yield return new WaitForSeconds (0.3f);
			if (itemNum <= 100) {
				info.number = itemNum;
				break;
			} else {
				info.number = 100;
				itemNum -= 100;
			}

		}
	}

	void RandomGenerate(int level){
		float ran = Random.Range (1.0f,3.0f);;
		int goodsNum = level * (int)(ran-((ran*10)%10)/10f)+(randomMultiplier-5);
		for (int i = 0; i < goodsNum; i++) {
			int index=(int)Random.Range (0.0f, (float)baseItemPrefabs.Length);
			GameObject obj = Instantiate (baseItemPrefabs [index])as GameObject;
			obj.name = baseItemPrefabs [index].name;
			obj.AddComponent<DragItemController> ();

			Transform tagCell=GetEmptyCell();
			if (tagCell != null) {
				obj.transform.parent = tagCell;
				obj.transform.localScale = Vector3.one;
				obj.transform.localPosition = Vector3.zero;
				obj.GetComponentInChildren<UILabel> ().text =(int)Random.Range(1.0f,8.0f+(float)(randomMultiplier-5)) + "";	
				generateList.Add (obj);

			} else
				Debug.Log ("goodsCell is full!");
		}

	}



	public GameObject GetPrefab(string name){
		for (int i = 0; i < itemPrefabs.Length; i++) {
			if (itemPrefabs [i].name == name) {
				return itemPrefabs [i];
			}
		}

		return null;
	}


	Transform GetEmptyCell(){
		for (int i = 0; i < goodCells.Length; i++) {
			if (goodCells [i].childCount == 0)
				return goodCells [i];
		}
	
		return null;
	}



	public  void GenerateGoods(GoodsInfo info){
		EOpened = true;
		goodsInfo = info;
		UIShowed = true;

		if (info.opened) {
			foreach(string s in info.itemTable.Keys){
				Debug.Log ("..."+s);
				GameObject item = GetPrefab (s);

				if (item!= null) {
					
					if (item.GetComponent<ItemObj> ().skinType == SkinType.NONE) {
						Transform tagCell = GetEmptyCell ();
						if (tagCell != null) {
							GameObject obj = Instantiate (item)as GameObject;
							obj.name = s;
							obj.transform.parent = tagCell;
							obj.transform.localScale = Vector3.one;
							obj.transform.localPosition = Vector3.zero;
							obj.AddComponent<DragItemController> ();

							obj.GetComponentInChildren<UILabel> ().text = info.itemTable [s] + "";
							generateList.Add (obj);
						}
					}else {
							for (int i = 0; i < (int)info.itemTable [s]; i++) {
								Transform tagCell2=GetEmptyCell();
								if (tagCell2 != null) {
									GameObject obj2 = Instantiate (item)as GameObject;
									obj2.name = s;
									obj2.transform.parent = tagCell2;
									obj2.transform.localScale = Vector3.one;
									obj2.transform.localPosition = Vector3.zero;
									generateList.Add (obj2);
									obj2.AddComponent<DragItemController> ();

								}
							}
						}
						//generateList.Add (obj);
						Debug.Log ("sssss");
				} else
					Debug.Log ("no such a good!");
			} 
			return;
		}
			
	



		RandomGenerate (info.randomLevel);

		if (info.nonRandomGoods) {
			for(int i=0;i<info.goodsName.Length;i++){
				GameObject item = GetPrefab (info.goodsName[i]);

				if (item!= null) {
					Transform tagCell=GetEmptyCell();
					if (tagCell != null) {
						GameObject obj = Instantiate (item)as GameObject;
						obj.name = info.goodsName [i];
						obj.transform.parent = tagCell;
						obj.transform.localScale = Vector3.one;
						obj.transform.localPosition = Vector3.zero;
						obj.AddComponent<DragItemController> ();

						if(obj.GetComponent<ItemObj>().skinType==SkinType.NONE)
							obj.GetComponentInChildren<UILabel> ().text = info.goodsNum [i] + "";
						
						generateList.Add (obj);
					} else
						Debug.Log ("goodsCell is full!");
				} else
					Debug.Log ("no such a good!");
					
			}
		}

		info.opened = true;
	}

	void ClearAndCopyItems(){
		goodsInfo.itemTable.Clear ();
		Transform obj = null;
		int num = 0;
		string s=null;
		string name = null;
		for (int i = 0; i < goodCells.Length; i++) {
			if (goodCells[i].childCount!=0) {
				obj = goodCells [i].GetChild (0);

				if (obj.GetComponent<ItemObj> ().skinType == SkinType.NONE) {
					s = obj.GetComponentInChildren<UILabel> ().text;
					if (s == null || s == "")
						num = 0;
					else
						num = int.Parse (s);
				} else
					num = 1;
					
					
			name = obj.GetComponent<ItemObj> ().name;
			if (goodsInfo.itemTable.Contains (name))
					goodsInfo.itemTable [name]	= num+(int)goodsInfo.itemTable [name];
				else
					goodsInfo.itemTable.Add (name,num);	
				
			Destroy (obj.gameObject);
			Debug.Log (obj.GetComponent<ItemObj> ().name);
		}
	}
		generateList.Clear ();
	}

	public void ShowUI(bool open){
		if (open) {
			Camera.main.GetComponent<CameraFollow> ().enabled = false;
			bagPanel.GetComponent<UIPanel> ().alpha = 1.0f;
		}
		else
			Camera.main.GetComponent<CameraFollow> ().enabled = true;
		
		UIPlayerCam.enabled = open;
		UICam.enabled = open;
		UIShowed = open;
		if (!UIShowed)
			StartCoroutine (SC());
		else
			moveController.enabled = !open;


		if (!open) {
			//ShowBuildPanelUI (false);
			ShowBuildingsUI (false);
		}
	}

	IEnumerator SC(){
		yield return null;
		moveController.enabled = true;
	}

	public void ShowHealthBar(bool show){
		if (show) {
			UICam.enabled = true;
			UIPlayerCam.enabled = false;
			bg.GetComponent<UISprite> ().alpha = 0f;
			buildPanelUI.gameObject.SetActive (false);
			buildPanelBG.gameObject.SetActive (false);

			bagPanel.GetComponent<UIPanel> ().alpha = 0f;

			healthBar.gameObject.SetActive (true);
		} else {

			UICam.enabled = false;
			healthBar.gameObject.SetActive (false);
		}
	
	}

  public void ShowBuildPanelUI(bool open){
		buildPanelOpen = open;
		if (!UIShowed)
			ShowUI (true);
		buildPanelUI.gameObject.SetActive (open);
		buildPanelBG.gameObject.SetActive (open);
		//buildPanelMask.gameObject.SetActive (open);

		if (UIShowed) {
			if(open)
			bagPanel.GetComponent<UIPanel> ().alpha = 0.5f;
			else
			bagPanel.GetComponent<UIPanel> ().alpha = 1.0f;
		}


		if (open)
			composeSys.InitComposingSys ();
		else {
			for (int i = 0; i < composeSys.grid.childCount; i++)
				Destroy (composeSys.grid.GetChild(i).gameObject);
		}
			
	}

}
